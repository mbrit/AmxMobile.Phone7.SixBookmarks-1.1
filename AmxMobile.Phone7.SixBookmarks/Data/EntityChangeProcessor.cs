using System;

namespace AmxMobile.Phone7.SixBookmarks
{
    internal abstract class EntityChangeProcessor
    {
        public EntityType EntityType { get; private set; }

        protected EntityChangeProcessor(EntityType et)
        {
            if (et == null)
                throw new ArgumentNullException("et");
            this.EntityType = et;
        }

        internal void SaveChanges(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            // new?
            if (entity.IsNew)
                Insert(entity);
            else if (entity.IsModified())
                Update(entity);
            else if (entity.IsDeleted)
                Delete(entity);
        }

        internal abstract void Delete(Entity entity);

        internal abstract void Update(Entity entity);

        internal abstract void Insert(Entity entity);
    }
}
