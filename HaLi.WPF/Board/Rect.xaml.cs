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

namespace HaLi.WPF.Board;

/// <summary>
/// Interaction logic for Rect.xaml
/// </summary>
public partial class Rect : RectBase
{
    public Point LeftTop { get; set; }
    public Point RightTop { get; set; }
    public Point RightBottom { get; set; }
    public Point LeftBottom { get; set; }

    public Rect()
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

        Canvas.SetLeft(this, LeftTop.X);
        Canvas.SetTop(this, LeftTop.Y);

        uiRect.Width = Size.Width;
        uiRect.Height = Size.Height;
        uiRect.Stroke = Stroke;
        uiRect.StrokeThickness = StrokeThickness;
        uiRect.Fill = Fill;
        //Stroke = Shape.Stroke;
        //StrokeThickness = Shape.StrokeThickness;

        base.UpdateGUI();
    }
}
