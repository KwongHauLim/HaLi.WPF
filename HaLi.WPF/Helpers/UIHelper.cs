using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace HaLi.WPF.Helpers
{
    public static class GuiHelper
    {
        public static Binding Bind(object source, PropertyPath path, BindingMode way) => new Binding
        {
            Source = source,
            Path = path,
            Mode = way,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };

        public static Binding OneWay(object source, string path) => Bind(source, new PropertyPath(path), BindingMode.OneWay);

        public static Binding TwoWay(object source, string path) => Bind(source, new PropertyPath(path), BindingMode.TwoWay);
    }
}
