using HaLi.WPF.Helpers;
using HandyControl.Tools.Extension;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HaLi.WPF.Board;

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class DrawBase<T> : UserControl, INotifyPropertyChanged
    where T : Shapes.Shape, new()
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public T Shape { get; set; }

    protected bool _flagBatch;
    protected bool _flagChanged;

    protected virtual bool IsWholeCanvas => true;

    protected override void OnInitialized(EventArgs e)
    {
        if (Shape == null)
        {
            Shape = new T();
            OnNewShape(Shape);
        }

        _flagBatch = true;

        base.OnInitialized(e);

        // skip in designer
        if (DesignerProperties.GetIsInDesignMode(this))
            return;

        ChildInit();

        Loaded += OnLoaded;
    }

    protected virtual void OnNewShape(T shape)
    {
    }

    public virtual void Export(string path)
    {
        var json = JsonSerializer.Serialize(Shape);
        System.IO.File.WriteAllText(path, json);
    }

    public void Import(string path)
    {
        var json = System.IO.File.ReadAllText(path);
        Import(JsonDocument.Parse(json));
    }

    public virtual void Import(JsonDocument json)
    {
        Shape = json.Deserialize<T>() ?? Shape;

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

    protected virtual void ChildInit()
    {
        _flagChanged = true;
    }

    protected virtual void UpdateGUI()
    {
        _flagChanged = false;
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        //if (IsInitialized)
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
            Monitors.Where(m => m.WhenPressed).Do(m => m.InvokeEvent(this));
        }

        internal virtual void OnMouseEnter(object sender, MouseEventArgs e)
        {
            Position = e.GetPosition((IInputElement)sender);
            InArea = true;
            Monitors.Where(m => m.WhenEnter).Do(m => m.InvokeEvent(this));
        }

        internal virtual void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Position = e.GetPosition((IInputElement)sender);
            InArea = false;
            Monitors.Where(m => m.WhenLeave).Do(m => m.InvokeEvent(this));
        }

        internal virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            LastPosition = Position;
            Position = e.GetPosition((IInputElement)sender);
            Monitors.Where(m => m.WhenMove).Do(m => m.InvokeEvent(this));
        }

        internal virtual void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            IsDown = false;
            Monitors.Where(m => m.WhenRelease).Do(m => m.InvokeEvent(this));
        }

        public class EditMonitor
        {
            public bool WhenEnter { get; set; }
            public bool WhenLeave { get; set; }
            public bool WhenPressed { get; set; }
            public bool WhenRelease { get; set; }
            public bool WhenMove { get; set; }

            public event EventHandler? On;
            internal void InvokeEvent(EditMouse edit)
            {
                On?.Invoke(edit, EventArgs.Empty);
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
}