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
using System.Collections.Generic;

namespace AmxMobile.Phone7.SixBookmarks
{
    public abstract class EntityCollection<T> : List<T>
        where T : Entity
    {

    }
}
