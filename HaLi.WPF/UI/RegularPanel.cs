using System.Windows;
using System.Windows.Controls;

namespace HaLi.WPF.UI;

public class RegularPanel : Panel
{
    public int Columns
    {
        get { return (int)GetValue(ColumnsProperty); }
        set { SetValue(ColumnsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ColumnsProperty =
        DependencyProperty.Register("Columns", typeof(int), typeof(RegularPanel), new PropertyMetadata(2));


    public int ColumnFill
    {
        get { return (int)GetValue(ColumnFillProperty); }
        set { SetValue(ColumnFillProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ColumnFill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ColumnFillProperty =
        DependencyProperty.Register("ColumnFill", typeof(int), typeof(RegularPanel), new PropertyMetadata(-1));

    private double[] widths;
    private double[] height;
    private int fillIndex;

    public int Rows { get; private set; }

    protected override Size MeasureOverride(Size availableSize)
    {
        Rows = Children.Count / Columns;
        if (Children.Count % Columns > 0)
            Rows++;

        var sizeMeasure = new Size[Columns, Rows];
        widths = new double[Columns];
        height = new double[Rows];

        fillIndex = (ColumnFill >= 0 && ColumnFill < Columns) ? ColumnFill : Columns - 1;

        for (int i = 0; i < Children.Count; i++)
        {
            var col = i % Columns;
            var row = i / Columns;
            if (col != fillIndex)
            {
                var child = Children[i];
                child.Measure(availableSize);
                sizeMeasure[col, row] = child.DesiredSize; 
            }
        }

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                if (sizeMeasure[i, j] != null)
                {
                    widths[i] = Math.Max(widths[i], sizeMeasure[i, j].Width);
                    height[j] = Math.Max(height[j], sizeMeasure[i, j].Height);
                }
            }
        }

        if (fillIndex >= 0 && fillIndex < widths.Length)
        {
            widths[fillIndex] = 0;
            widths[fillIndex] = availableSize.Width - widths.Sum();

            for (int j = 0; j < Rows; j++)
            {
                var size = new Size(widths[fillIndex], height[j]);
                sizeMeasure[fillIndex, j] = size;

                if (size != null)
                {
                    int idx = j * Columns + fillIndex;
                    if (idx >= 0 && idx < Children.Count)
                    {
                        var child = Children[idx];
                        child.Measure(size);
                    }

                }
            }
        }

        return new Size(widths.Sum(), height.Sum());
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
