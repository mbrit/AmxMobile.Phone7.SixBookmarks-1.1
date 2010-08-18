using System;
using System.Collections.Generic;
using AMX = AmxMobile.Services;

namespace AmxMobile.Phone7.SixBookmarks
{
    public class Bookmark : Entity
    {
        public const string BookmarkIdKey = "BookmarkId";
        public const string OrdinalKey = "Ordinal";
        public const string NameKey = "Name";
        public const string UrlKey = "Url";
        public const string IsLocalModifiedKey = "IsLocalModified";
        public const string IsLocalDeletedKey = "IsLocalDeleted";

        public Bookmark()
        {
        }

        internal Bookmark(AMX.Bookmark serverItem)
        {
            if (serverItem == null)
                throw new ArgumentNullException("serverItem");

            // copy the properties...
            this.Name = serverItem.Name;
            this.Url = serverItem.Url;
            this.Ordinal = serverItem.Ordinal;
        }

        public int BookmarkId
        {
            get
            {
                return this.GetInt32Value(BookmarkIdKey);
            }
            set
            {
                this.SetValue(BookmarkIdKey, value, SetReason.UserSet);
            }
        }

        public string Name
        {
            get
            {
                return this.GetStringValue(NameKey);
            }
            set
            {
                this.SetValue(NameKey, value, SetReason.UserSet);
            }
        }

        public string Url
        {
            get
            {
                return this.GetStringValue(UrlKey);
            }
            set
            {
                this.SetValue(UrlKey, value, SetReason.UserSet);
            }
        }

        public int Ordinal
        {
            get
            {
                return this.GetInt32Value(OrdinalKey);
            }
            set
            {
                this.SetValue(OrdinalKey, value, SetReason.UserSet);
            }
        }

        public bool IsLocalModified
        {
            get
            {
                return this.GetBooleanValue(IsLocalModifiedKey);
            }
            set
            {
                this.SetValue(IsLocalModifiedKey, value, SetReason.UserSet);
            }
        }

        public bool IsLocalDeleted
        {
            get
            {
                return this.GetBooleanValue(IsLocalDeletedKey);
            }
            set
            {
                this.SetValue(IsLocalDeletedKey, value, SetReason.UserSet);
            }
        }

        internal static List<Bookmark> GetBookmarksForDisplay()
        {
            // create a filter...
            DataBoxFilter filter = new DataBoxFilter(GetDataBox());
            filter.AddConstraint("islocaldeleted", false);

            // return...
            return filter.ExecuteEntityCollection<Bookmark>();
        }

        private static DataBox GetDataBox()
        {
            EntityType et = EntityType.GetEntityType(typeof(Bookmark));
            return new DataBox(et);
        }

        internal static void DeleteAll()
        {
            DataBoxFilter filter = new DataBoxFilter(GetDataBox());
            foreach(Bookmark bookmark in filter.ExecuteEntityCollection<Bookmark>())
            {
                bookmark.MarkForDeletion();
                bookmark.SaveChanges();
            }
        }

        internal static Bookmark GetByOrdinal(int ordinal)
        {
            DataBoxFilter filter = new DataBoxFilter(GetDataBox());
            filter.AddConstraint("ordinal", ordinal);

            // return...
            return filter.ExecuteEntity<Bookmark>();
        }

        internal static List<Bookmark> GetBookmarksForServerUpdate()
        {
            DataBoxFilter filter = new DataBoxFilter(GetDataBox());
            filter.AddConstraint("islocalmodified", true);
            filter.AddConstraint("islocaldeleted", false);

            // return...
            return filter.ExecuteEntityCollection<Bookmark>();
        }

        internal static List<Bookmark> GetBookmarksForServerDelete()
        {
            DataBoxFilter filter = new DataBoxFilter(GetDataBox());
            filter.AddConstraint("islocaldeleted", true);

            // return...
            return filter.ExecuteEntityCollection<Bookmark>();
        }
    }
}
