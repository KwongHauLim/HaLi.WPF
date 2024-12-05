using HaLi.WPF.Helpers;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace HaLi.WPF.Board
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DrawBase<T> : UserControl, INotifyPropertyChanged
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
}
