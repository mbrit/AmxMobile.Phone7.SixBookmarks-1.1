using System;
using System.Data.Services.Client;
using System.Collections.Generic;
using AMX = AmxMobile.Services;

namespace AmxMobile.Phone7.SixBookmarks
{
    internal class BookmarksService : ODataServiceProxy
    {
        internal BookmarksService()
            : base("Bookmarks.svc/")
        {
        }

        private class QueryState
        {
            internal DataServiceQuery<AMX.Bookmark> Query;
            internal Action<List<Bookmark>> Success;
            internal Failed Failed;
        }

        private AMX.BookmarkCollection GetServerTarget()
        {
            // ask the server for a collection...
            AMX.BookmarkCollection serverTarget = new AMX.BookmarkCollection(new Uri(this.ResolvedServiceUrl));

            // set up the proxy so that our special headers go up...
            ConfigureODataProxy(serverTarget);

            // return...
            return serverTarget;
        }

        internal ExecuteServerItemsState ExecuteServerItems(AsyncCallback callback, Failed failed)
        {
            // state...
            ExecuteServerItemsState state = new ExecuteServerItemsState();
            state.ServerTarget = this.GetServerTarget();
            if (state.ServerTarget == null)
                throw new InvalidOperationException("'state.Bookmarks' is null.");

            // run...
            state.Query = state.ServerTarget.CreateQuery<AMX.Bookmark>("Bookmark");
            state.Query.BeginExecute(callback, failed);

            // erturn...
            return state;
        }

        internal void GetAll(Action<List<Bookmark>> callback, Failed failed)
        {
            // ask the server for a collection...
            AMX.BookmarkCollection serverItems = this.GetServerTarget();

            // create some state...
            QueryState state = new QueryState();
            state.Query = serverItems.CreateQuery<AMX.Bookmark>("Bookmark");
            state.Success = callback;
            state.Failed = failed;

            // run it, passing in the state object to be unwrapped in the callback handler...
            state.Query.BeginExecute(new AsyncCallback(ServerQueryFinished), state);
        }

        private void ServerQueryFinished(IAsyncResult result)
        {
            QueryState state = (QueryState)result.AsyncState;   
            try
            {
                // read the server items...
                List<Bookmark> results = new List<Bookmark>();
                foreach (AMX.Bookmark serverItem in state.Query.EndExecute(result))
                    results.Add(new Bookmark(serverItem));

                // ok - send it back...
                state.Success(results);
            }
            catch (Exception ex)
            {
                state.Failed(ex);
            }
        }
    }
}
