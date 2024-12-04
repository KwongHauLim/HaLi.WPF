using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace HaLi.WPF.Board
{
    public class DrawBase<T> : UserControl, INotifyPropertyChanged
        where T : Shapes.Shape, new()
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public T Shape { get; set; }

        protected bool _flagLoaded;
        protected bool _flagChanged;


        protected override void OnInitialized(EventArgs e)
        {
            Shape ??= new T();

            base.OnInitialized(e);

            // skip in designer
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            ChildInit();

            Loaded += OnLoaded;
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateGUI();
            _flagLoaded = true;
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

            var name = e.Property.Name;
            Type shapeType = typeof(T);
            PropertyInfo propertyInfo = shapeType.GetProperty(name);

            if (propertyInfo != null)
            {
                propertyInfo.SetValue(Shape, e.NewValue);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

                if (IsInitialized && _flagLoaded)
                {
                    UpdateGUI();
                }
            }            
        }
    }
}
