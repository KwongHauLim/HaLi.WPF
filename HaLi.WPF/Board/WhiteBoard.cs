using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HaLi.WPF.Board;

public class WhiteBoard : ItemsControl
{
    public Brush Background
    {
        get { return (Brush)GetValue(BackgroundProperty); }
        set { SetValue(BackgroundProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty BackgroundProperty =
        DependencyProperty.Register("Background", typeof(Brush), typeof(WhiteBoard), new PropertyMetadata(Brushes.White));


}
