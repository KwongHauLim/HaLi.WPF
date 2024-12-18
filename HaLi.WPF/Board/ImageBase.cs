using HaLi.WPF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaLi.WPF.Board;

public class ImageBase : DrawOnCanvas<Shapes.Image>
{
}

public class ImageEdit : EditBase
{
    public ImageEdit()
    {
        var m = new EditMouse.EditMonitor();
        m.WhenPressed = true;
        m.On += When;
        Mouse.Monitors.Add(m);
    }

    private void When(object? sender, EditMouse.MouseArgs e)
    {
        if (e.Event == EditMouse.MouseEvent.Down)
        {
            var img = new Image();
            img.X = Mouse.Position.X;
            img.Y = Mouse.Position.Y;
            Helper.CopyProperties(img.Shape, img);
            SetEdit(img);
            Board.StopEdit();
        }
    }
}