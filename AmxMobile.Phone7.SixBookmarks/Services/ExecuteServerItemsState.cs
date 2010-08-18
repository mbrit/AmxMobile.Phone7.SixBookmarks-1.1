using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using AMX = AmxMobile.Services;
using System.Data.Services.Client;

namespace AmxMobile.Phone7.SixBookmarks
{
    internal class ExecuteServerItemsState
    {
        internal AMX.BookmarkCollection ServerTarget { get; set; }
        internal DataServiceQuery<AMX.Bookmark> Query;
    }
}
