using HaLi.WPF.Helpers;
using HandyControl.Tools.Extension;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json.Linq;

namespace HaLi.WPF.Board;

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class DrawBase : UserControl
{

    protected bool _flagBatch;
    protected bool _flagChanged;
    protected bool _flagEditing;

    protected virtual bool IsWholeCanvas => true;

    public abstract JObject Export();
    public abstract void Import(JToken json);

    protected override void OnInitialized(EventArgs e)
    {
        _flagBatch = true;

        base.OnInitialized(e);

        // skip in designer
        if (DesignerProperties.GetIsInDesignMode(this))
            return;

        ChildInit();

        Loaded += OnLoaded;
    }

    protected virtual void ChildInit()
    {
        _flagChanged = true;
    }

    protected virtual void OnLoaded(object sender, RoutedEventArgs e)
    {
        _flagBatch = false;

        if (IsWholeCanvas)
        {
            SetBinding(WidthProperty, GuiHelper.OneWay(Parent, "ActualWidth"));
            SetBinding(HeightProperty, GuiHelper.OneWay(Parent, "ActualHeight"));
        }

        UpdateGUI();
    }

    protected virtual void UpdateGUI()
    {
        _flagChanged = false;
    }

    public virtual void StartEdit()
    {
        _flagEditing = true;
    }

    public virtual void StopEdit()
    {
        _flagEditing = false;
    }
}

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class DrawElement<T> : DrawBase, INotifyPropertyChanged
    where T : Shapes.Shape, new()
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public T Shape { get; set; }


    protected override void OnInitialized(EventArgs e)
    {
        if (Shape == null)
        {
            Shape = new T();
            OnNewShape(Shape);
        }

        base.OnInitialized(e);
    }

    protected virtual void OnNewShape(T shape)
    {
    }

    public override JObject Export() => JObject.FromObject(Shape);

    public override void Import(JToken json)
    {
        Shape = json.ToObject<T>() ?? Shape;

        // Use reflection to copy values from Shape to this class properties
        Type shapeType = typeof(T);
        PropertyInfo[] shapeProperties = shapeType.GetProperties();

        _flagBatch = true;

        foreach (PropertyInfo shapeProperty in shapeProperties)
        {
            PropertyInfo classProperty = GetType().GetProperty(shapeProperty.Name);
            if (classProperty != null && classProperty.CanWrite)
            {
                object shapeValue = shapeProperty.GetValue(Shape);
                classProperty.SetValue(this, shapeValue);
            }
        }

        _flagBatch = false;
        UpdateGUI();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (Shape != null)
        {
            var name = e.Property.Name;
            if (IsInitialized)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

            if (!_flagBatch)
            {
                Type shapeType = typeof(T);
                PropertyInfo propertyInfo = shapeType.GetProperty(name);

                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(Shape, e.NewValue);

                    UpdateGUI();
                }
            }
        }
    }
}

public abstract class EditBase
{
    public BoardCanvas Board { get; internal set; }
    public DrawBase? Editing { get; internal set; }

    public class EditMouse
    {
        public Point Position { get; private set; }
        public Point StartPosition { get; private set; }
        public Point EndPosition { get; private set; }
        public Point LastPosition { get; private set; }
        public bool IsDown { get; private set; }
        public bool InArea { get; private set; }

        internal virtual void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Position = e.GetPosition((IInputElement)sender);
            StartPosition = Position;
            LastPosition = Position;
            IsDown = true;
            var args = new MouseArgs { Event = MouseEvent.Down };
            Monitors.Where(m => m.WhenPressed).Do(m => m.InvokeEvent(this, args));
        }

        internal virtual void OnMouseEnter(object sender, MouseEventArgs e)
        {
            Position = e.GetPosition((IInputElement)sender);
            InArea = true;
            var args = new MouseArgs { Event = MouseEvent.Enter };
            Monitors.Where(m => m.WhenEnter).Do(m => m.InvokeEvent(this, args));
        }

        internal virtual void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Position = e.GetPosition((IInputElement)sender);
            InArea = false;
            var args = new MouseArgs { Event = MouseEvent.Leave };
            Monitors.Where(m => m.WhenLeave).Do(m => m.InvokeEvent(this, args));
        }

        internal virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            LastPosition = Position;
            Position = e.GetPosition((IInputElement)sender);
            var args = new MouseArgs { Event = MouseEvent.Move };
            Monitors.Where(m => m.WhenMove).Do(m => m.InvokeEvent(this, args));
        }

        internal virtual void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            IsDown = false;
            var args = new MouseArgs { Event = MouseEvent.Up };
            Monitors.Where(m => m.WhenRelease).Do(m => m.InvokeEvent(this, args));
        }

        public enum MouseEvent
        {
            Down,
            Enter,
            Leave,
            Move,
            Up
        }

        public class MouseArgs : EventArgs
        {
            public MouseEvent Event { get; set; }
        }

        public class EditMonitor
        {
            public bool WhenEnter { get; set; }
            public bool WhenLeave { get; set; }
            public bool WhenPressed { get; set; }
            public bool WhenRelease { get; set; }
            public bool WhenMove { get; set; }

            public event EventHandler<MouseArgs>? On;
            internal void InvokeEvent(EditMouse edit, MouseArgs args)
            {
                On?.Invoke(edit, args);
            }
        }
        public List<EditMonitor> Monitors { get; set; } = new();
    }

    public EditMouse Mouse { get; } = new();

    public class EditKeyboard
    {
        private HashSet<Key> pressedKeys = new HashSet<Key>();

        internal void OnKeyDown(object sender, KeyEventArgs e)
        {
            pressedKeys.Add(e.Key);

            foreach (var m in Monitors)
            {
                if (m.WhenPressed && m.Match(pressedKeys))
                {
                    m.InvokeEvent(this);
                }
            }
        }

        internal void OnKeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys.Remove(e.Key);

            foreach (var m in Monitors)
            {
                if (m.WhenRelease && m.Match(pressedKeys))
                {
                    m.InvokeEvent(this);
                }
            }
        }

        public class EditMonitor
        {
            public List<Key> Keys { get; private set; } = new();
            public bool WhenPressed { get; set; }
            public bool WhenRelease { get; set; }

            public event EventHandler? On;
            internal void InvokeEvent(EditKeyboard edit)
            {
                On?.Invoke(edit, EventArgs.Empty);
            }

            public EditMonitor(params Key[] keys)
            {
                Keys.AddRange(keys);
            }

            internal bool Match(HashSet<Key> pressedKeys)
            {
                bool match = true;

                foreach (var key in Keys)
                {
                    if (!pressedKeys.Contains(key))
                    {
                        match = false;
                        break;
                    }
                }

                return match;
            }
        }
        public List<EditMonitor> Monitors { get; set; } = new();
    }

    public EditKeyboard Keyboard { get; } = new();

    internal protected virtual void StartEdit()
    {
        Editing?.StartEdit();
    }

    internal protected virtual void StopEdit()
    {
        Editing?.StopEdit();
    }

    protected void SetEdit(DrawBase element)
    {
        Editing = element;
        if (element != null)
        {
            Board.uiCanvas.Children.Add(element);
        }
    }
}

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class DrawOnCanvas<T> : DrawElement<T>
    where T : Shapes.Shape, new()
{
    public double X
    {
        get { return (double)GetValue(XProperty); }
        set { SetValue(XProperty, value); }
    }

    // Using a DependencyProperty as the backing store for X.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty XProperty =
        DependencyProperty.Register("X", typeof(double), typeof(DrawOnCanvas<T>), new PropertyMetadata(0d));


    public double Y
    {
        get { return (double)GetValue(YProperty); }
        set { SetValue(YProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Y.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty YProperty =
        DependencyProperty.Register("Y", typeof(double), typeof(DrawOnCanvas<T>), new PropertyMetadata(0d));


    protected override void UpdateGUI()
    {
        if (!IsInitialized || System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            return;

        Canvas.SetLeft(this, X);
        Canvas.SetTop(this, Y);

        base.UpdateGUI();
    }
}