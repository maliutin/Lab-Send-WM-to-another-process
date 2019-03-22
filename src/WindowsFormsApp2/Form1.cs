using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public int HWND_BROADCAST = 0xffff;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern uint RegisterWindowMessage(string message);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SendNotifyMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        private uint UWM_HELLO;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            UWM_HELLO = RegisterWindowMessage("TEST MESSAGE");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendNotifyMessage((IntPtr)HWND_BROADCAST, UWM_HELLO, 0, 0);
        }
    }
}
