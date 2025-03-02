using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace HaLi.WPF.Tray
{
    /// <summary>
    /// Interaction logic for Icon.xaml
    /// </summary>
    public partial class Icon : UserControl
    {
        // click event
        public event EventHandler MouseClick;
        // double click event
        public event EventHandler MouseDoubleClick;


        public Icon()
        {
            InitializeComponent();
        }

        public void SetIcon(string path) => SetIcon(new Uri(path, UriKind.Relative));
        public void SetIcon(Uri uri) => SetIcon(new BitmapImage(uri));
        public void SetIcon(BitmapImage bitmap)
        {
            ui.Icon = bitmap;
        }

        public void ClearMenu()
        {
            uiMenu.Items.Clear();
        }

        public void AddSimple(string key, Action action)
        {
            var item = new MenuItem();
            item.Header = key;
            item.Click += (s, e) => action();
            uiMenu.Items.Add(item);
        }

        public void AddMenu(string key, Action action)
        {
            var item = new MenuItem();
            item.SetResourceReference(MenuItem.HeaderProperty, key);
            item.Click += (s, e) => action();
            uiMenu.Items.Add(item);
        }

        public void AddMenu(string key, object icon, Action action)
        {
            var item = new MenuItem();
            item.SetResourceReference(MenuItem.HeaderProperty, key);
            item.Icon = icon;
            item.Click += (s, e) => action();
            uiMenu.Items.Add(item);
        }

        public void AddMenu(MenuItem item)
        {
            uiMenu.Items.Add(item);
        }

        public void AddMenuSeparator()
        {
            uiMenu.Items.Add(new Separator());
        }

        private void ui_Click(object sender, RoutedEventArgs e)
        {
            // raise event
            MouseClick?.Invoke(this, EventArgs.Empty);
        }

        private void ui_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            // raise event
            MouseDoubleClick?.Invoke(this, EventArgs.Empty);
        }
    }

}
