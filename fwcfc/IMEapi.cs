using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace fwcfc
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GUITHREADINFO
    {
        public int cbSize;
        public int flags;
        public IntPtr hwndActive;
        public IntPtr hwndFocus;
        public IntPtr hwndCapture;
        public IntPtr hwndMenuOwner;
        public IntPtr hwndMoveSize;
        public IntPtr hwndCaret;
        public System.Drawing.Rectangle rcCaret;
    }

    public static class IMEapi
    {
        [DllImport("User32.dll")] public static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("imm32.dll")] public static extern IntPtr ImmGetDefaultIMEWnd(IntPtr hWnd);
        [DllImport("user32.dll")] public static extern bool GetGUIThreadInfo(uint dwthreadid, ref GUITHREADINFO lpguithreadinfo);

        public const int WM_IME_CONTROL = 0x283;
        public const int IMC_GETCONVERSIONMODE = 1;
        public const int IMC_SETCONVERSIONMODE = 2;
        public const int IMC_GETOPENSTATUS = 5;
        public const int IMC_SETOPENSTATUS = 6;

        public const int IME_FULL_WIDTH = 8;
        public const int IME_HALF_WIDTH = 0;
    }
}
