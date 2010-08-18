using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;

namespace AmxMobile.Phone7.SixBookmarks
{
    public abstract class RestServiceProxy : ServiceProxy
    {
        protected RestServiceProxy(string serviceName)
            : base(serviceName)
        {
        }

        internal void SendRequest(RestRequestArgs args, Action<XElement> success, Failed failed)
        {
            if (args == null)
                throw new ArgumentNullException("args");
            if (success == null)
                throw new ArgumentNullException("success");
            if (failed == null)
                throw new ArgumentNullException("failed");

            // create a request state...
            RequestState state = new RequestState()
            {
                Owner = this,
                Args = args,
                Success = success,
                Failed = failed
            };

            // are we authenticated?  if we're not, we need to call that first...
            if (!(IsAuthenticated))
            {
                // call the authenticate routine, and ask it to call the state we just setup
                // if authentication works...
                ApiService.Authenticate(new Action(state.DoRequest), failed);
            }
            else
            {
                // call the method directly...
                state.DoRequest();
            }
        }

        private class RequestState
        {
            internal RestServiceProxy Owner { get; set; }
            internal RestRequestArgs Args { get; set; }
            internal Action<XElement> Success { get; set; }
            internal Failed Failed { get; set; }

            internal void DoRequest()
            {
                // get a url...
                string url = this.Owner.ResolvedServiceUrl;
                url = HttpHelper.BuildUrl(url, this.Args);

                // call download.  this is an async method, so we need to block...
                XDocument doc = null;
                HttpHelper.Download(url, this.Owner.GetDownloadSettings(), delegate(string result)
                {
                    // we'll have some content - initialie XDocument and parse it...
                    using (TextReader reader = new StringReader(result))
                    {
                        doc = XDocument.Load(reader);
                        if (doc == null)
                            throw new InvalidOperationException("'doc' is null.");
                    }

                    // look for the response element...
                    var responseElements = new List<XElement>(doc.Descendants("AmxResponse"));
                    if (responseElements.Count == 0)
                        throw new InvalidOperationException("An AmxResponse element was not returned.");

                    // select out "HasException"...
                    XElement responseElement = responseElements[0];
                    bool hasException = XmlHelper.GetElementBoolean(responseElement, "HasException", true);
                    if (!(hasException))
                        this.Success(responseElement);
                    else
                    {
                        // get the error...
                        string message = XmlHelper.GetElementString(responseElement, "Error", true);
                        throw new InvalidOperationException(string.Format("The server returned an error: {0}.", message));
                    }

                }, this.Failed);
            }
        }
    }
}
