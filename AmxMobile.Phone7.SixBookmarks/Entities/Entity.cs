using System;

namespace AmxMobile.Phone7.SixBookmarks
{
    public abstract class Entity
    {
        public EntityType EntityType { get; private set; }
        private object[] _values { get; set; }
        private FieldFlags[] _flags { get; set; }
        public bool IsDeleted { get; private set; }

        protected Entity()
        {
            // get the entity type...
            this.EntityType = EntityType.GetEntityType(this.GetType());
            if (this.EntityType == null)
                throw new InvalidOperationException("'this.EntityType' is null.");

            // create the slots...
            _values = new object[this.EntityType.Fields.Count];
            _flags = new FieldFlags[this.EntityType.Fields.Count];
        }

        protected void SetValue(String name, Object value, SetReason reason)
        {
            EntityField field = EntityType.GetField(name, true);
            SetValue(field, value, reason);
        }

        public void SetValue(EntityField field, Object value, SetReason reason)
        {
            int ordinal = field.Ordinal;
            SetValue(ordinal, value, reason);
        }

        private void SetValue(int ordinal, Object value, SetReason reason)
        {
            _values[ordinal] = value;

            // if...
            SetFlag(ordinal, FieldFlags.Loaded);
            if (reason == SetReason.UserSet)
                SetFlag(ordinal, FieldFlags.Modified);
        }

        private void SetFlag(int ordinal, FieldFlags flag)
        {
            _flags[ordinal] = _flags[ordinal] | flag;
        }

        public bool IsNew
        {
            get
            {
                EntityField key = EntityType.GetKeyField();

                // state...
                if (!(IsLoaded(key)) && !(IsLoaded(key)))
                    return true;
                else
                    return false;
            }
        }

        public bool IsModified()
        {
            for (int index = 0; index < _flags.Length; index++)
            {
                if (IsModified(index))
                    return true;
            }

            // nope...
            return false;
        }

        public bool IsLoaded(EntityField field)
        {
            return IsLoaded(field.Ordinal);
        }

        private bool IsLoaded(int index)
        {
            return IsFlagSet(index, FieldFlags.Loaded);
        }

        public bool IsModified(EntityField field)
        {
            return IsModified(field.Ordinal);
        }

        private bool IsModified(int index)
        {
            return IsFlagSet(index, FieldFlags.Modified);
        }

        private bool IsFlagSet(int index, FieldFlags flag)
        {
            if ((_flags[index] & flag) == flag)
                return true;
            else
                return false;
        }

        public Object GetValue(String name)
        {
            EntityField field = EntityType.GetField(name, true);
            return GetValue(field.Ordinal);
        }

        public Object GetValue(EntityField field)
        {
            return GetValue(field.Ordinal);
        }

        private Object GetValue(int index)
        {
            // do we need to demand load?
            if (!(IsLoaded(index)) && !(this.IsNew))
                throw new Exception("Demand loading is not implemented.");

            // return...
            return _values[index];
        }

        public String GetStringValue(String name)
        {
            EntityField field = EntityType.GetField(name, true);
            return GetStringValue(field);
        }

        public String GetStringValue(EntityField field)
        {
            Object value = GetValue(field);
            if (value != null)
                return value.ToString();
            else
                return null;
        }

        public int GetInt32Value(String name)
        {
            EntityField field = EntityType.GetField(name, true);
            return GetInt32Value(field);
        }

        public int GetInt32Value(EntityField field)
        {
            Object value = GetValue(field);
            if (value == null)
                return 0;
            else if (value is Int32)
                return (int)value;
            else
                throw new Exception(string.Format("Cannot handle '%s'.", value.GetType()));
        }

        public bool GetBooleanValue(String name)
        {
            EntityField field = EntityType.GetField(name, true);
            return GetBooleanValue(field);
        }

        public bool GetBooleanValue(EntityField field)
        {
            Object value = GetValue(field);
            if (value == null)
                return false;
            else if (value is bool)
                return (bool)value;
            else if (value is int)
            {
                int asInt = (int)value;
                if (asInt == 0)
                    return false;
                else
                    return true;
            }
            else
                throw new Exception(string.Format("Cannot handle '{0}'.", value.GetType()));
        }

        public void SaveChanges()
        {
            DataBoxEntityChangeProcessor processor = new DataBoxEntityChangeProcessor(this.EntityType);
            processor.SaveChanges(this);
        }

        public void MarkForDeletion()
        {
            this.IsDeleted = true;
        }
    }
}
