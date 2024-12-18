using HaLi.WPF.Helpers;
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

namespace HaLi.WPF.Board
{
    /// <summary>
    /// Interaction logic for TextBox.xaml
    /// </summary>
    public partial class TextBox : TextBoxBase
    {
        public TextBox()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            base.OnLoaded(sender, e);

            SetBinding(WidthProperty, GuiHelper.OneWay(Parent, "ActualWidth"));
            SetBinding(HeightProperty, GuiHelper.OneWay(Parent, "ActualHeight"));
        }

        protected override void UpdateGUI()
        {
            if (!IsInitialized || System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            Canvas.SetLeft(this, X);
            Canvas.SetTop(this, Y);

            uiText.Text = Text;

            base.UpdateGUI();
        }

        public override void StopEdit()
        {
            base.StopEdit();
            uiCanvas.IsHitTestVisible = false;
        }

        private void OnKey(object sender, KeyEventArgs e)
        {
            // if key "Enter" is pressed
            if (e.Key == Key.Enter)
            {
                uiText.IsReadOnly = true;
                uiText.BorderThickness = new Thickness(0);

                // remove focus from the textbox
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this), null);
            }
        }
    }
}
