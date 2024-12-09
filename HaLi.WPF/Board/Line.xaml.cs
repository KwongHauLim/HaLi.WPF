using HaLi.WPF.Helpers;
using System.ComponentModel;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HaLi.WPF.Board;


/// <summary>
/// Interaction logic for Line.xaml
/// </summary>
[EditorBrowsable(EditorBrowsableState.Always)]
public partial class Line : LineBase
{
    public Line()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
        base.OnLoaded(sender, e);

        SetBinding(WidthProperty, GuiHelper.OneWay(Parent, "ActualWidth"));
        SetBinding(HeightProperty, GuiHelper.OneWay(Parent, "ActualHeight"));
    }

    public override void Import(JsonDocument json)
    {
        Brush = null;

        base.Import(json);
    }

    protected override void UpdateGUI()
    {
        if (!IsInitialized || System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            return;

        var x1 = X1;
        var y1 = Y1;
        var x2 = X2;
        var y2 = Y2;

        Canvas.SetLeft(this, Math.Min(x1, x2));
        Canvas.SetTop(this, Math.Min(y1, y2));


        double h1 = Thickness / 2d;

        var pointA = new Point(0, 0);
        var pointB = new Point(0, 0);
        var pointC = new Point(0, 0);
        var pointD = new Point(0, 0);

        //
        //  A------------B
        //  O            |
        //  D------------C
        //

        // length by (x,y) to (x2,y2)
        double len = MathHelper.Length(x1, y1, x2, y2);
        // anlge by (x,y) to (x2,y2)
        double angle = MathHelper.Angle(x1, y1, x2, y2);

        // rotate, calc square size should be
        var lefttop = MathHelper.GetPointRotate(new Point(0, 0), angle);
        var rightbottom = MathHelper.GetPointRotate(new Point(len, 0), angle);
        var w = uiCanvas.Width = Math.Abs(rightbottom.X - lefttop.X);
        var h = uiCanvas.Height = Math.Abs(rightbottom.Y - lefttop.Y);


        //
        //  A---B
        //  |\ /|
        //  | X |
        //  |/ \|
        //  D---C
        //
        var sqrA = new Point(0, 0);
        var sqrB = new Point(w, 0);
        var sqrC = new Point(w, h);
        var sqrD = new Point(0, h);
        var offT = MathHelper.GetPointRotate(new Point(0, -h1), angle);
        var offB = MathHelper.GetPointRotate(new Point(0, h1), angle);
        // swicth quater by angle
        if (angle > 0d && angle <= 90d)
        {
            // line from square left top to right bottom
            pointA = new Point(sqrA.X + offT.X, sqrA.Y + offT.Y);
            pointB = new Point(sqrC.X + offT.X, sqrC.Y + offT.Y);
            pointC = new Point(sqrC.X + offB.X, sqrC.Y + offB.Y);
            pointD = new Point(sqrA.X + offB.X, sqrA.Y + offB.Y);
        }
        else if (angle > 90d && angle <= 180d)
        {
            // line from square right top to left bottom
            pointA = new Point(sqrB.X + offT.X, sqrB.Y + offT.Y);
            pointB = new Point(sqrD.X + offT.X, sqrD.Y + offT.Y);
            pointC = new Point(sqrD.X + offB.X, sqrD.Y + offB.Y);
            pointD = new Point(sqrB.X + offB.X, sqrB.Y + offB.Y);
        }
        else if (angle > 180d && angle <= 270d)
        {
            // line from square right bottom to left top
            pointA = new Point(sqrC.X + offT.X, sqrC.Y + offT.Y);
            pointB = new Point(sqrA.X + offT.X, sqrA.Y + offT.Y);
            pointC = new Point(sqrA.X + offB.X, sqrA.Y + offB.Y);
            pointD = new Point(sqrC.X + offB.X, sqrC.Y + offB.Y);
        }
        else if (angle > 270d && angle <= 360d)
        {
            // line from square left bottom to right top
            pointA = new Point(sqrD.X + offT.X, sqrD.Y + offT.Y);
            pointB = new Point(sqrB.X + offT.X, sqrB.Y + offT.Y);
            pointC = new Point(sqrB.X + offB.X, sqrB.Y + offB.Y);
            pointD = new Point(sqrD.X + offB.X, sqrD.Y + offB.Y);
        }

        uiPoly.Points.Clear();
        uiPoly.Points.Add(pointA);
        uiPoly.Points.Add(pointB);
        uiPoly.Points.Add(pointC);
        uiPoly.Points.Add(pointD);

        if (Brush == null)
        {
            // When reload, i clean the brush, so i need to reassign to new color
            Brush = new SolidColorBrush(Shape.Color);
        }
        uiPoly.Fill = Brush;

        base.UpdateGUI();
    }
}
