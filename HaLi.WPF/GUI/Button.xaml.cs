using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HaLi.WPF.GUI;

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
        DependencyProperty.Register("ShowLabel", typeof(bool), typeof(Button), new PropertyMetadata(true, OnChanged));

    public Geometry Icon
    {
        get { return (Geometry)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(Geometry), typeof(Button), new PropertyMetadata(null, OnChanged));

    private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Button uc)
        {
            var label = uc.uiLabel;
            if (uc.ShowLabel && !string.IsNullOrEmpty(uc.Label))
            {
                label.Visibility = Visibility.Visible;
            }
            else
            {
                label.Visibility = Visibility.Collapsed;
            }
        }
    }

    public string Label
    {
        get { return (string)GetValue(LabelProperty); }
        set { SetValue(LabelProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register("Label", typeof(string), typeof(Button), new PropertyMetadata(""));




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
