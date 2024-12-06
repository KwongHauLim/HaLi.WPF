using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HaLi.WPF.Board
{
    /// <summary>
    /// Interaction logic for BoardCanvas.xaml
    /// </summary>
    public partial class BoardCanvas : UserControl
    {
        public bool Editable
        {
            get { return (bool)GetValue(EditableProperty); }
            set { SetValue(EditableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Editable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditableProperty =
            DependencyProperty.Register("Editable", typeof(bool), typeof(BoardCanvas), new PropertyMetadata(true));

        public bool IsEditing { get; set; }
        public EditBase? Editor { get; set; }

        public BoardCanvas()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            IsHitTestVisible = true;
            Background ??= Brushes.Transparent;

            Loaded += OnLoaded;

            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            MouseLeftButtonDown += OnMouseDown;
            MouseLeftButtonUp += OnMouseUp;
            MouseMove += OnMouseMove;
            MouseDoubleClick += OnMouseDoubleClick;
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Editor = new LineEdit();
            var mon = new EditBase.EditMouse.EditMonitor { WhenRelease = true };
            mon.On += (s, e) =>
            {

            };
            Editor.Mouse.Monitors.Add(mon);
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            Editor?.Mouse.OnMouseEnter(sender, e);
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Editor?.Mouse.OnMouseLeave(sender, e);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Editor?.Mouse.OnMouseDown(sender, e);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Editor?.Mouse.OnMouseUp(sender, e);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Editor?.Mouse.OnMouseMove(sender, e);
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Editor?.Mouse.OnMouseDown(sender, e);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Editor?.Keyboard.OnKeyDown(sender, e);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Editor?.Keyboard.OnKeyUp(sender, e);
        }
    }
}
