using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AmxMobile.Phone7.SixBookmarks
{
    public static class Alert
    {
        private const string Caption = "Six Bookmarks";

        internal static void Show(object context, Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            // defer...
            Show(context, "An error occurred.", ex);
        }

        internal static void Show(object context, string message)
        {
            // defer...
            Show(context, message, null);
        }

        internal static void Show(object context, string message, Exception ex)
        {
            string toShow = message;
            if (ex != null)
                toShow = string.Concat(message, "\r\n", ex.ToString());

            // do we need to marshal?
            if (context is IContextSource)
            {
                Dispatcher dispatcher = ((IContextSource)context).Dispatcher;
                if(dispatcher != null)
                {
                    dispatcher.BeginInvoke((Action)delegate()
                    {
                        MessageBox.Show(toShow, Caption, MessageBoxButton.OK);
                    });
                    return;
                }
            }

            // show it, and hope we're on the right thread!
            MessageBox.Show(toShow, Caption, MessageBoxButton.OK);
        }

        internal static Failed GetFailedHandler(IContextSource context)
        {
            // return...
            FailedHandler handler = new FailedHandler(context);
            return new Failed(handler.Failed);
        }

        private class FailedHandler
        {
            private IContextSource Context { get; set; }

            internal FailedHandler(IContextSource context)
            {
                this.Context = context;
            }

            internal void Failed(Exception ex)
            {
                Alert.Show(this.Context, "An error occurred.", ex);
            }
        }
    }
}
