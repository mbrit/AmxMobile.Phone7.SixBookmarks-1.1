using System;
using System.Collections.Generic;

namespace AmxMobile.Phone7.SixBookmarks
{
    public class EntityType : EntityItem
    {
        private static Dictionary<Type, EntityType> EntityTypes { get; set; }

        public List<EntityField> Fields { get; private set; }
        public Type InstanceType { get; private set; }

        public EntityType(Type instanceType, string nativeName)
            : base(instanceType.Name, nativeName)
        {
            // set...
            this.Fields = new List<EntityField>();
            this.InstanceType = instanceType;
        }

        static EntityType()
        {
            EntityTypes = new Dictionary<Type, EntityType>();
        }

        public EntityField AddField(string name, string nativeName, DataType type, int size)
        {
            EntityField field = new EntityField(name, nativeName, type, size, this.Fields.Count);
            this.Fields.Add(field);

            // return...
            return field;
        }

        public EntityField GetField(String name, bool throwIfNotFound)
        {
            foreach (EntityField field in Fields)
            {
                if (string.Compare(field.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0)
                    return field;
            }

            // throw...
            if (throwIfNotFound)
                throw new Exception(string.Format("Failed to find a field with name '{0}'.", name));
            else
                return null;
        }

        public EntityField GetKeyField()
        {
            foreach (EntityField field in this.Fields)
            {
                if (field.IsKey)
                    return field;
            }

            // nope...
            throw new InvalidOperationException("Failed to find a key field.");
        }

        public bool IsField(string name)
        {
            EntityField field = this.GetField(name, false);
            if (field != null)
                return true;
            else
                return false;
        }

        public static void RegisterEntityType(EntityType entityType)
        {
            EntityTypes[entityType.InstanceType] = entityType;
        }

        public static EntityType GetEntityType(Type type)
        {
            if (EntityTypes.ContainsKey(type))
                return EntityTypes[type];
            else
                throw new Exception(string.Format("Failed to get entity type for '{0}'.", type));
        }

        internal Entity CreateInstance()
        {
            return (Entity)Activator.CreateInstance(this.InstanceType);
        }

        internal List<T> CreateCollectionInstance<T>()
            where T : Entity
        {
            Type listType = typeof(List<>);
            Type genericType = listType.MakeGenericType(new Type[] { typeof(T) });

            // return...
            return (List<T>)Activator.CreateInstance(genericType);
        }
    }
}
