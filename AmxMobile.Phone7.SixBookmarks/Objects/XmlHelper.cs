using System;
using System.Xml.Linq;

namespace AmxMobile.Phone7.SixBookmarks
{
    public static class XmlHelper
    {
        public static string GetElementString(XElement element, string name, bool throwIfNotFound)
        {
            return (string)GetElementValue(element, name, XmlDataType.String, throwIfNotFound);
        }

        public static bool GetElementBoolean(XElement element, String name, bool throwIfNotFound)
	    {
		    return (bool)GetElementValue(element, name, XmlDataType.Boolean, throwIfNotFound);
	    }
	
	    public static int GetElementInt32(XElement element, String name, bool throwIfNotFound)
	    {
            return (int)GetElementValue(element, name, XmlDataType.Int32, throwIfNotFound);
	    }
	
	    private static Object GetElementValue(XElement element, String name, XmlDataType dt, bool throwIfNotFound)
	    {
		    // find it...
            XElement child = element.Element(name);
		    if(child != null)
		    {
			    if(dt == XmlDataType.String)
				    return GetStringValue(child);
			    else if(dt == XmlDataType.Boolean)
                    return (bool)GetBooleanValue(child);
			    else if(dt == XmlDataType.Int32)
                    return (int)GetInt32Value(child);
			    else
				    throw new Exception(string.Format("Cannot handle '{0}'.", dt));
		    }
		    else
		    {
			    if(throwIfNotFound)
				    throw new Exception(string.Format("An element with name '{0}' was not found within an element with name '{1}'.", name, element.Name));
			    else
				    return null;
		    }
	    }

        public static String GetStringValue(XNode item)
	    {
            if(item == null)
	            throw new ArgumentNullException("item");

            if (item is XElement)
                return ((XElement)item).Value;
            else
                throw new Exception(string.Format("Cannot handle '{0}'.", item.GetType()));
	    }

	    public static int GetInt32Value(XNode item)
	    {
		    string asString = GetStringValue(item);
		    return int.Parse(asString);
	    }
	
	    public static bool GetBooleanValue(XNode item)
	    {
		    string asString = GetStringValue(item);
            if (asString == null)
                throw new InvalidOperationException("'asString' is null.");
            if (asString.Length == 0)
                throw new InvalidOperationException("'asString' is zero-length.");

            // check...
		    if(asString == "0" || string.Compare(asString, "false", StringComparison.InvariantCultureIgnoreCase) == 0)
			    return false;
            else if (asString == "1" || string.Compare(asString, "true", StringComparison.InvariantCultureIgnoreCase) == 0)
			    return true;
		    else
			    throw new Exception(string.Format("The value '{0}' could not be recognised as valid Boolean value.", asString));
	    }
    }
}
