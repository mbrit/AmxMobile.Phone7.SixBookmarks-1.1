using System;

namespace AmxMobile.Phone7.SixBookmarks
{
    public class EntityField : EntityItem
    {
        public DataType Type { get; private set; }
        public int Size { get; private set; }
        public int Ordinal { get; private set; }
        public bool IsKey { get; set; }

        internal EntityField(string name, string nativeName, DataType type, int size, int ordinal)
            : base(name, nativeName)
        {
            this.Type = type;
            this.Size = size;
            this.Ordinal = ordinal;
        }

        public object DefaultValue
        {
            get
            {
                if (this.Type == DataType.Int32)
                    return 0;
                else if (this.Type == DataType.String)
                    return null;
                else if (this.Type == DataType.Boolean)
                    return false;
                else
                    throw new NotSupportedException(string.Format("Cannot handle '{0}'.", this.Type));
            }
        }
    }
}
