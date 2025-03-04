﻿using HaLi.WPF.Helpers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace HaLi.WPF.Board;

public class LineBase : DrawElement<Shapes.Line>
{
    public double X1
    {
        get { return (double)GetValue(X1Property); }
        set { SetValue(X1Property, value); }
    }

    // Using a DependencyProperty as the backing store for X1.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty X1Property =
        DependencyProperty.Register("X1", typeof(double), typeof(LineBase), new PropertyMetadata(0d));


    public double X2
    {
        get { return (double)GetValue(X2Property); }
        set { SetValue(X2Property, value); }
    }

    // Using a DependencyProperty as the backing store for X2.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty X2Property =
        DependencyProperty.Register("X2", typeof(double), typeof(LineBase), new PropertyMetadata(1d));


    public double Y1
    {
        get { return (double)GetValue(Y1Property); }
        set { SetValue(Y1Property, value); }
    }

    // Using a DependencyProperty as the backing store for Y1.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty Y1Property =
        DependencyProperty.Register("Y1", typeof(double), typeof(LineBase), new PropertyMetadata(0d));


    public double Y2
    {
        get { return (double)GetValue(Y2Property); }
        set { SetValue(Y2Property, value); }
    }

    // Using a DependencyProperty as the backing store for Y2.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty Y2Property =
        DependencyProperty.Register("Y2", typeof(double), typeof(LineBase), new PropertyMetadata(1d));


    public double Thickness
    {
        get { return (double)GetValue(ThicknessProperty); }
        set { SetValue(ThicknessProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Thickness.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ThicknessProperty =
        DependencyProperty.Register("Thickness", typeof(double), typeof(LineBase), new PropertyMetadata(1d));



    public Brush Brush
    {
        get { return (Brush)GetValue(BrushProperty); }
        set { SetValue(BrushProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Brush.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty BrushProperty =
        DependencyProperty.Register("Brush", typeof(Brush), typeof(LineBase), new PropertyMetadata(Brushes.Black));



}

public class LineEdit : EditBase
{
    public LineEdit()
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
                var line = new Line();
                line.Shape.X1 = line.Shape.X2 = Mouse.StartPosition.X;
                line.Shape.Y1 = line.Shape.Y2 = Mouse.StartPosition.Y;
                Helper.CopyProperties(line.Shape, line);
                SetEdit(line);
                break;
            case EditMouse.MouseEvent.Enter:
                break;
            case EditMouse.MouseEvent.Leave:
                break;
            case EditMouse.MouseEvent.Move:
                if (Editing is Line l)
                {
                    l.X2 = Mouse.Position.X;
                    l.Y2 = Mouse.Position.Y;
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