using System;
using System.IO;
using System.Net;
using System.Text;

namespace AmxMobile.Phone7.SixBookmarks
{
    public static class HttpHelper
    {
        public static void Download(string url, DownloadSettings settings, Action<string> success, Failed failure)
        {
            // create the request stub...
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // add...
            if (settings != null)
            {
                foreach (string name in settings.ExtraHeaders.Keys)
                    request.Headers[name] = settings.ExtraHeaders[name];
            }

            // call the server... we'll get notified at some point...
            request.BeginGetResponse(new AsyncCallback(HandleDownloadResult), new object[] { request, success, failure });
        }

        private static void HandleDownloadResult(IAsyncResult result)
        {
            // unpackage the state...
            object[] state = (object[])result.AsyncState;
            HttpWebRequest request = (HttpWebRequest)state[0];
            Action<string> success = (Action<string>)state[1];
            Failed failed = (Failed)state[2];

            // unwind...
            try
            {

                // get the response...
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                using (Stream stream = response.GetResponseStream())
                {
                    // read the html out...
                    StreamReader reader = new StreamReader(stream);
                    string html = reader.ReadToEnd();

                    // callback the html...
                    success(html);
                }
            }
            catch (WebException ex)
            {
                HandleDownloadException(request.RequestUri.ToString(), ex, failed);
            }
            catch (Exception ex)
            {
                // callback the error...
                failed(ex);
            }
        }

        private static void HandleDownloadException(string url, WebException ex, Failed failed)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Failed to download '{0}'.", url);
            builder.Append("\r\n");

            // do we have a response...
            if (ex.Response != null)
            {
                try
                {
                    Stream stream = ex.Response.GetResponseStream();
                    if (stream == null)
                        throw new InvalidOperationException("'stream' is null.");
                    using (stream)
                    {
                        builder.Append("Response data: ");

                        // reader...
                        using (StreamReader reader = new StreamReader(stream))
                            builder.Append(reader.ReadToEnd());
                    }
                }
                catch (Exception readEx)
                {
                    builder.AppendFormat("An exception occurred when reading error data: " + readEx.Message);
                }
            }
            else
                builder.Append("(No response)");

            // defer to the context...
            failed(new InvalidOperationException(builder.ToString(), ex));
        }

        internal static string BuildUrl(string url, RestRequestArgs args)
        {
            StringBuilder builder = new StringBuilder();

            // remove the old query...
            int index = url.IndexOf("?");
            if(index != -1)
                builder.Append(url.Substring(0, index));
            else
                builder.Append(url);

            // add the arguments...
            if (args.Count > 0)
            {
                builder.Append("?");

                // params...
                bool first = true;
                foreach (string key in args.Keys)
                {
                    if (first)
                        first = false;
                    else
                        builder.Append("&");
                    builder.Append(key);
                    builder.Append("=");
                    builder.Append(HttpUtility.UrlEncode(args[key]));
                }
            }

            // return...
            return builder.ToString();
        }
    }
}
