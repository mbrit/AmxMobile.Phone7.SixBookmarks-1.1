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
using System.Collections.Generic;

namespace AmxMobile.Phone7.SixBookmarks
{
    public class DownloadSettings
    {
        public Dictionary<string, string> ExtraHeaders { get; private set; }

        public DownloadSettings()
        {
            this.ExtraHeaders = new Dictionary<string, string>();
        }
    }
}
