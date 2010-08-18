using System;
using System.Xml.Linq;

namespace AmxMobile.Phone7.SixBookmarks
{
    public class LogonResponse
    {
        public LogonResult Result { get; private set; }
        public string Message { get; private set; }
        public string Token { get; private set; }

        internal LogonResponse(LogonResult result, string message, string token)
        {
            this.Result = result;
            this.Message = message;
            this.Token = token;
        }

        internal static LogonResponse FromXmlElement(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            // get the result...
            string asString = XmlHelper.GetElementString(element, "Result", true);
            LogonResult result = (LogonResult)Enum.Parse(typeof(LogonResult), asString, true);
            return new LogonResponse(result, XmlHelper.GetElementString(element, "Message", false),
                XmlHelper.GetElementString(element, "Token", false));
        }
    }
}
