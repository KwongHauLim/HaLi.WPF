using HaLi.WPF.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HaLi.WPF.Board
{
    public class LineBase : DrawBase<Shapes.Line>
    {
        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }

        // Using a DependencyProperty as the backing store for X1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register("X1", typeof(double), typeof(LineBase), new PropertyMetadata(0d));


        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        // Using a DependencyProperty as the backing store for X2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2", typeof(double), typeof(LineBase), new PropertyMetadata(1d));


        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Y1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register("Y1", typeof(double), typeof(LineBase), new PropertyMetadata(0d));


        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Y2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2", typeof(double), typeof(LineBase), new PropertyMetadata(1d));


        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(double), typeof(LineBase), new PropertyMetadata(1d));



        public Brush Brush
        {
            get { return (Brush)GetValue(BrushProperty); }
            set { SetValue(BrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Brush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BrushProperty =
            DependencyProperty.Register("Brush", typeof(Brush), typeof(LineBase), new PropertyMetadata(Brushes.Black));



    }

    /// <summary>
    /// Interaction logic for Line.xaml
    /// </summary>
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

        protected override void UpdateGUI()
        {
            if (!IsInitialized || System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            var x1 = Math.Min(X1, X2);
            var y1 = Math.Min(Y1, Y2);
            var x2 = Math.Max(X1, X2);
            var y2 = Math.Max(Y1, Y2);

            Canvas.SetLeft(uiCanvas, x1);
            Canvas.SetTop(uiCanvas, y1);


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

            uiPoly.Fill = Brush;

            base.UpdateGUI();
        }
    }
}
