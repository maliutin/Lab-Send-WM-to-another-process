using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class MyNativeWindow : NativeWindow
    {
        private const int WmClose = 0x0010;

        [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        private static readonly HandleRef HwndMessage = new HandleRef(null, new IntPtr(-3));

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        private static extern IntPtr PostMessage(HandleRef hwnd, int msg, int wparam, int lparam);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.Process)]
        private static extern int GetWindowThreadProcessId(HandleRef hWnd, out int lpdwProcessId);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.Process)]
        private static extern int GetCurrentThreadId();

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern uint RegisterWindowMessage(string message);

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private readonly uint UWM_HELLO;
        private readonly Action Action;

        public MyNativeWindow(Action action)
        {
            Action = action;
            UWM_HELLO = RegisterWindowMessage("TEST MESSAGE");
        }

        public bool CreateWindow()
        {
            if (Handle == IntPtr.Zero)
            {
                var cp = new CreateParams
                {
                    //Style = 0,
                    //ExStyle = 0,
                    //ClassStyle = 0,
                    Caption = this.GetType().FullName,
                    // Parent = (IntPtr)HwndMessage
                };

                //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                //{
                //    cp.Parent = (IntPtr)HwndMessage;
                //}

                CreateHandle(cp);
            }
            return Handle != IntPtr.Zero;
        }

        public void DestroyWindow()
        {
            DestroyWindow(true, IntPtr.Zero);
        }

        private bool GetInvokeRequired(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero) return false;
            int pid;
            var hwndThread = GetWindowThreadProcessId(new HandleRef(this, hWnd), out pid);
            var currentThread = GetCurrentThreadId();
            return (hwndThread != currentThread);
        }

        private void DestroyWindow(bool destroyHwnd, IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
            {
                hWnd = Handle;
            }

            if (GetInvokeRequired(hWnd))
            {
                PostMessage(new HandleRef(this, hWnd), WmClose, 0, 0);
                return;
            }

            lock (this)
            {
                if (destroyHwnd)
                {
                    base.DestroyHandle();
                }
            }
        }

        public override void DestroyHandle()
        {
            DestroyWindow(false, IntPtr.Zero);
            base.DestroyHandle();
        }

        protected override void WndProc(ref Message m)
        {
            if(m.Msg == UWM_HELLO)
            {
                Action?.Invoke();
                return;
            }

            base.WndProc(ref m);
        }
    }
}
