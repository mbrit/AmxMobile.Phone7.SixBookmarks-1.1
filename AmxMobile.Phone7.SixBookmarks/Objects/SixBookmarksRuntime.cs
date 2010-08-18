using System;

namespace AmxMobile.Phone7.SixBookmarks
{
    public class SixBookmarksRuntime
    {
        /// <summary>
		/// Private field to hold singleton instance.
		/// </summary>
		private static SixBookmarksRuntime _current = new SixBookmarksRuntime();
		
		// settings...
        public SimpleXmlPropertyBag Settings { get; private set; }

		/// <summary>
		/// Private constructor.
		/// </summary>
		private SixBookmarksRuntime()
		{
            // settings...
            this.Settings = SimpleXmlPropertyBag.Load("Settings.xml", false);

            // register the entity type...
            EntityType bookmark = new EntityType(typeof(Bookmark), "Bookmark");
            bookmark.AddField(Bookmark.BookmarkIdKey, Bookmark.BookmarkIdKey, DataType.Int32, -1).IsKey = true;
            bookmark.AddField(Bookmark.NameKey, Bookmark.NameKey, DataType.String, 128);
            bookmark.AddField(Bookmark.UrlKey, Bookmark.UrlKey, DataType.String, 128);
            bookmark.AddField(Bookmark.OrdinalKey, Bookmark.OrdinalKey, DataType.Int32, -1);
            bookmark.AddField(Bookmark.IsLocalModifiedKey, Bookmark.IsLocalModifiedKey, DataType.Boolean, -1);
            bookmark.AddField(Bookmark.IsLocalDeletedKey, Bookmark.IsLocalDeletedKey, DataType.Boolean, -1);
            EntityType.RegisterEntityType(bookmark);
		}
						
		/// <summary>
		/// Gets the singleton instance of <see cref="SixBookmarksRuntime">SixBookmarksRuntime</see>.
		/// </summary>
		internal static SixBookmarksRuntime Current
		{
			get
			{
				if(_current == null)
					throw new ObjectDisposedException("SixBookmarksRuntime");
				return _current;
			}
		}
    }
}
