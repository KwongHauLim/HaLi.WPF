using HandyControl.Data;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HaLi.WPF.GUI
{
    /// <summary>
    /// Interaction logic for FileBox.xaml
    /// </summary>
    public partial class FileBox : LabeledUI
    {
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Path.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(FileBox), new PropertyMetadata(string.Empty));

        public bool ShowFullPath
        {
            get { return (bool)GetValue(ShowFullPathProperty); }
            set { SetValue(ShowFullPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowFullPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowFullPathProperty =
            DependencyProperty.Register("ShowFullPath", typeof(bool), typeof(FileBox), new PropertyMetadata(true));

        public string Filters
        {
            get { return (string)GetValue(FiltersProperty); }
            set { SetValue(FiltersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Filters.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FiltersProperty =
            DependencyProperty.Register("Filters", typeof(string), typeof(FileBox), new PropertyMetadata("All Files|*.*"));

        public bool DirectoryMode
        {
            get { return (bool)GetValue(DirectoryModeProperty); }
            set { SetValue(DirectoryModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DirectoryMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectoryModeProperty =
            DependencyProperty.Register("DirectoryMode", typeof(bool), typeof(FileBox), new PropertyMetadata(false));

        public bool CheckExists
        {
            get { return (bool)GetValue(CheckExistsProperty); }
            set { SetValue(CheckExistsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckExists.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckExistsProperty =
            DependencyProperty.Register("CheckExists", typeof(bool), typeof(FileBox), new PropertyMetadata(true));

        public event EventHandler<FunctionEventArgs<string>> OnChanged;

        public FileBox()
        {
            InitializeComponent();
        }

        private void OnClick_Open(object sender, EventArgs e)
        {
            string path = string.Empty;

            var pop = new CommonOpenFileDialog()
            {
                Title = Label,
                InitialDirectory = Path,
                IsFolderPicker = DirectoryMode
            };

            if (pop.ShowDialog() == CommonFileDialogResult.Ok)
            {
                path = pop.FileName;
            }

            if (!string.IsNullOrEmpty(path))
            {
                Path = path;

                UpdateGUI();
                uiText.ClearValue(TextBox.ForegroundProperty);

                OnChanged?.Invoke(this, new FunctionEventArgs<string>(Path));
            }
        }

        private void OnInput(object sender, TextChangedEventArgs e)
        {
            Path = uiText.Text;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if ((DirectoryMode && Directory.Exists(Path)) || !DirectoryMode && File.Exists(Path))
                {
                    UpdateGUI();
                    OnChanged?.Invoke(this, new FunctionEventArgs<string>(Path));
                }
            }
            else
            {
                Path = uiText.Text;

                if (CheckExists)
                {
                    bool exists = DirectoryMode ? Directory.Exists(Path) : File.Exists(Path);
                    if (exists)
                        uiText.ClearValue(TextBox.ForegroundProperty);
                    else
                        uiText.Foreground = Brushes.Red;
                }
            }
        }

        public void UpdateGUI()
        {
            if (ShowFullPath)
                uiText.Text = Path;
            else if (DirectoryMode)
                uiText.Text = new DirectoryInfo(Path).Name;
            else
                uiText.Text = System.IO.Path.GetFileName(Path);
        }
    }
}
