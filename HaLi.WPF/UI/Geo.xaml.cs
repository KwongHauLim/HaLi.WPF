using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HaLi.WPF.UI
{
    /// <summary>
    /// Interaction logic for Geo.xaml
    /// </summary>
    public partial class Geo : UserControl
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Brush ForegroundOnHover
        {
            get { return (Brush)GetValue(ForegroundOnHoverProperty); }
            set { SetValue(ForegroundOnHoverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ForegroundOnHover.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundOnHoverProperty =
            DependencyProperty.Register("ForegroundOnHover", typeof(Brush), typeof(Geo), new PropertyMetadata(null, OnChanged));

        public Brush BackgroundOnHover
        {
            get { return (Brush)GetValue(BackgroundOnHoverProperty); }
            set { SetValue(BackgroundOnHoverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundOnHover.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundOnHoverProperty =
            DependencyProperty.Register("BackgroundOnHover", typeof(Brush), typeof(Geo), new PropertyMetadata(null, OnChanged));

        public Geometry Geometry
        {
            get { return (Geometry)GetValue(GeometryProperty); }
            set
            {
                SetValue(GeometryProperty, value);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Geometry"));
                }
            }
        }

        // Using a DependencyProperty as the backing store for Geometry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GeometryProperty =
            DependencyProperty.Register("Geometry", typeof(Geometry), typeof(Geo), new PropertyMetadata(default, OnChanged));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(Geo), new PropertyMetadata(default));

        public object CommandParameters
        {
            get { return (object)GetValue(CommandParametersProperty); }
            set { SetValue(CommandParametersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameters.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParametersProperty =
            DependencyProperty.Register("CommandParameters", typeof(object), typeof(Geo), new PropertyMetadata(null));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Geo geo)
            {
                geo.uiDraw.Geometry = geo.Geometry;
                geo.MouseHover(geo.IsHover);
            }
        }

        public event EventHandler OnClick;
        public event EventHandler OnEnter;
        public event EventHandler OnLeave;

        public bool IsHover { get; private set; }

        public Geo()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Command != null && Command.CanExecute(CommandParameters))
                Command.Execute(CommandParameters);
            if (OnClick != null)
                OnClick(this, EventArgs.Empty);
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
            => MouseHover(true);

        private void OnMouseLeave(object sender, MouseEventArgs e)
            => MouseHover(false);

        private void MouseHover(bool hover)
        {
            IsHover = hover;

            uiDraw.Brush = Foreground;
            uiBack.Background = Background;

            if (hover)
            {
                if (ForegroundOnHover != null)
                    uiDraw.Brush = ForegroundOnHover;
                if (BackgroundOnHover != null)
                    uiBack.Background = BackgroundOnHover;

                OnEnter?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnLeave?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
