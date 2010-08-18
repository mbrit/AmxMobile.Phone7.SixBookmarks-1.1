using System;
using System.Collections.Generic;

namespace AmxMobile.Phone7.SixBookmarks
{
    public class ConfigurePageViewModel
    {
        public List<Bookmark> Items { get; private set; }

        internal ConfigurePageViewModel()
        {
            // set...
            List<Bookmark> items = new List<Bookmark>(Bookmark.GetBookmarksForDisplay());

            // sort...
            items.Sort(new OrdinalComparer());

            // set...
            this.Items = items;
        }

        private class OrdinalComparer : IComparer<Bookmark>
        {
            public int Compare(Bookmark x, Bookmark y)
            {
                if (x.Ordinal < y.Ordinal)
                    return -1;
                else if (x.Ordinal > y.Ordinal)
                    return 1;
                else
                    return 0;
            }
        }
    }
}
