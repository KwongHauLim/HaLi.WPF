using System.Windows;
using System.Windows.Controls;

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
        DependencyProperty.Register("ShowLabel", typeof(bool), typeof(Button), new PropertyMetadata(true, OnShowLabelChanged));

    private static void OnShowLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Button uc)
        {
            var label = uc.uiLabel;
            if (uc.ShowLabel)
            {
                label.Visibility = Visibility.Visible;
            }
            else
            {
                label.Visibility = Visibility.Collapsed;
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
