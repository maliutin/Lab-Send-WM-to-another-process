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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        [DllImport("user32")]
        private static extern uint RegisterWindowMessage(string message);

        MyNativeWindow nativeWindow;
        private SecondWindowsThread secondWindowsThread;
       
        private uint UWM_HELLO;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UWM_HELLO = RegisterWindowMessage("TEST MESSAGE");

            this.nativeWindow = new MyNativeWindow(
                ()=>
                {
                    WriteHelloNW();
                }
            );
            this.nativeWindow.CreateWindow();

            this.secondWindowsThread = new SecondWindowsThread(
                () =>
                {
                    Action action = this.WriteHelloFromSTA;
                    this.BeginInvoke(action);
                });
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            this.nativeWindow.DestroyWindow();
            this.secondWindowsThread?.Dispose();
        }

        public void WriteHelloFromSTA()
        {
            var text = this.textBox1.Text;

            this.textBox1.Text += (text == "" ? "" : "\r\n") + "Hello from STA!";
        }

        public void WriteHelloFromForm()
        {
            var text = this.textBox1.Text;

            this.textBox1.Text += (text == "" ? "" : "\r\n") + "Hello from Form!";
        }

        public void WriteHelloNW()
        {
            var text = this.textBox1.Text;

            this.textBox1.Text += (text == "" ? "" : "\r\n") + "Hello from NW!";
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == UWM_HELLO)
                WriteHelloFromForm();

            base.WndProc(ref m);
        }
    }
}
