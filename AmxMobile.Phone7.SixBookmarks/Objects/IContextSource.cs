using System;
using System.Windows.Threading;

namespace AmxMobile.Phone7.SixBookmarks
{
    public interface IContextSource
    {
        Dispatcher Dispatcher
        {
            get;
        }
    }
}
