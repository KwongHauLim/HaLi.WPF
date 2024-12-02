using HaLi.WPF.Helpers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HaLi.WPF.UI;

public class RegularPanel : Panel, INotifyPropertyChanged
{
    public int Columns
    {
        get { return (int)GetValue(ColumnsProperty); }
        set { SetValue(ColumnsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ColumnsProperty =
        DependencyProperty.Register("Columns", typeof(int), typeof(RegularPanel), new PropertyMetadata(2, OnPropertyChanged));


    public string ColumsWidth
    {
        get { return (string)GetValue(ColumsWidthProperty); }
        set { SetValue(ColumsWidthProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ColumsWidth.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ColumsWidthProperty =
        DependencyProperty.RegisterAttached("ColumsWidth", typeof(string), typeof(RegularPanel), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, OnPropertyChanged), new ValidateValueCallback(IsValidWidth));

    private static bool IsValidWidth(object value)
    {
        return value != null;
    }

    //DependencyProperty.Register("ColumsWidth", typeof(string), typeof(RegularPanel), new PropertyMetadata("", OnPropertyChanged));

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RegularPanel panel)
        {
            panel.InvalidateVisual();
            panel.PropertyChanged?.Invoke(panel, new PropertyChangedEventArgs(e.Property.Name));
        }
    }

    private double[] widths;
    private double[] height;
    private int fillIndex;

    public event PropertyChangedEventHandler? PropertyChanged;

    public int Rows { get; private set; }

    protected override Size MeasureOverride(Size availableSize)
    {
        Rows = Children.Count / Columns;
        if (Children.Count % Columns > 0)
            Rows++;

        var gridChildren = new UIElement[Columns, Rows];
        var gridSizes = new Size[Columns, Rows];
        widths = new double[Columns];
        height = new double[Rows];

        if (string.IsNullOrEmpty(ColumsWidth))
        {
            ColumsWidth = string.Join(',', Enumerable.Repeat("*", Columns));
        }

        var defines = GridHelper.ConvertToGridLengths(ColumsWidth);
        if (defines.Length < Columns)
            Array.Resize(ref defines, Columns);
        for (int i = 0; i < defines.Length; i++)
        {
            if (defines[i] == null)
                defines[i] = new GridLength(1, GridUnitType.Auto);
        }

        for (int i = 0; i < Children.Count; i++)
        {
            var col = i % Columns;
            var row = i / Columns;
            gridChildren[col, row] = Children[i];
        }

        var remainWidth = availableSize.Width;
        var remainHeight = availableSize.Height;

        // First measure absolute columns
        for (int c = 0; c < Columns; c++)
        {
            var length = defines[c];
            if (length.IsAbsolute)
            {
                var size = new Size(length.Value, remainHeight);
                for (int r = 0; r < Rows; r++)
                {
                    MeasureChild(c, r, size, true);
                }
                remainWidth -= length.Value;
            }
        }

        // Second measure Auto columns
        for (int c = 0; c < Columns; c++)
        {
            var length = defines[c];
            if (length.IsAuto)
            {
                var size = new Size(remainWidth, remainHeight);
                for (int r = 0; r < Rows; r++)
                {
                    MeasureChild(c, r, size, false);
                }
                remainWidth -= widths[c];
            }
        }

        // First count how many stars
        var starCount = 0d;
        for (int c = 0; c < Columns; c++)
        {
            var length = defines[c];
            if (length.IsStar)
            {
                starCount += length.Value;
            }
        }
        var starWidth = remainWidth / starCount;
        // Then finally measure Stars columns
        for (int c = 0; c < Columns; c++)
        {
            var length = defines[c];
            if (length.IsStar)
            {
                var size = new Size(starWidth * length.Value, remainHeight);
                for (int r = 0; r < Rows; r++)
                {
                    MeasureChild(c, r, size, true);
                }
            }
        }

        return new Size(widths.Sum(), height.Sum());

        void MeasureChild(int c, int r, Size size, bool fixedWidth = false)
        {
            var child = gridChildren[c, r];
            if (child != null)
            {
                child.Measure(size);
                gridSizes[c, r] = child.DesiredSize;

                if (fixedWidth)
                    widths[c] = Math.Max(widths[c], size.Width);
                else
                    widths[c] = Math.Max(widths[c], gridSizes[c, r].Width);
                height[r] = Math.Max(height[r], gridSizes[c, r].Height); 
            }
        }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        int col = 0;
        int row = 0;
        double x = 0d;
        double y = 0d;
        double w = 0d;
        double h = 0d;

        foreach (UIElement child in InternalChildren)
        {
            w = widths[col];
            h = height[row];

            child.Arrange(new Rect(x, y, w, h));
            x += w;
            col++;

            if (col >= Columns)
            {
                col = 0;
                row++;
                x = 0;
                y += h;
            }
        }        

        return finalSize;
    }
}
