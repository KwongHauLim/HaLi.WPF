using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HaLi.WPF.UI
{
    /// <summary>
    /// Interaction logic for Tack.xaml
    /// </summary>
    public partial class Tack : UserControl
    {
        public class Data
        {
            public double Width { get; set; }
            public double Height { get; set; }
            public Brush Background { get; set; }
            public double BorderThickness { get; set; }
            public Brush BorderBrush { get; set; }
            public double IconWidth { get; set; }
            public double IconHeight { get; set; }
        }



        public Data Normal
        {
            get { return (Data)GetValue(NormalProperty); }
            set { SetValue(NormalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Normal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NormalProperty =
            DependencyProperty.Register("Normal", typeof(Data), typeof(Tack), new PropertyMetadata(new Data {
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                BorderThickness = 0.35,
                Width = 20,
                Height = 20,
                IconWidth = 13,
                IconHeight = 13
            }));


        public Data Hover
        {
            get { return (Data)GetValue(HoverProperty); }
            set { SetValue(HoverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Hover.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverProperty =
            DependencyProperty.Register("Hover", typeof(Data), typeof(Tack), new PropertyMetadata(new Data
            {
                Background = new SolidColorBrush(Color.FromArgb(255,232,232,232)), 
                BorderBrush = Brushes.Black,
                BorderThickness = 0.35,
                Width = 18,
                Height = 18,
                IconWidth = 13,
                IconHeight = 13
            }));

        public bool Pin { get; private set; }
        public bool Flip { get; set; }

        public Tack()
        {
            InitializeComponent();

            transPinScale.ScaleX = Flip ? -1 : 1;
        }

        private void OnEnter(object sender, MouseEventArgs e)
        {
            uiBorder.Width = Hover.Width;
            uiBorder.Height = Hover.Height;
            uiBorder.Background = Hover.Background;
            uiBorder.BorderBrush = Hover.BorderBrush;
            uiBorder.BorderThickness = new Thickness(Hover.BorderThickness);
            uiBorder.CornerRadius = new CornerRadius(Math.Max(Hover.Width, Hover.Height) / 2);
            uiIcon.Width = Hover.IconWidth;
            uiIcon.Height = Hover.IconHeight;
        }

        private void OnLeave(object sender, MouseEventArgs e)
        {
            uiBorder.Width = Normal.Width;
            uiBorder.Height = Normal.Height;
            uiBorder.Background = Normal.Background;
            uiBorder.BorderBrush = Normal.BorderBrush;
            uiBorder.BorderThickness = new Thickness(Normal.BorderThickness);
            uiBorder.CornerRadius = new CornerRadius(Math.Max(Normal.Width, Normal.Height) / 2);
            uiIcon.Width = Normal.IconWidth;
            uiIcon.Height = Normal.IconHeight;
        }

        private void OnClick_Pin(object sender, EventArgs e)
        {
            Pin = !Pin;

            double angle = Flip ? 45 : -45;
            double a = Pin ? 0 : angle;
            double b = Pin ? angle : 0;

            var dur = new Duration(TimeSpan.FromSeconds(0.2));
            var animation = new DoubleAnimation(a, b, dur);
            transPinRoate.BeginAnimation(RotateTransform.AngleProperty, animation);
        }
    }
}
