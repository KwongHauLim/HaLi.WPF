using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HaLi.WPF.Helpers;

public static class MathHelper
{
    // Clamp angle between 0 and 360
    public static double Clamp360(double angle)
    {
        if (double.IsNaN(angle) || double.IsInfinity(angle))
            return 0d;

        while (angle.CompareTo(0d) < 0)
        {
            angle += 360d;
        }
        while (angle.CompareTo(360d) >= 0)
        {
            angle -= 360d;
        }
        return angle;
    }

    // Value should be positive
    public static double Pos(double value)
        => value.CompareTo(0d) < 0 ? 0d : value;
    public static double Neg(double value)
        => value.CompareTo(0d) > 0 ? 0d : value;

    public static double Min(double value, double min, double x)
        => value.CompareTo(min) < 0 ? min : value;
    public static double Max(double value, double max)
        => value.CompareTo(max) > 0 ? max : value;

    internal static double Length(double x, double y, double x2, double y2)
        => Math.Sqrt(Math.Pow(x2 - x, 2) + Math.Pow(y2 - y, 2));

    internal static double Angle(double x, double y, double x2, double y2)
        => Clamp360(Math.Atan2(y2 - y, x2 - x) * 180.0 / Math.PI);

    public static double Min(IEnumerable<double> values)
        => values.Min();
    public static double Max(IEnumerable<double> values)
        => values.Max();

    // Lerp of two points
    public static double Lerp(double a, double b, double t)
        => a + (b - a) * t;
    public static Point Lerp(Point a, Point b, double t)
        => new Point(Lerp(a.X, b.X, t), Lerp(a.Y, b.Y, t));

    internal static double Distance(double x1, double y1, double x2, double y2)
        => Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

    public static Point GetPointRotate(Point point, double angle)
    {
        double radians = angle * Math.PI / 180.0;
        double cosTheta = Math.Cos(radians);
        double sinTheta = Math.Sin(radians);

        double rotatedX = point.X * cosTheta - point.Y * sinTheta;
        double rotatedY = point.X * sinTheta + point.Y * cosTheta;

        return new Point(rotatedX, rotatedY);
    }
}
