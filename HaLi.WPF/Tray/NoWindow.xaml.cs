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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HaLi.WPF.Tray
{
    /// <summary>
    /// Interaction logic for NoWindow.xaml
    /// </summary>
    public partial class NoWindow : Window
    {
        public NoWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
            Visibility = Visibility.Hidden;
        }

        public static NoWindow Create(Icon tray)
        {
            var window = new NoWindow();
            window.Content = tray;
            window.Show();
            return window;
        }
    }
}
