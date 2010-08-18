using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using System.Collections.Generic;

namespace AmxMobile.Phone7.SixBookmarks
{
    internal class DataBox
    {
        public EntityType EntityType { get; private set; }
        internal string FolderName { get; private set; }

        internal DataBox(EntityType et)
        {
            if (et == null)
                throw new ArgumentNullException("et");
            this.EntityType = et;

            // create a folder...
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            this.FolderName = "Entities/" + et.NativeName;
            if (!(store.DirectoryExists(FolderName)))
                store.CreateDirectory(FolderName);
        }

        private string GetFilePath(Entity entity)
        {
            return string.Format("{0}/{1}.xml", this.FolderName, entity.GetValue(EntityType.GetKeyField()));
        }

        internal IsolatedStorageFile GetStore()
        {
            return IsolatedStorageFile.GetUserStoreForApplication();
        }

        private int GetNextId()
        {
            IsolatedStorageFile store = this.GetStore();
            int max = 0;
            foreach (string path in store.GetFileNames(this.FolderName + "/*.*"))
            {
                string filename = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileName(path));
                int id = int.Parse(filename);
                if (id > max)
                    max = id;
            }

            // return...
            return max + 1;
        }

        internal void Insert(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            // get an ID by walking files...
            int id = GetNextId();

            // set the id into the entity...
            entity.SetValue(this.EntityType.GetKeyField(), id, SetReason.UserSet);

            // when we create an entity, all of the values have to be set as "modified" otherwise we get a 
            // problem when reading them back...
            foreach (EntityField field in entity.EntityType.Fields)
            {
                if (!(entity.IsModified(field)))
                {
                    object defaultValue = field.DefaultValue;
                    entity.SetValue(field, defaultValue, SetReason.UserSet);
                }
            }

            // ...and then update it, because that operation is basically just a save...
            Update(entity);
        }

        internal void Update(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            // get the filename...
            string path = GetFilePath(entity);
            if (path == null)
                throw new InvalidOperationException("'path' is null.");
            if (path.Length == 0)
                throw new InvalidOperationException("'path' is zero-length.");

            // save it...
            SaveEntity(entity, path);
        }

        private void SaveEntity(Entity entity, string path)
        {
            // create a document...
            XDocument doc = EntityToXml(entity);

            // save the file...
            IsolatedStorageFile store = this.GetStore();
            using (IsolatedStorageFileStream stream = store.CreateFile(path))
                doc.Save(stream);
        }

        private XDocument EntityToXml(Entity entity)
        {
            // save...
            XDocument doc = new XDocument();
            XElement root = new XElement(EntityType.NativeName);
            doc.Add(root);

            // walk the fields...
            foreach (EntityField field in EntityType.Fields)
            {
                if (entity.IsLoaded(field))
                {
                    XElement element = new XElement(field.NativeName);
                    root.Add(element);

                    // get...
                    object value = entity.GetValue(field);
                    if (value != null)
                        element.Value = value.ToString();
                }
            }

            // return...
            return doc;
        }

        internal void Delete(Entity entity)
        {
            string path = this.GetFilePath(entity);

            // delete...
            IsolatedStorageFile store = this.GetStore();
            if (store.FileExists(path))
                store.DeleteFile(path);
        }

        internal List<T> GetAll<T>()
            where T : Entity
        {
            IsolatedStorageFile store = this.GetStore();

            // load them all...
            List<T> results = this.EntityType.CreateCollectionInstance<T>();
            foreach (string filename in store.GetFileNames(this.FolderName + "/*.xml"))
            {
                // get the xml...
                XDocument doc = null;
                using (Stream stream = store.OpenFile(this.FolderName + "/" + filename, FileMode.Open, FileAccess.Read))
                    doc = XDocument.Load(stream);

                // turn it into an entity...
                T entity = (T)EntityFromXml(doc);
                results.Add(entity);
            }

            // return...
            return results;
        }

        private Entity EntityFromXml(XDocument doc)
        {
            Entity entity = this.EntityType.CreateInstance();

            // walk...
            List<XElement> elements = new List<XElement>(doc.Descendants(this.EntityType.NativeName));
            if (elements.Count == 0)
                throw new InvalidOperationException(string.Format("An element with name '{0}' was not found.", this.EntityType.NativeName));

            // walk the fields...
            XElement root = elements[0];
            foreach (EntityField field in this.EntityType.Fields)
            {
                // find an element...
                XElement element = root.Element(field.NativeName);
                if (element != null)
                {
                    if (field.Type == DataType.String)
                        entity.SetValue(field, XmlHelper.GetStringValue(element), SetReason.Load);
                    else if (field.Type == DataType.Int32)
                        entity.SetValue(field, XmlHelper.GetInt32Value(element), SetReason.Load);
                    else if (field.Type == DataType.Boolean)
                        entity.SetValue(field, XmlHelper.GetBooleanValue(element), SetReason.Load);
                    else
                        throw new NotSupportedException(string.Format("Cannot handle '{0}'.", field.Type));
                }
            }

            // return...
            return entity;
        }
    }
}
