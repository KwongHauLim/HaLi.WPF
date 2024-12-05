using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace HaLi.WPF.Board
{
    public class DrawBase<T> : UserControl, INotifyPropertyChanged
        where T : Shapes.Shape, new()
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public T Shape { get; set; }

        protected bool _flagBatch;
        protected bool _flagChanged;


        protected override void OnInitialized(EventArgs e)
        {
            Shape ??= new T();
            _flagBatch = true;

            base.OnInitialized(e);

            // skip in designer
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            ChildInit();

            Loaded += OnLoaded;
        }

        public virtual void Reload(JsonDocument json)
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

        public virtual void Save(string path)
        {
            var json = JsonSerializer.Serialize(Shape);
            System.IO.File.WriteAllText(path, json);
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            _flagBatch = false;
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

            if (IsInitialized)
            {
                var name = e.Property.Name;
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
