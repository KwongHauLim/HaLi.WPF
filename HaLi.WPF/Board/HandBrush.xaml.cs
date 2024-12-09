using HaLi.WPF.Helpers;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static HaLi.WPF.Board.Shapes.Hand;

namespace HaLi.WPF.Board;

// ref: https://www.cnblogs.com/LCHL/p/9055642.html#4206298

/// <summary>
/// Interaction logic for HandBrush.xaml
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public partial class HandBrush : InkCanvas
{
    internal CustomDraw Drawer { get; set; }

    public HandBrush()
    {
        InitializeComponent();

        Drawer = new CustomDraw
        {
            Texture = new Uri("pack://application:,,,/HaLi.WPF;component/Resources/Images/chalk.png"),
            TextureSize = 64,
            Color = Colors.Black,
            CursorSize = 8,
            BrushSize = 8
        };
        DynamicRenderer = new CustomRenderer
        {
            Drawer = Drawer
        };


        Loaded += (_, _) =>
        {
            SetBinding(WidthProperty, GuiHelper.OneWay(Parent, "ActualWidth"));
            SetBinding(HeightProperty, GuiHelper.OneWay(Parent, "ActualHeight"));

            Background = Brushes.Transparent;
            DefaultDrawingAttributes.Color = Drawer.Color;
            DefaultDrawingAttributes.Width = Drawer.CursorSize;
            DefaultDrawingAttributes.Height = Drawer.CursorSize;
        };
    }

    protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
    {
        //感兴趣的童鞋，注释这一句看看？
        this.Strokes.Remove(e.Stroke);
        this.Strokes.Add(new CustomStroke(Drawer, e.Stroke.StylusPoints));
    }

    public void UpdateGUI()
    {
        if (Drawer != null)
        {
            DefaultDrawingAttributes.Color = Drawer.Color;
            DefaultDrawingAttributes.Width = Drawer.CursorSize;
            DefaultDrawingAttributes.Height = Drawer.CursorSize;
        }
    }

    public void ModifyStrokes()
    {
        DrawingAttributes newAttributes = DefaultDrawingAttributes.Clone();
        newAttributes.Color = Drawer.Color; // 設置為您需要的顏色
        newAttributes.Width = Drawer.CursorSize; // 設置為您需要的寬度
        newAttributes.Height = Drawer.CursorSize; // 設置為您需要的高度

        // 遍歷所有筆跡並更改它們的顏色
        foreach (var stroke in Strokes)
        {
            stroke.DrawingAttributes = newAttributes;
        }
    }

    public void Modify(Stroke stroke, Color color, double size)
    {
        DrawingAttributes newAttributes = DefaultDrawingAttributes.Clone();
        newAttributes.Color = color; // 設置為您需要的顏色
        newAttributes.Width = size; // 設置為您需要的寬度
        newAttributes.Height = size; // 設置為您需要的高度
        stroke.DrawingAttributes = newAttributes;
    }

    public void Clear()
    {
        Strokes.Clear();
    }

    internal List<StrokeData> Export()
    {
        var list = new List<StrokeData>();

        foreach (var stroke in Strokes)
        {
            var stylusPoints = stroke.StylusPoints.SelectMany(sp => new double[] { sp.X, sp.Y, sp.PressureFactor }).ToArray();
            var stylusPointsBytes = new byte[stylusPoints.Length * sizeof(double)];
            System.Buffer.BlockCopy(stylusPoints, 0, stylusPointsBytes, 0, stylusPointsBytes.Length);

            list.Add(new StrokeData
            {
                StylusPoints = stylusPointsBytes,
                Color = stroke.DrawingAttributes.Color,
                BrushSize = Math.Max(stroke.DrawingAttributes.Height, stroke.DrawingAttributes.Width)
            });
        }

        return list;
    }

    internal void Import(List<StrokeData> list)
    {
        Clear();
        foreach (var strokeData in list)
        {
            var stylusPoints = new double[strokeData.StylusPoints.Length / sizeof(double)];
            System.Buffer.BlockCopy(strokeData.StylusPoints, 0, stylusPoints, 0, strokeData.StylusPoints.Length);

            var points = new StylusPointCollection();
            for (int i = 0; i < stylusPoints.Length; i += 3)
            {
                points.Add(new StylusPoint(stylusPoints[i], stylusPoints[i + 1], (float)stylusPoints[i + 2]));
            }

            var drawingAttributes = new DrawingAttributes
            {
                Color = strokeData.Color,
                Width = strokeData.BrushSize,
                Height = strokeData.BrushSize
            };

            var drawer = new CustomDraw
            {
                Texture = Drawer.Texture,
                TextureSize = Drawer.TextureSize,
                Color = drawingAttributes.Color,
                CursorSize = drawingAttributes.Width,
                BrushSize = drawingAttributes.Height
            };
            var stroke = new CustomStroke(drawer, points, drawingAttributes);
            Strokes.Add(stroke);
        }
    }

    internal class CustomDraw
    {
        public ImageSource Image { get; set; }
        public Uri Texture { get; set; }
        public int TextureSize { get; set; }
        public Color Color { get; set; }
        public double CursorSize { get; set; }
        public double BrushSize { get; set; }

        private void DrawImage()
        {
            var dv = new DrawingVisual();
            var size = TextureSize;
            using (var conext = dv.RenderOpen())
            {
                //[关键]OpacityMask了解下？也许有童鞋想到的办法是，各种颜色的图片来一张？
                conext.PushOpacityMask(new ImageBrush(new BitmapImage(Texture)));
                //用颜色生成画笔画一个矩形
                conext.DrawRectangle(new SolidColorBrush(Color), null, new System.Windows.Rect(0, 0, size, size));
                conext.Close();
            }
            var rtb = new RenderTargetBitmap(size, size, 96d, 96d, PixelFormats.Pbgra32);
            rtb.Render(dv);
            var imageSource = BitmapFrame.Create(rtb);
            //[重要]此乃解决卡顿问题的关键！
            imageSource.Freeze();

            Image = imageSource;
        }

        public void Draw(DrawingContext drawingContext, StylusPointCollection stylusPoints)
        {
            if (stylusPoints.Count <= 1)
                return;

            if (Image == null)
                DrawImage();

            var p1 = new Point(double.NegativeInfinity, double.NegativeInfinity);
            var p2 = new Point(0, 0);
            var w1 = BrushSize + 20;

            for (int i = 0; i < stylusPoints.Count; i++)
            {
                p2 = (Point)stylusPoints[i];

                //两点相减得到一个向量[高中数学知识了解下？]
                var vector = p1 - p2;

                //得到 x、y的变化值
                var dx = (p2.X - p1.X) / vector.Length;
                var dy = (p2.Y - p1.Y) / vector.Length;

                var w2 = BrushSize;
                if (w1 - vector.Length > BrushSize)
                    w2 = w1 - vector.Length;

                //为啥又来一个for？图像重叠，实现笔画的连续性，感兴趣的童鞋可以把for取消掉看看效果
                for (int j = 0; j < vector.Length; j++)
                {
                    var x = p2.X;
                    var y = p2.Y;

                    if (!double.IsInfinity(p1.X) && !double.IsInfinity(p1.Y))
                    {
                        x = p1.X + dx;
                        y = p1.Y + dy;
                    }

                    //画图，没啥可说的
                    drawingContext.DrawImage(Image, new System.Windows.Rect(x - w2 / 2.0, y - w2 / 2.0, w2, w2));

                    //再把新的坐标赋值给p1，以序后来
                    p1 = new Point(x, y);

                    if (double.IsInfinity(vector.Length))
                        break;

                }
            }
        }
    }

    class CustomRenderer : DynamicRenderer
    {
        public CustomDraw Drawer { get; set; }

        protected override void OnDraw(DrawingContext drawingContext, StylusPointCollection stylusPoints, Geometry geometry, Brush fillBrush)
        {
            Drawer.Draw(drawingContext, stylusPoints);
        }
    }

    class CustomStroke : Stroke
    {
        public CustomDraw Drawer { get; set; }

        public CustomStroke(CustomDraw draw, StylusPointCollection stylusPoints) : base(stylusPoints)
        {
            Drawer = draw;
            DrawingAttributes.Color = Drawer.Color;
            DrawingAttributes.Width = Drawer.CursorSize;
        }

        public CustomStroke(CustomDraw draw, StylusPointCollection stylusPoints, DrawingAttributes drawingAttributes) : base(stylusPoints, drawingAttributes)
        {
            Drawer = draw;
            DrawingAttributes.Color = Drawer.Color;
            DrawingAttributes.Width = Drawer.CursorSize;
        }

        //卡顿就是该函数造成，每写完一笔就会调用，当笔画过长，后果可想而知~
        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            Drawer.Draw(drawingContext, StylusPoints);
        }
    }
}
