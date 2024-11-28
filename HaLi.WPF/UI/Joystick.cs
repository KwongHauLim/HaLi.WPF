using HaLi.WPF.Helpers.Hook;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HaLi.WPF.UI;

public class Joystick : Border
{
    public FrameworkElement Target
    {
        get { return (FrameworkElement)GetValue(TargetProperty); }
        set { SetValue(TargetProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TargetProperty =
        DependencyProperty.Register("Target", typeof(FrameworkElement), typeof(Joystick), new PropertyMetadata(null));


    public MouseButton Button
    {
        get { return (MouseButton)GetValue(ButtonProperty); }
        set { SetValue(ButtonProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Button.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ButtonProperty =
        DependencyProperty.Register("Button", typeof(MouseButton), typeof(Joystick), new PropertyMetadata(MouseButton.Left));


    public double Cushion
    {
        get { return (double)GetValue(CushionProperty); }
        set { SetValue(CushionProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Cushion.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CushionProperty =
        DependencyProperty.Register("Cushion", typeof(double), typeof(Joystick), new PropertyMetadata(5d));


    public bool Anywhere
    {
        get { return (bool)GetValue(AnywhereProperty); }
        set { SetValue(AnywhereProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Anywhere.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AnywhereProperty =
        DependencyProperty.Register("Anywhere", typeof(bool), typeof(Joystick), new PropertyMetadata(false));




    private long lastMouseDown;
    private Point lastMousePos;
    private bool enableDrag = false;
    private bool startDrag = false;

    public event EventHandler<MouseEventArgs> Dragging;
    public event EventHandler<MouseButtonEventArgs> DoubleClick;

    private Window Window => Window.GetWindow(this);

    public Joystick() : base()
    {
        Background = Brushes.Transparent;
        MouseDown += Joystick_MouseDown;
        MouseMove += Joystick_MouseMove;
        MouseUp += Joystick_MouseUp;
    }

    private bool IsPress(MouseEventArgs e)
    {
        switch (Button)
        {
            default: return false;
            case MouseButton.Left:
                return e.LeftButton == MouseButtonState.Pressed;
            case MouseButton.Middle:
                return e.MiddleButton == MouseButtonState.Pressed;
            case MouseButton.Right:
                return e.RightButton == MouseButtonState.Pressed;
            case MouseButton.XButton1:
                return e.XButton1 == MouseButtonState.Pressed;
            case MouseButton.XButton2:
                return e.XButton2 == MouseButtonState.Pressed;
        }
    }

    private bool IsPress(MouseHookEventArgs e)
    {
        switch (Button)
        {
            default: return false;
            case MouseButton.Left:
                return e.MessageType == MouseHookMessageType.LeftButtonDown;
            case MouseButton.Right:
                return e.MessageType == MouseHookMessageType.RightButtonDown;
        }
    }

    private bool IsRelease(MouseHookEventArgs e)
    {
        switch (Button)
        {
            default: return false;
            case MouseButton.Left:
                return e.MessageType == MouseHookMessageType.LeftButtonUp;
            case MouseButton.Right:
                return e.MessageType == MouseHookMessageType.RightButtonUp;
        }
    }

    private void Joystick_MouseDown(object sender, MouseButtonEventArgs e)
    {
        enableDrag = false;
        startDrag = false;

        if (IsPress(e))
        {
            if (Environment.TickCount64 - lastMouseDown < 350)
            {
                if (DoubleClick != null)
                {
                    var currMousePos = e.GetPosition(this);
                    if (Math.Abs(currMousePos.X - lastMousePos.X) < 5d && Math.Abs(currMousePos.Y - lastMousePos.Y) < 5d)
                    {
                        DoubleClick(this, e);
                    }
                }

                // reset the tick time
                lastMouseDown = 0;
            }
            else if (Anywhere)
            {
                MouseHook.Start();
                MouseHook.StatusChanged += MouseHook_StatusChanged;
            }
            else
            {
                // mark down the mouse position
                lastMousePos = e.GetPosition(this);
                // mark down the tick time for check is double click
                lastMouseDown = Environment.TickCount64;
                enableDrag = true;
            }
        }
    }

    private void MouseHook_StatusChanged(object? sender, MouseHookEventArgs e)
    {
        if (!enableDrag)
        {
            lastMousePos = new Point(e.Point.X, e.Point.Y);
            enableDrag = true;
        }

        if (IsRelease(e))
        {
            enableDrag = false;
            startDrag = false;

            MouseHook.Stop();
            MouseHook.StatusChanged -= MouseHook_StatusChanged;
        }
        else if (startDrag && e.MessageType == MouseHookMessageType.MouseMove)
        {
            var p = e.Point;
            var dx = p.X - lastMousePos.X;
            var dy = p.Y - lastMousePos.Y;
            Canvas.SetLeft(Target, Canvas.GetLeft(Target) + dx);
            Canvas.SetTop(Target, Canvas.GetTop(Target) + dy);
            lastMousePos = new Point(p.X, p.Y);
        }
    }

    private void Joystick_MouseMove(object sender, MouseEventArgs e)
    {
        if (enableDrag && IsPress(e) && Target != null)
        {
            var nowPos = e.GetPosition(this);
            if (!startDrag)
                startDrag = (Math.Abs(nowPos.X - lastMousePos.X) > Cushion || Math.Abs(nowPos.Y - lastMousePos.Y) > Cushion);

            if (startDrag && !Anywhere)
            {
                var win = Window;
                if (win != null && Target == win)
                {
                    if (win.WindowState == WindowState.Maximized)
                    {
                        win.DragMove();
                    }
                }
                else
                {
                    double dx = nowPos.X - lastMousePos.X;
                    double dy = nowPos.Y - lastMousePos.Y;
                    Canvas.SetLeft(Target, Canvas.GetLeft(Target) + dx);
                    Canvas.SetTop(Target, Canvas.GetTop(Target) + dy);
                }

                Dragging?.Invoke(this, e);
                lastMousePos = nowPos;
            }
        }
    }

    private void Joystick_MouseUp(object sender, MouseButtonEventArgs e)
    {
        startDrag = false;
        enableDrag = IsPress(e);
    }
}
