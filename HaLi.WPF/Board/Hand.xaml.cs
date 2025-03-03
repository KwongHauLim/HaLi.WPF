using HaLi.WPF.Helpers;
using HandyControl.Tools.Converter;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace HaLi.WPF.Board;

/// <summary>
/// Interaction logic for Hand.xaml
/// </summary>
[EditorBrowsable(EditorBrowsableState.Always)]
public partial class Hand : HandBase
{
    public Hand()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
        base.OnLoaded(sender, e);

        uiCanvas.SetBinding(WidthProperty, GuiHelper.OneWay(this, "ActualWidth"));
        uiCanvas.SetBinding(HeightProperty, GuiHelper.OneWay(this, "ActualHeight"));
    }

    protected override void UpdateGUI()
    {
        if (!IsInitialized || System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            return;

        uiCanvas.Drawer.Color = Color;
        uiCanvas.Drawer.CursorSize = CursorSize;
        uiCanvas.Drawer.BrushSize = BrushSize;
        uiCanvas.UpdateGUI();

        if (OneStyle)
        {
            uiCanvas.ModifyStrokes(); 
        }

        base.UpdateGUI();
    }

    public override void StopEdit()
    {
        base.StopEdit();
        uiCanvas.IsHitTestVisible = false;
    }

    public override JObject Export()
    {
        Shape.Datas = uiCanvas.Export();
        return base.Export();
    }

    public override void Import(JToken json)
    {
        base.Import(json);
        uiCanvas.Import(Shape.Datas);
    }

    public void Clear()
    {
        uiCanvas.Clear();
    }
}
