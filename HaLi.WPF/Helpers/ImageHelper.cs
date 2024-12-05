using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HaLi.WPF.Helpers;

public static class ImageHelper
{
    public static void Save(ImageSource source, string path)
    {
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(source as BitmapSource));
        using var stream = System.IO.File.Create(path);
        encoder.Save(stream);
    }
}
