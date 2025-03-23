using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        public Dictionary<string, MenuItem> MenuMap { get; private set; } = new();

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

        private void AddTo(MenuItem? parent, MenuItem item)
        {
            if (parent == null)
            {
                uiMenu.Items.Add(item);
            }
            else
            {
                parent.Items.Add(item);
            }

            string key = GetKey(item);
            MenuMap[key] = item;
        }

        private string GetKey(MenuItem item)
        {
            var key = string.Empty;
            if (item.Header is string head)
                key = head;

            var parent = GetParent(item);
            if (parent != null)
            {
                key = GetKey(parent) + "." + key;
            }

            return key;
        }

        private MenuItem GetParent(MenuItem menuItem)
            => (menuItem.Parent is MenuItem parent) ? parent : null;

        public MenuItem AddSimple(string key, Action action, MenuItem parent = null)
        {
            var item = new MenuItem();
            item.Header = key;
            item.Click += (s, e) => action();
            AddTo(parent, item);
            return item;
        }

        public MenuItem AddMenu(string key, Action action, MenuItem parent = null)
        {
            var item = new MenuItem();
            item.SetValue(MenuItem.HeaderProperty, key); // set header (key)
            item.Click += (s, e) => action();
            AddTo(parent, item);
            return item;
        }

        public MenuItem AddMenu(string key, object icon, Action action, MenuItem parent = null)
        {
            var item = new MenuItem();
            item.SetValue(MenuItem.HeaderProperty, key); // set header (key)
            item.Icon = icon;
            item.Click += (s, e) => action();
            AddTo(parent, item);
            return item;
        }

        public MenuItem AddMenu(string key, ICommand cmd, MenuItem parent = null)
        {
            var item = new MenuItem();
            item.SetValue(MenuItem.HeaderProperty, key); // set header (key)
            item.Click += (s, e) => cmd.Execute(s);
            AddTo(parent, item);
            return item;
        }

        public MenuItem AddMenu(string key, object icon, ICommand cmd, MenuItem parent = null)
        {
            var item = new MenuItem();
            item.SetValue(MenuItem.HeaderProperty, key); // set header (key)
            item.Icon = icon;
            item.Click += (s, e) => cmd.Execute(s);
            AddTo(parent, item);
            return item;
        }

        public void AddMenu(MenuItem item, MenuItem parent = null)
        {
            AddTo(parent, item);
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
