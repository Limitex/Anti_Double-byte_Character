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

namespace fwcfc
{
    public partial class MainForm : Form
    {
        private bool flug = true;

        private ToolStripMenuItem toolStripMenuItem_switch;
        public MainForm()
        {
            InitializeComponent();

            ToolStripMenuItem toolStripMenuItem_exit = new ToolStripMenuItem();
            toolStripMenuItem_exit.Text = "EXIT";
            toolStripMenuItem_exit.Click += ToolStripMenuItem_exit_Click;

            toolStripMenuItem_switch = new ToolStripMenuItem();
            toolStripMenuItem_switch.Text = "Enable";
            toolStripMenuItem_switch.Click += ToolStripMenuItem_switch_Click;
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(toolStripMenuItem_exit);
            contextMenuStrip.Items.Add(toolStripMenuItem_switch);
            notifyIcon.ContextMenuStrip = contextMenuStrip;

            Task.Run(() => 
            { 
                while (true) 
                { 
                    if (flug) Execute(); 
                    Thread.Sleep(100); 
                } 
            });
        }

        private void ToolStripMenuItem_exit_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ToolStripMenuItem_switch_Click(object? sender, EventArgs e)
        {
            if (flug)
            {
                toolStripMenuItem_switch.Text = "Enable";
                flug = false;
            }
            else
            {
                toolStripMenuItem_switch.Text = "Disable";
                flug = true;
            }
        }

        private void Execute()
        {
            var guiThreadInfo = new GUITHREADINFO();
            guiThreadInfo.cbSize = Marshal.SizeOf(guiThreadInfo);

            if (!IMEapi.GetGUIThreadInfo(0, ref guiThreadInfo)) return;

            var imwd = IMEapi.ImmGetDefaultIMEWnd(guiThreadInfo.hwndFocus);

            var imeConvMode = IMEapi.SendMessage(imwd, IMEapi.WM_IME_CONTROL, (IntPtr)IMEapi.IMC_GETCONVERSIONMODE, IntPtr.Zero);
            var imeEnabled = IMEapi.SendMessage(imwd, IMEapi.WM_IME_CONTROL, (IntPtr)IMEapi.IMC_GETOPENSTATUS, IntPtr.Zero);

            if (imeEnabled != IMEapi.IME_HALF_WIDTH && imeConvMode == IMEapi.IME_FULL_WIDTH)
                IMEapi.SendMessage(imwd, IMEapi.WM_IME_CONTROL, (IntPtr)IMEapi.IMC_SETCONVERSIONMODE, (IntPtr)IMEapi.IME_HALF_WIDTH);
        }
    }
}