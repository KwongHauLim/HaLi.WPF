using HaLi.WPF.Board;
using HaLi.WPF.GUI;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Geo> Buttons = new List<Geo>();

        public MainWindow()
        {
            InitializeComponent();

            Buttons.AddRange(new Geo[] { uiLine, uiRect });
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            PreviewKeyDown += (_, e) =>
            {
                //if(e.Key == Key.C)
                //{
                //    ui.Clear();
                //}
                //else
                //if (e.Key == Key.E)
                //{
                //    ui.Export(@"C:\Antelope\Output\hand.json");
                //}
                //else if (e.Key == Key.I)
                //{
                //    ui.Import(@"C:\Antelope\Output\hand.json");
                //}
            };
        }

        private void WhiteBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void StartEdit<T>(HaLi.WPF.GUI.Geo block)
            where T : EditBase, new()
        {
            foreach (var item in Buttons)
            {
                item.Background = Brushes.Transparent;
            }

            block.Background = Brushes.CornflowerBlue;
            uiBoard.StartEdit<T>();
        }

        private void OnClick_Load(object sender, EventArgs e)
        {

        }

        private void OnClick_Save(object sender, EventArgs e)
        {
            var data = uiBoard.Export();
        }

        private void OnClick_Line(object sender, EventArgs e) => StartEdit<LineEdit>(uiLine);

        private void OnClick_Rect(object sender, EventArgs e) => StartEdit<RectEdit>(uiRect);

        private void OnClick_Hand(object sender, EventArgs e) => StartEdit<HandEdit>(uiHand);

        private void OnClick_Text(object sender, EventArgs e) => StartEdit<TextEdit>(uiText);

        private void OnClick_Image(object sender, EventArgs e) => StartEdit<ImageEdit>(uiImage);
    }
}