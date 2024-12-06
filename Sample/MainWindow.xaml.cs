using HaLi.WPF.Board;
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
        public MainWindow()
        {
            InitializeComponent();
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

        private void OnClick_Line(object sender, EventArgs e)
        {
            //uiBoard.Editor = new HaLi.WPF.Board.LineEdit();
            uiBoard.StartEdit<LineEdit>();
        }
    }
}