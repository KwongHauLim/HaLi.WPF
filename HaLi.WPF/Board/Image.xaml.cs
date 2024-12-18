using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HaLi.WPF.Board;

/// <summary>
/// Interaction logic for Image.xaml
/// </summary>
public partial class Image : ImageBase
{
    public Image()
    {
        InitializeComponent();
    }

    protected override void UpdateGUI()
    {
        if (!IsInitialized || System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            return;

        var b64 = Shape.Data;
        uiImage.Source = Base64ToBitmapSource(b64);

        base.UpdateGUI();
    }

    public BitmapSource? Base64ToBitmapSource(string base64String)
    {
        if (string.IsNullOrEmpty(base64String))
            return null;

        byte[] binaryData = Convert.FromBase64String(base64String);
        using (MemoryStream ms = new MemoryStream(binaryData))
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }
    }

    public override void StopEdit()
    {
        base.StopEdit();
        uiCanvas.IsHitTestVisible = false;
    }
}
