using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Anti_Double_byte_Character
{
    public partial class Form1 : Form
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
        [DllImport("User32.dll")] static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("imm32.dll")] static extern IntPtr ImmGetDefaultIMEWnd(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool GetGUIThreadInfo(uint dwthreadid, ref GUITHREADINFO lpguithreadinfo);

        const int WM_IME_CONTROL = 0x283;
        const int IMC_GETCONVERSIONMODE = 1;
        const int IMC_SETCONVERSIONMODE = 2;
        const int IMC_GETOPENSTATUS = 5;
        const int IMC_SETOPENSTATUS = 6;

        const int IME_FULL_WIDTH = 8;
        const int IME_HALF_WIDTH = 0;

        private static bool ImeChangerEnable = true;
        private static bool RunEnable = true;
        private static int interval = 10;

        private static Thread thread = new Thread(ImeChanger);

        public Form1()
        {
            InitializeComponent();
            SetEneble(true);
            thread.Start();
        }
        private void choiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetEneble(!RunEnable);
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exit();
            Application.Exit();
        }
        public static void ImeChanger()
        {
            while (ImeChangerEnable)
            {
                if (RunEnable)
                {
                    var guiThreadInfo = new GUITHREADINFO();
                    guiThreadInfo.cbSize = Marshal.SizeOf(guiThreadInfo);

                    if (!GetGUIThreadInfo(0, ref guiThreadInfo))
                    {
                        Console.WriteLine("GetGUIThreadInfo failed");
                        Console.WriteLine(new Win32Exception());
                        continue;
                    }

                    var imwd = ImmGetDefaultIMEWnd(guiThreadInfo.hwndFocus);

                    var imeConvMode = SendMessage(imwd, WM_IME_CONTROL, (IntPtr)IMC_GETCONVERSIONMODE, IntPtr.Zero);
                    var imeEnabled = (SendMessage(imwd, WM_IME_CONTROL, (IntPtr)IMC_GETOPENSTATUS, IntPtr.Zero) != IME_HALF_WIDTH);

                    if (imeEnabled) if (imeConvMode == IME_FULL_WIDTH)
                            SendMessage(imwd, WM_IME_CONTROL, (IntPtr)IMC_SETCONVERSIONMODE, (IntPtr)IME_HALF_WIDTH);
                }
                Thread.Sleep(interval);
            }
        }
        private void SetEneble(bool e)
        {
            RunEnable = e;
            choiceToolStripMenuItem.Text = e ? "Disabled" : "Enabled";
        }
        private void exit()
        {
            ImeChangerEnable = false;
            RunEnable = false;
        }
    }
}