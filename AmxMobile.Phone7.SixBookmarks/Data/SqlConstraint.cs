using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AmxMobile.Phone7.SixBookmarks
{
    public class SqlConstraint
    {
        internal EntityField Field { get; private set; }
        internal object Value { get; private set; }

        internal SqlConstraint(EntityField field, object value)
        {
            this.Field = field;
            this.Value = value;
        }
    }
}
