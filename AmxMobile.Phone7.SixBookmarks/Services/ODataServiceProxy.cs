using System;
using System.Data.Services.Client;

namespace AmxMobile.Phone7.SixBookmarks
{
    internal abstract class ODataServiceProxy : ServiceProxy
    {
        protected ODataServiceProxy(string serviceName)
            : base(serviceName)
        {
        }

        internal void ConfigureODataProxy(DataServiceContext proxy)
        {
            proxy.SendingRequest += new EventHandler<SendingRequestEventArgs>(proxy_SendingRequest);
        }

        void proxy_SendingRequest(object sender, SendingRequestEventArgs e)
        {
            // get the settings...
            DownloadSettings settings = GetDownloadSettings();
            if (settings == null)
                throw new InvalidOperationException("'settings' is null.");

            // walk...
            foreach (string name in settings.ExtraHeaders.Keys)
                e.RequestHeaders[name] = settings.ExtraHeaders[name];
        }
    }
}
