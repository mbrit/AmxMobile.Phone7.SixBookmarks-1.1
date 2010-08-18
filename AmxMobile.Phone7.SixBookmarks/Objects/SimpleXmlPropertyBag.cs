using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;

namespace AmxMobile.Phone7.SixBookmarks
{
    public class SimpleXmlPropertyBag : Dictionary<string, string>
    {
        private string Path { get; set; }

        internal SimpleXmlPropertyBag(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (path.Length == 0)
                throw new ArgumentException("'path' is zero-length.");

            // set...
            this.Path = path;
        }

        public void Save()
        {
            // get...
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            if (store == null)
                throw new InvalidOperationException("'store' is null.");

            // get a document...
            XDocument doc = this.ToXmlDocument();
            if (doc == null)
                throw new InvalidOperationException("'doc' is null.");

            // open a stream and recreate the file...
            if (store.FileExists(this.Path))
                store.DeleteFile(this.Path);
            using (Stream stream = store.OpenFile(this.Path, FileMode.Create, FileAccess.Write))
                doc.Save(stream);
        }

        private XDocument ToXmlDocument()
        {
            XDocument doc = new XDocument();

            // root...
            XElement root = new XElement("Root");
            doc.Add(root);

            // items...
            foreach (string key in this.Keys)
            {   
                // create a child element...
                XElement element = new XElement(key);
                root.Add(element);

                // set...
                element.SetValue(this[key]);
            }

            // return...
            return doc;
        }

        internal static SimpleXmlPropertyBag Load(string path, bool throwIfNotFound)
        {
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

            // does the file exist?
            if (store.FileExists(path))
            {
                // load it...
                XDocument doc = null;
                using(Stream stream = store.OpenFile(path, FileMode.Open, FileAccess.Read))
                    doc = XDocument.Load(stream);

                // find the root...
                XElement root = doc.Element("Root");
                if(root == null)
	                throw new InvalidOperationException("'root' is null.");

                // load...
                SimpleXmlPropertyBag bag = new SimpleXmlPropertyBag(path);
                foreach(XElement element in root.Elements())
                    bag[element.Name.LocalName] = element.Value;

                // return...
                return bag;
            }
            else
            {
                if(throwIfNotFound)
                    throw new InvalidOperationException(string.Format("A file at '{0}' was not found.", path));
                else
                {
                    // return a new one...
                    return new SimpleXmlPropertyBag(path);
                }
            }
        }
    }
}
