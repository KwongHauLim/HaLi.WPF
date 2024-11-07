using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HaLi.WPF.UI
{
    /// <summary>
    /// Basic UI with a label
    /// </summary>
    public class LabeledUI : ContentControl
    {
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(LabeledUI), new PropertyMetadata(string.Empty));

        public Thickness LabelMargin
        {
            get { return (Thickness)GetValue(LabelMarginProperty); }
            set { SetValue(LabelMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelMarginProperty =
            DependencyProperty.Register("LabelMargin", typeof(Thickness), typeof(LabeledUI), new PropertyMetadata(new Thickness(6,4,6,4)));


        public Brush LabelBrush
        {
            get { return (Brush)GetValue(LabelBrushProperty); }
            set { SetValue(LabelBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelBrushProperty =
            DependencyProperty.Register("LabelBrush", typeof(Brush), typeof(LabeledUI), new PropertyMetadata(Brushes.Black));


        public Dock LabelPlace
        {
            get { return (Dock)GetValue(LabelPlaceProperty); }
            set { SetValue(LabelPlaceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelPlace.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelPlaceProperty =
            DependencyProperty.Register("LabelPlace", typeof(Dock), typeof(LabeledUI), new PropertyMetadata(Dock.Top));


        public HorizontalAlignment LabelHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(LabelHorizontalAlignmentProperty); }
            set { SetValue(LabelHorizontalAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelHorizontalAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelHorizontalAlignmentProperty =
            DependencyProperty.Register("LabelHorizontalAlignment", typeof(HorizontalAlignment), typeof(LabeledUI), new PropertyMetadata(HorizontalAlignment.Left));


        public VerticalAlignment LabelVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(LabelVerticalAlignmentProperty); }
            set { SetValue(LabelVerticalAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelVerticalAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelVerticalAlignmentProperty =
            DependencyProperty.Register("LabelVerticalAlignment", typeof(VerticalAlignment), typeof(LabeledUI), new PropertyMetadata(VerticalAlignment.Center));


        public double LabelWidth
        {
            get { return (double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof(double), typeof(LabeledUI), new PropertyMetadata(double.NaN));


        public double LabelHeight
        {
            get { return (double)GetValue(LabelHeightProperty); }
            set { SetValue(LabelHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelHeightProperty =
            DependencyProperty.Register("LabelHeight", typeof(double), typeof(LabeledUI), new PropertyMetadata(double.NaN));


        public bool MustFlag
        {
            get { return (bool)GetValue(MustFlagProperty); }
            set { SetValue(MustFlagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MustFlag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MustFlagProperty =
            DependencyProperty.Register("MustFlag", typeof(bool), typeof(LabeledUI), new PropertyMetadata(false));


        public string MustSymbols
        {
            get { return (string)GetValue(MustSymbolsProperty); }
            set { SetValue(MustSymbolsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MustSymbols.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MustSymbolsProperty =
            DependencyProperty.Register("MustSymbols", typeof(string), typeof(LabeledUI), new PropertyMetadata("*"));


        public Brush MustBrush
        {
            get { return (Brush)GetValue(MustBrushProperty); }
            set { SetValue(MustBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MustBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MustBrushProperty =
            DependencyProperty.Register("MustBrush", typeof(Brush), typeof(LabeledUI), new PropertyMetadata(Brushes.Red));





        static LabeledUI()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabeledUI), new FrameworkPropertyMetadata(typeof(LabeledUI)));
        }
    }
}
