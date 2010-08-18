using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AmxMobile.Phone7.SixBookmarks
{
    internal class SyncWorkItem
    {
        internal ODataOperation Operation { get; private set; }
        internal Bookmark Bookmark { get; private set; }
        internal int ServerId { get; private set; }

        internal SyncWorkItem(ODataOperation op, Bookmark bookmark, int serverId)
        {
            if (bookmark == null)
                throw new ArgumentNullException("bookmark");

            this.Operation = op;
            this.Bookmark = bookmark;
            this.ServerId = serverId;
        }
    }
}
