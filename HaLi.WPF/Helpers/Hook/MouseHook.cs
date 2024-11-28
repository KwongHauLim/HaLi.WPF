using HaLi.WPF.Helpers.Interop;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HaLi.WPF.Helpers.Hook;

internal class MouseHookEventArgs : EventArgs
{
    public MouseHookMessageType MessageType { get; set; }

    public InteropValues.POINT Point { get; set; }
}

public enum MouseHookMessageType
{
    LeftButtonDown = 0x0201,
    LeftButtonUp = 0x0202,
    MouseMove = 0x0200,
    MouseWheel = 0x020A,
    RightButtonDown = 0x0204,
    RightButtonUp = 0x0205
}

internal class MouseHook
{
    public static event EventHandler<MouseHookEventArgs> StatusChanged;

    private static IntPtr HookId = IntPtr.Zero;

    private static readonly InteropValues.HookProc Proc = HookCallback;

    private static int Count;

    public static bool InHook { get; private set; }
    public static InteropValues.POINT MousePoint { get; private set; }

    public static void Start()
    {
        if (HookId == IntPtr.Zero)
        {
            HookId = SetHook(Proc);
        }

        if (HookId != IntPtr.Zero)
        {
            Count++;
        }
    }

    public static void Stop()
    {
        Count--;
        if (Count < 1)
        {
            InteropMethods.UnhookWindowsHookEx(HookId);
            HookId = IntPtr.Zero;
            InHook = false;
        }
    }

    private static IntPtr SetHook(InteropValues.HookProc proc)
    {
        using var curProcess = Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule;

        if (curModule != null)
        {
            InHook = true;
            return InteropMethods.SetWindowsHookEx((int)InteropValues.HookType.WH_MOUSE_LL, proc,
                InteropMethods.GetModuleHandle(curModule.ModuleName), 0);
        }
        return IntPtr.Zero;
    }

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode < 0) return InteropMethods.CallNextHookEx(HookId, nCode, wParam, lParam);
        var hookStruct = (InteropValues.MOUSEHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(InteropValues.MOUSEHOOKSTRUCT));
        MousePoint = new InteropValues.POINT(hookStruct.pt.X, hookStruct.pt.Y);
        StatusChanged?.Invoke(null, new MouseHookEventArgs
        {
            MessageType = (MouseHookMessageType)wParam,
            Point = MousePoint
        });

        return InteropMethods.CallNextHookEx(HookId, nCode, wParam, lParam);
    }
}
