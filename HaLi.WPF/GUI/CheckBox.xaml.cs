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
    /// Interaction logic for CheckBox.xaml
    /// </summary>
    public partial class CheckBox : UserControl
    {
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(CheckBox), new PropertyMetadata(string.Empty, OnChanged));

        public double LabelMargin
        {
            get { return (double)GetValue(LabelMarginProperty); }
            set { SetValue(LabelMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelMarginProperty =
            DependencyProperty.Register("LabelMargin", typeof(double), typeof(CheckBox), new PropertyMetadata(6d));

        public Brush LabelBrush
        {
            get { return (Brush)GetValue(LabelBrushProperty); }
            set { SetValue(LabelBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelBrushProperty =
            DependencyProperty.Register("LabelBrush", typeof(Brush), typeof(CheckBox), new PropertyMetadata(Brushes.Black, OnChanged));

        public Dock LabelPlace
        {
            get { return (Dock)GetValue(LabelPlaceProperty); }
            set { SetValue(LabelPlaceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelPlace.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelPlaceProperty =
            DependencyProperty.Register("LabelPlace", typeof(Dock), typeof(CheckBox), new PropertyMetadata(Dock.Left, OnChanged));

        public double LabelWidth
        {
            get { return (double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof(double), typeof(CheckBox), new PropertyMetadata(double.NaN, OnChanged));

        public bool MustFlag
        {
            get { return (bool)GetValue(MustFlagProperty); }
            set { SetValue(MustFlagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MustFlag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MustFlagProperty =
            DependencyProperty.Register("MustFlag", typeof(bool), typeof(CheckBox), new PropertyMetadata(false, OnChanged));

        public string MustSymbols
        {
            get { return (string)GetValue(MustSymbolsProperty); }
            set { SetValue(MustSymbolsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MustSymbols.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MustSymbolsProperty =
            DependencyProperty.Register("MustSymbols", typeof(string), typeof(CheckBox), new PropertyMetadata("*", OnChanged));

        public Brush MustBrush
        {
            get { return (Brush)GetValue(MustBrushProperty); }
            set { SetValue(MustBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MustBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MustBrushProperty =
            DependencyProperty.Register("MustBrush", typeof(Brush), typeof(CheckBox), new PropertyMetadata(Brushes.Red, OnChanged));

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(CheckBox), new PropertyMetadata(false, OnChanged));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CheckBox uc)
            {
                uc.OnChanged(uc, null);
            }
        }

        public CheckBox()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnChanged(this, null);
        }

        private void OnChanged(object sender, RoutedEventArgs e)
        {
            uiLabel.VerticalAlignment = VerticalAlignment.Center;

            if (LabelPlace == Dock.Left)
            {
                Grid.SetColumn(pLabel, 0);
                Grid.SetColumn(uiCheck, 1);

                pLabel.Margin = new Thickness(0, 0, LabelMargin, 0);
                pLabel.Width = LabelWidth;
            }
            else
            {
                Grid.SetColumn(uiCheck, 0);
                Grid.SetColumn(pLabel, 1);

                pLabel.Margin = new Thickness(LabelMargin, 0, 0, 0);
                pLabel.Width = double.NaN;
            }
        }
    }
}
