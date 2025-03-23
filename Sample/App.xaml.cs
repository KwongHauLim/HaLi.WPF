using HaLi.WPF.Tray;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static App _ptr;
        internal Window? Showing { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _ptr = this;

            Window win = new Window();
            var Tray = new Icon();
            Tray.SetIcon(GetUri("icon.png"));

            var a = Tray.AddMenu("A", () => MessageBox.Show("A"));
            var b = Tray.AddMenu("B", () => MessageBox.Show("B"), a);

            win = NoWindow.Create(Tray);

            Showing = new MainWindow();
            Showing.Closed += (_, _) => Shutdown();
            Showing.Show();

            Uri GetUri(string key) => new Uri($"pack://application:,,,/{key}", UriKind.Absolute);
            Image GetIcon(string key) => new Image { Source = new BitmapImage(GetUri(key)), Width = 16, Height = 16 };
        }
    }

}
