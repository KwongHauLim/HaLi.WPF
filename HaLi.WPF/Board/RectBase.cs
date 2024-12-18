using HaLi.WPF.Helpers;
using System.Windows;
using System.Windows.Media;

namespace HaLi.WPF.Board;

public class RectBase : DrawElement<Shapes.Rectangle>
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


    public Color Stroke
    {
        get { return (Color)GetValue(StrokeProperty); }
        set { SetValue(StrokeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Stroke.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StrokeProperty =
        DependencyProperty.Register("Stroke", typeof(Color), typeof(RectBase), new PropertyMetadata(Colors.Black));


    public Color Fill
    {
        get { return (Color)GetValue(FillProperty); }
        set { SetValue(FillProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FillProperty =
        DependencyProperty.Register("Fill", typeof(Color), typeof(RectBase), new PropertyMetadata(Colors.Transparent));



}

public class RectEdit : EditBase
{
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
                var rect = new Rect();
                rect.LeftTop = rect.RightTop = rect.RightBottom = rect.LeftBottom = Mouse.StartPosition;
                rect.Shape.X = Mouse.StartPosition.X;
                rect.Shape.Y = Mouse.StartPosition.Y;
                Helper.CopyProperties(rect.Shape, rect);
                SetEdit(rect);
                break;
            case EditMouse.MouseEvent.Enter:
                break;
            case EditMouse.MouseEvent.Leave:
                break;
            case EditMouse.MouseEvent.Move:
                if (Editing is Rect r)
                {
                    var x2 = Mouse.Position.X;
                    var y2 = Mouse.Position.Y;

                    var left = Math.Min(r.LeftTop.X, x2);
                    var top = Math.Min(r.LeftTop.Y, y2);
                    var right = Math.Max(r.RightBottom.X, x2);
                    var bottom = Math.Max(r.RightBottom.Y, y2);

                    r.LeftTop = new Point(left, top);
                    r.RightTop = new Point(right, top);
                    r.RightBottom = new Point(right, bottom);
                    r.LeftBottom = new Point(left, bottom);

                    // Center
                    r.X = (left + right) / 2;
                    r.Y = (top + bottom) / 2;

                    // Width & Height
                    var w = right - left;
                    var h = bottom - top;
                    r.Size = new Size(w, h);
                }
                break;
            case EditMouse.MouseEvent.Up:
                StopEdit();
                break;
        }
    }

    protected internal override void StopEdit()
    {
        base.StopEdit();
        Editing = null;
    }
}