using System;

namespace AmxMobile.Phone7.SixBookmarks
{
    public abstract class ServiceProxy
    {
        // YOU MUST CHANGE THESE VALUES IN ORDER TO USE THIS SAMPLE...
        internal const string ApiUsername = "amxmobile";
        internal const string ApiPassword = "password";

        private const string RootUrl = "http://services.multimobiledevelopment.com/services/";

        internal string ServiceName { get; private set; }
        internal static string Token { get; set; }

        protected ServiceProxy(string serviceName)
        {
            if (serviceName == null)
                throw new ArgumentNullException("serviceName");
            if (serviceName.Length == 0)
                throw new ArgumentException("'serviceName' is zero-length.");

            // set...
            this.ServiceName = serviceName;
        }


        internal virtual bool IsAuthenticated
        {
            get
            {
                // we are not authenticated if we do not have a token...
                return !(string.IsNullOrEmpty(Token));
            }
        }

        public string ResolvedServiceUrl
        {
            get
            {
                return RootUrl + this.ServiceName;
            }
        }

        protected DownloadSettings GetDownloadSettings()
        {
            DownloadSettings settings = new DownloadSettings();
            settings.ExtraHeaders["x-amx-apiusername"] = ApiUsername;
            if (!(string.IsNullOrEmpty(Token)))
                settings.ExtraHeaders["x-amx-token"] = Token;

            // return...
            return settings;
        }
    }
}
