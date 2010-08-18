//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Original file name: BookmarksClient.cs
// Generation date: 20/07/2010 13:25:23
namespace AmxMobile.Services
{
    
    /// <summary>
    /// There are no comments for BookmarkCollection in the schema.
    /// </summary>
    public partial class BookmarkCollection : global::System.Data.Services.Client.DataServiceContext
    {
        /// <summary>
        /// Initialize a new BookmarkCollection object.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public BookmarkCollection(global::System.Uri serviceRoot) : 
                base(serviceRoot)
        {
            this.OnContextCreated();
        }
        partial void OnContextCreated();
        /// <summary>
        /// There are no comments for Bookmark in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceQuery<Bookmark> Bookmark
        {
            get
            {
                if ((this._Bookmark == null))
                {
                    this._Bookmark = base.CreateQuery<Bookmark>("Bookmark");
                }
                return this._Bookmark;
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceQuery<Bookmark> _Bookmark;
        /// <summary>
        /// There are no comments for Bookmark in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public void AddToBookmark(Bookmark bookmark)
        {
            base.AddObject("Bookmark", bookmark);
        }
    }
    /// <summary>
    /// There are no comments for AmxMobile.Services.Bookmark in the schema.
    /// </summary>
    /// <KeyProperties>
    /// BookmarkId
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("Bookmark")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("BookmarkId")]
    public partial class Bookmark : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Create a new Bookmark object.
        /// </summary>
        /// <param name="bookmarkId">Initial value of BookmarkId.</param>
        /// <param name="userId">Initial value of UserId.</param>
        /// <param name="ordinal">Initial value of Ordinal.</param>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Bookmark CreateBookmark(int bookmarkId, int userId, int ordinal)
        {
            Bookmark bookmark = new Bookmark();
            bookmark.BookmarkId = bookmarkId;
            bookmark.UserId = userId;
            bookmark.Ordinal = ordinal;
            return bookmark;
        }
        /// <summary>
        /// There are no comments for Property BookmarkId in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public int BookmarkId
        {
            get
            {
                return this._BookmarkId;
            }
            set
            {
                this.OnBookmarkIdChanging(value);
                this._BookmarkId = value;
                this.OnBookmarkIdChanged();
                this.OnPropertyChanged("BookmarkId");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private int _BookmarkId;
        partial void OnBookmarkIdChanging(int value);
        partial void OnBookmarkIdChanged();
        /// <summary>
        /// There are no comments for Property UserId in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public int UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                this.OnUserIdChanging(value);
                this._UserId = value;
                this.OnUserIdChanged();
                this.OnPropertyChanged("UserId");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private int _UserId;
        partial void OnUserIdChanging(int value);
        partial void OnUserIdChanged();
        /// <summary>
        /// There are no comments for Property Name in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this._Name = value;
                this.OnNameChanged();
                this.OnPropertyChanged("Name");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        /// <summary>
        /// There are no comments for Property Url in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Url
        {
            get
            {
                return this._Url;
            }
            set
            {
                this.OnUrlChanging(value);
                this._Url = value;
                this.OnUrlChanged();
                this.OnPropertyChanged("Url");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Url;
        partial void OnUrlChanging(string value);
        partial void OnUrlChanged();
        /// <summary>
        /// There are no comments for Property Ordinal in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public int Ordinal
        {
            get
            {
                return this._Ordinal;
            }
            set
            {
                this.OnOrdinalChanging(value);
                this._Ordinal = value;
                this.OnOrdinalChanged();
                this.OnPropertyChanged("Ordinal");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private int _Ordinal;
        partial void OnOrdinalChanging(int value);
        partial void OnOrdinalChanged();
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
}
