using System;

namespace AmxMobile.Phone7.SixBookmarks
{
    internal class DataBoxEntityChangeProcessor : EntityChangeProcessor
    {
        private DataBox DataBox { get; set; }

        internal DataBoxEntityChangeProcessor(EntityType et)
            : base(et)
        {
            this.DataBox = new DataBox(et);
        }

        internal override void Insert(Entity entity)
        {
            // defer...
            this.DataBox.Insert(entity);
        }

        internal override void Update(Entity entity)
        {
            // defer...
            this.DataBox.Update(entity);
        }

        internal override void Delete(Entity entity)
        {
            // defer...
            this.DataBox.Delete(entity);
        }
    }
}
