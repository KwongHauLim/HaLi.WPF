using HaLi.WPF.Board;
using HaLi.WPF.GUI;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sample.Panels
{
    /// <summary>
    /// Interaction logic for BoardCanvasSample.xaml
    /// </summary>
    public partial class BoardCanvasSample : UserControl
    {
        private List<Geo> Buttons = new List<Geo>();

        public BoardCanvasSample()
        {
            InitializeComponent();

            Buttons.AddRange(new Geo[] { uiLine, uiRect, uiText, uiHand, uiImage });
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


        private void WhiteBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
