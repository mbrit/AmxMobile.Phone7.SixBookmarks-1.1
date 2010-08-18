using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace AmxMobile.Phone7.SixBookmarks
{
    public class ErrorBucket : List<string>
    {
        public ErrorBucket()
        {
        }

        public bool HasErrors
        {
            get
            {
                if (this.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public string GetAllErrorsSeparatedByCrLf()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string error in this)
            {
                if (builder.Length > 0)
                    builder.Append("\r\n");
                builder.Append(error);
            }

            // return...
            return builder.ToString();
        }
    }
}
