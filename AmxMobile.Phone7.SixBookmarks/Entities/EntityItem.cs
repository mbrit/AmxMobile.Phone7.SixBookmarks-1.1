using System;

namespace AmxMobile.Phone7.SixBookmarks
{
    public abstract class EntityItem
    {
        public string Name { get; private set; }
        public string NativeName { get; private set; }

        protected EntityItem(string name, string nativeName)
        {
            this.Name = name;
            this.NativeName = nativeName;
        }
    }
}
