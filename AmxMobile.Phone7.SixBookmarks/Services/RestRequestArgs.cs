using System;
using System.Collections.Generic;

namespace AmxMobile.Phone7.SixBookmarks
{
    internal class RestRequestArgs : Dictionary<string, string>
    {
        internal RestRequestArgs(string operation)
        {
            this["operation"] = operation;
        }
    }
}
