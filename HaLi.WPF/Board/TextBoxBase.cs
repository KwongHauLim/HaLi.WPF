using HaLi.WPF.Helpers;
using System.Windows;

namespace HaLi.WPF.Board;

public class TextBoxBase : DrawElement<Shapes.TextBox>
{
    public double X
    {
        get { return (double)GetValue(XProperty); }
        set { SetValue(XProperty, value); }
    }

    // Using a DependencyProperty as the backing store for X.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty XProperty =
        DependencyProperty.Register("X", typeof(double), typeof(TextBoxBase), new PropertyMetadata(0d));


    public double Y
    {
        get { return (double)GetValue(YProperty); }
        set { SetValue(YProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Y.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty YProperty =
        DependencyProperty.Register("Y", typeof(double), typeof(TextBoxBase), new PropertyMetadata(0d));


    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(TextBoxBase), new PropertyMetadata(""));


}

public class TextEdit : EditBase
{
    public TextEdit()
    {
        var m = new EditMouse.EditMonitor();
        m.WhenPressed = true;
        m.On += When;
        Mouse.Monitors.Add(m);
    }

    private void When(object? sender, EditMouse.MouseArgs e)
    {
        if (e.Event == EditMouse.MouseEvent.Down)
        {
            var text = new TextBox();
            text.X = Mouse.Position.X;
            text.Y = Mouse.Position.Y;
            text.Shape.Text = "Text1234";
            Helper.CopyProperties(text.Shape, text);
            SetEdit(text);
            Board.StopEdit();
        }
    }
}