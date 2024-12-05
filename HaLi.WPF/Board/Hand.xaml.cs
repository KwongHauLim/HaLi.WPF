using HaLi.WPF.Helpers;
using HandyControl.Tools.Converter;
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

    public override void Export(string path)
    {
        Shape.Datas = uiCanvas.Export();
        base.Export(path);
    }

    public override void Import(JsonDocument json)
    {
        base.Import(json);
        uiCanvas.Import(Shape.Datas);
    }

    public void Clear()
    {
        uiCanvas.Clear();
    }
}
