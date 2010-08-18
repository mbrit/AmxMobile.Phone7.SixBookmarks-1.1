using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Services.Client;
using AMX = AmxMobile.Services;

namespace AmxMobile.Phone7.SixBookmarks
{
    public class Sync
    {
        private Action Callback { get; set; }
        private Failed Failed { get; set; }
        private ExecuteServerItemsState PushChangesState {get; set; }

        public Sync()
        {
        }

        public void DoSync(Action callback, Failed failed)
        {
            // set...
            this.Callback = callback;
            this.Failed = failed;

            // push up the changes...
            this.PushChanges();
        }

        private void PushChanges()
        {
            // call the OData service directly.  we need an actual instance of the collection bound to the server
            // as we need to change the items on this and send them back...
            BookmarksService service = new BookmarksService();
            this.PushChangesState = service.ExecuteServerItems(new AsyncCallback(ReceiveServerItemsForPushChanges), this.Failed);
            if (PushChangesState == null)
                throw new InvalidOperationException("'PushChangesState' is null.");
        }

        private void ReceiveServerItemsForPushChanges(IAsyncResult result)
        {
            try
            {
                if (PushChangesState == null)
                    throw new InvalidOperationException("'PushChangesState' is null.");

                // unwrap the server response...
                var response = new List<AMX.Bookmark>(this.PushChangesState.Query.EndExecute(result));

                // we can only enumerate the server repsonse once - so we'll store it in a separate list 
                // so that we can loop it several times...
                List<AMX.Bookmark> serverItems = new List<AMX.Bookmark>();
                foreach (AMX.Bookmark serverItem in response)
                    serverItems.Add(serverItem);

                // get the local changes...
                List<Bookmark> updates = Bookmark.GetBookmarksForServerUpdate();
                List<Bookmark> deletes = Bookmark.GetBookmarksForServerDelete();

                // get an entity type...
                EntityType et = EntityType.GetEntityType(typeof(Bookmark));
                if (et == null)
                    throw new InvalidOperationException("'et' is null.");

                // we do have changes, so get the latest from the server...
                foreach (Bookmark localItem in updates)
                {
                    // find it in our set...
                    Bookmark toUpdateLocal = null;
                    AMX.Bookmark toUpdateServer = null;
                    foreach (AMX.Bookmark serverItem in serverItems)
                    {
                        if (localItem.Ordinal == serverItem.Ordinal)
                        {
                            toUpdateLocal = localItem;
                            toUpdateServer = serverItem;
                            break;
                        }
                    }

                    // did we have one to change?
                    if (toUpdateLocal != null)
                    {
                        if (toUpdateServer == null)
                            throw new InvalidOperationException("'toUpdateServer' is null.");

                        // patch the values...
                        toUpdateServer.Name = toUpdateLocal.Name;
                        toUpdateServer.Url = toUpdateLocal.Url;

                        // update...
                        this.PushChangesState.ServerTarget.UpdateObject(toUpdateServer);
                    }
                    else
                    {
                        // create a new item for the server...
                        AMX.Bookmark newItem = new AMX.Bookmark();
                        newItem.Ordinal = localItem.Ordinal;
                        newItem.Name = localItem.Name;
                        newItem.Url = localItem.Url;

                        // add...
                        this.PushChangesState.ServerTarget.AddObject(et.NativeName, newItem);
                    }
                }

                // what about ones to delete?
                foreach (Bookmark local in deletes)
                {
                    // find a matching ordinal on the server...
                    foreach (AMX.Bookmark serverItem in serverItems)
                    {
                        if (local.Ordinal == serverItem.Ordinal)
                            this.PushChangesState.ServerTarget.DeleteObject(serverItem);
                    }
                }

                // call up and save changes...
                this.PushChangesState.ServerTarget.BeginSaveChanges((AsyncCallback)delegate(IAsyncResult saveResult)
                {
                    try
                    {
                        // unwrap...
                        this.PushChangesState.ServerTarget.EndSaveChanges(saveResult);

                        // great - we've saved changes, now get latest...
                        this.GetLatest();
                    }
                    catch (Exception ex)
                    {
                        this.Failed(ex);
                    }

                }, null);
            }
            catch (Exception ex)
            {
                this.Failed(ex);
            }
        }

        //private void Doo()
        //{
        //    // need to get all from the server - we need to calculate a delta...
        //    BookmarksService service = new BookmarksService();
        //    service.GetAll((Action<List<Bookmark>>)delegate(List<Bookmark> fromServer)
        //    {
        //        // get the local set...
        //        List<Bookmark> updates = Bookmark.GetBookmarksForServerUpdate();
        //        List<Bookmark> deletes = Bookmark.GetBookmarksForServerDelete();

        //        // et...
        //        EntityType et = EntityType.GetEntityType(typeof(Bookmark));
        //        if (et == null)
        //            throw new InvalidOperationException("'et' is null.");

        //        // reset the work items...
        //        this.WorkItems = new List<SyncWorkItem>();

        //        // we do have changes, so get the latest from the server...
        //        foreach (Bookmark local in updates)
        //        {
        //            // find it in our set...
        //            Bookmark toUpdate = null;
        //            foreach (Bookmark server in fromServer)
        //            {
        //                if (local.Ordinal == server.Ordinal)
        //                {
        //                    toUpdate = server;
        //                    break;
        //                }
        //            }

        //            // did we have one to change?
        //            if (toUpdate != null)
        //            {
        //                // walk the fields...
        //                int serverId = 0;
        //                foreach (EntityField field in et.Fields)
        //                {
        //                    if (!(field.IsKey))
        //                        toUpdate.SetValue(field, local.GetValue(field), SetReason.UserSet);
        //                    else
        //                        serverId = toUpdate.BookmarkId;
        //                }

        //                // send that up...
        //                this.WorkItems.Add(new SyncWorkItem(ODataOperation.Update, toUpdate, serverId));
        //            }
        //            else
        //            {
        //                // we need to insert it...
        //                this.WorkItems.Add(new SyncWorkItem(ODataOperation.Insert, local, 0));
        //            }
        //        }

        //        // what about ones to delete?
        //        foreach (Bookmark local in deletes)
        //        {
        //            // find a matching ordinal on the server...
        //            foreach (Bookmark server in fromServer)
        //            {
        //                if (local.Ordinal == server.Ordinal)
        //                    this.WorkItems.Add(new SyncWorkItem(ODataOperation.Delete, server, server.BookmarkId));
        //            }
        //        }

        //        // reset the queue and run it...
        //        this.WorkItemIndex = 0;
        //        this.PushNextWorkItem();

        //    }, this.Failed);
        //}

        private void GetLatest()
        {
            BookmarksService service = new BookmarksService();
            service.GetAll((Action<List<Bookmark>>)delegate(List<Bookmark> bookmarks)
            {
                // delete first...
                Bookmark.DeleteAll();

                // go through and save them...
                foreach (Bookmark bookmark in bookmarks)
                    bookmark.SaveChanges();

                // signal that we've finished...
                this.Callback();

            }, this.Failed);
        }
    }
}
