using HaLi.WPF.Helpers;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Media;

namespace HaLi.WPF.Board;

public class RectBase : DrawBase<Shapes.Rectangle>
{
    public double X
    {
        get { return (double)GetValue(XProperty); }
        set { SetValue(XProperty, value); }
    }

    // Using a DependencyProperty as the backing store for X.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty XProperty =
        DependencyProperty.Register("X", typeof(double), typeof(RectBase), new PropertyMetadata(0d));


    public double Y
    {
        get { return (double)GetValue(YProperty); }
        set { SetValue(YProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Y.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty YProperty =
        DependencyProperty.Register("Y", typeof(double), typeof(RectBase), new PropertyMetadata(0d));


    public Size Size
    {
        get { return (Size)GetValue(SizeProperty); }
        set { SetValue(SizeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Size.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SizeProperty =
        DependencyProperty.Register("Size", typeof(Size), typeof(RectBase), new PropertyMetadata(new Size(1d, 1d)));


    public double StrokeThickness
    {
        get { return (double)GetValue(StrokeThicknessProperty); }
        set { SetValue(StrokeThicknessProperty, value); }
    }

    // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register("StrokeThickness", typeof(double), typeof(RectBase), new PropertyMetadata(1d));


    public Brush Stroke
    {
        get { return (Brush)GetValue(StrokeProperty); }
        set { SetValue(StrokeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Stroke.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StrokeProperty =
        DependencyProperty.Register("Stroke", typeof(Brush), typeof(RectBase), new PropertyMetadata(Brushes.Black));


    public Brush Fill
    {
        get { return (Brush)GetValue(FillProperty); }
        set { SetValue(FillProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FillProperty =
        DependencyProperty.Register("Fill", typeof(Brush), typeof(RectBase), new PropertyMetadata(Brushes.Transparent));



}

public class RectEdit : EditBase
{
    private Rect? editing;

    public RectEdit()
    {
        var m = new EditMouse.EditMonitor();
        m.WhenPressed = true;
        m.WhenMove = true;
        m.WhenRelease = true;
        m.On += When;
        Mouse.Monitors.Add(m);
    }

    private void When(object? sender, EditMouse.MouseArgs e)
    {
        switch (e.Event)
        {
            case EditMouse.MouseEvent.Down:
                editing = new Rect();
                editing.LeftTop = editing.RightTop = editing.RightBottom = editing.LeftBottom = Mouse.StartPosition;
                editing.Shape.X = Mouse.StartPosition.X;
                editing.Shape.Y = Mouse.StartPosition.Y;
                Helper.CopyProperties(editing.Shape, editing);
                Board.uiCanvas.Children.Add(editing);
                break;
            case EditMouse.MouseEvent.Enter:
                break;
            case EditMouse.MouseEvent.Leave:
                break;
            case EditMouse.MouseEvent.Move:
                if (editing != null)
                {
                    var x2 = Mouse.Position.X;
                    var y2 = Mouse.Position.Y;

                    var left = Math.Min(editing.LeftTop.X, x2);
                    var top = Math.Min(editing.LeftTop.Y, y2);
                    var right = Math.Max(editing.RightBottom.X, x2);
                    var bottom = Math.Max(editing.RightBottom.Y, y2);

                    editing.LeftTop = new Point(left, top);
                    editing.RightTop = new Point(right, top);
                    editing.RightBottom = new Point(right, bottom);
                    editing.LeftBottom = new Point(left, bottom);

                    // Center
                    editing.X = (left + right) / 2;
                    editing.Y = (top + bottom) / 2;

                    // Width & Height
                    var w = right - left;
                    var h = bottom - top;
                    editing.Size = new Size(w, h);
                }
                break;
            case EditMouse.MouseEvent.Up:
                editing = null;
                break;
        }
    }
}