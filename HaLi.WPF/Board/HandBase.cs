using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HaLi.WPF.Board;

public class HandBase : DrawElement<Shapes.Hand>
{


    public Color Color
    {
        get { return (Color)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register("Color", typeof(Color), typeof(HandBase), new PropertyMetadata(Colors.Black));


    public double CursorSize
    {
        get { return (double)GetValue(CursorSizeProperty); }
        set { SetValue(CursorSizeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CursorSize.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CursorSizeProperty =
        DependencyProperty.Register("CursorSize", typeof(double), typeof(HandBase), new PropertyMetadata(8d));


    public double BrushSize
    {
        get { return (double)GetValue(BrushSizeProperty); }
        set { SetValue(BrushSizeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for BrushSize.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty BrushSizeProperty =
        DependencyProperty.Register("BrushSize", typeof(double), typeof(HandBase), new PropertyMetadata(8d));

    /// <summary>
    /// 同一样式下, 已經寫入的筆跡也會依照新的筆跡樣式更新
    /// </summary>
    public bool OneStyle
    {
        get { return (bool)GetValue(OneStyleProperty); }
        set { SetValue(OneStyleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for OneStyle.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OneStyleProperty =
        DependencyProperty.Register("OneStyle", typeof(bool), typeof(HandBase), new PropertyMetadata(true));



    protected override void OnNewShape(Shapes.Hand shape)
    {
        base.OnNewShape(shape);

        shape.Color = Color;
        shape.CursorSize = CursorSize;
        shape.BrushSize = BrushSize;
    }

}

public class HandEdit : EditBase
{
    protected internal override void StartEdit()
    {
        base.StartEdit();

        SetEdit(new Hand());
    }
}