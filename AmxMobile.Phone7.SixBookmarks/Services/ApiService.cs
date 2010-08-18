using System;
using System.Xml.Linq;

namespace AmxMobile.Phone7.SixBookmarks
{
    public class ApiService : RestServiceProxy
    {
        public ApiService()
            : base("apirest.aspx")
        {
        }

        public void Logon(string password, Action<LogonResponse> result, Failed failed)
        {
            // create the request...
            RestRequestArgs args = new RestRequestArgs("logon");
            args["password"] = password;

            // send the request...
            this.SendRequest(args, (Action<XElement>)delegate(XElement element)
            {
                // walk...
                LogonResponse response = LogonResponse.FromXmlElement(element);
                if (response == null)
                    throw new InvalidOperationException("'response' is null.");

                // call...
                result(response);

            }, failed);
        }

        internal override bool IsAuthenticated
        {
            get
            {
                // make this return true, otherwise we'll get a
                // stack overflow...
                return true;
            }
        }

        internal static void Authenticate(Action callback, Failed failed)
        {
            ApiService service = new ApiService();
            service.Logon(ApiPassword, (Action<LogonResponse>)delegate(LogonResponse result)
            {
                // we have a result from *our* call to ApiService, if that works, call back to the next
                // item in the chain...
                if (result.Result == LogonResult.LogonOk)
                {
                    // store the token in static memory...
                    Token = result.Token;

                    // call the caller...
                    callback();
                }
                else
                    throw new InvalidOperationException(string.Format("A response of '{0}' was returned when authenticating the API.  Check that the the values of ApiUsername and ApiPassword correspond to your unique API account.", result.Result));

            }, failed);
        }
    }
}
