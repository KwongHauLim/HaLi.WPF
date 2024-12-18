using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HaLi.WPF.Board.Shapes;

public abstract class Shape
{
}

public class Line : Shape
{
    public double X1 { get; set; }
    public double Y1 { get; set; }
    public double X2 { get; set; }
    public double Y2 { get; set; }
    public double Thickness { get; set; } = 1d;
    public Color Color { get; set; } = Colors.Black;
}

public class Rectangle : Shape
{
    public double X { get; set; }
    public double Y { get; set; }
    public Size Size { get; set; } = new Size(1d, 1d);
    public double StrokeThickness { get; set; } = 1d;
    public Color Stroke { get; set; } = Colors.Black;
    public Color Fill { get; set; } = Colors.Transparent;
}

public class Hand : Shape
{
    public Color Color { get; set; } = Colors.Black;
    public double CursorSize { get; set; }
    public double BrushSize { get; set; }


    public class StrokeData
    {
        public byte[] StylusPoints { get; set; }
        public Color Color { get; set; } = Colors.Black;
        public double BrushSize { get; set; }
    }

    public List<StrokeData> Datas { get; set; }
}

public class TextBox : Shape
{
    public string Text { get; set; } = "";
}

public class Image : Shape
{
    public string Data { get; set; } = "";
}