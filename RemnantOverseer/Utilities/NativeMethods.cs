using System;
using System.Runtime.InteropServices;

namespace RemnantOverseer.Utilities;
internal class NativeMethods
{
    public const int HWND_BROADCAST = 0xffff;
    public static readonly int RO_WM_SHOWME = RegisterWindowMessage("RO_WM_SHOWME");
    [DllImport("user32")]
    public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
    [DllImport("user32")]
    public static extern int RegisterWindowMessage(string message);
}