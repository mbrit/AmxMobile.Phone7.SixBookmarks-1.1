using System;
using System.Xml.Linq;

namespace AmxMobile.Phone7.SixBookmarks
{
    public class UsersService : RestServiceProxy
    {
        public UsersService()
            : base("usersrest.aspx")
        {
        }

        public void Logon(String username, String password, Action<LogonResponse> callback, Failed failed)
        {
            // create the request...
            RestRequestArgs args = new RestRequestArgs("logon");

            // add the username and password...
            args["username"] = username;
            args["password"] = password;

            // send the request...
            SendRequest(args, delegate(XElement element)
            {
                // create a result from that...
                LogonResponse response = LogonResponse.FromXmlElement(element);
                if (response == null)
                    throw new InvalidOperationException("'response' is null.");

                // callback...
                callback(response);

            }, failed);
        }
    }
}
