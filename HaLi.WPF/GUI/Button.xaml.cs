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

namespace HaLi.WPF.GUI
{
    /// <summary>
    /// Interaction logic for Button.xaml
    /// </summary>
    public partial class Button : UserControl
    {
        public bool ShowLabel
        {
            get { return (bool)GetValue(ShowLabelProperty); }
            set { SetValue(ShowLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowLabelProperty =
            DependencyProperty.Register("ShowLabel", typeof(bool), typeof(Button), new PropertyMetadata(true, OnShowLabelChanged));

        private static void OnShowLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Button uc)
            {
                uc.uiLabel.Visibility = uc.ShowLabel ? Visibility.Visible : Visibility.Collapsed;

                if (uc.ShowLabel)
                {
                    uc.b.Width = double.NaN;
                }
                else
                {
                    uc.b.SetBinding(WidthProperty, GuiHelper.OneWay(uc.b, "ActualHeight"));
                }
            }
        }

        public Button()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            uiLabel.Visibility = ShowLabel ? Visibility.Visible : Visibility.Collapsed;

            if (ShowLabel)
            {
                b.Width = double.NaN;
            }
        }
    }
}
