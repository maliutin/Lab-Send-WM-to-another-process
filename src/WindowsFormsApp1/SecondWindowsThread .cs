using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class SecondWindowsThread : IDisposable
    {
        private SynchronizationContext ctx;
        private MyNativeWindow testWindow;

        public SecondWindowsThread(Action action)
        {
            ManualResetEvent mre = new ManualResetEvent(false);
            try
            {
                Thread thread = new Thread(
                    () =>
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        this.ctx = new WindowsFormsSynchronizationContext();
                        SynchronizationContext.SetSynchronizationContext(this.ctx);
                        mre.Set();
                        Application.Run();
                    }
                );
                thread.IsBackground = true;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                mre.WaitOne();

                this.ctx.Send(
                    (o) =>
                    {
                        this.testWindow = new MyNativeWindow(action);
                        this.testWindow.CreateWindow();
                    },
                    null);
            }
            finally
            {
                mre.Dispose();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        public void Dispose()
        {
            if (disposedValue) return;

            if (this.ctx != null)
            {
                this.ctx.Send(
                    (_) =>
                    {
                        this.testWindow?.DestroyWindow();
                        Application.ExitThread();
                    },
                    null);
                this.ctx = null;
            }
            disposedValue = true;
        }

        #endregion

        public SynchronizationContext SynchronizationContext
        {
            get
            {
                if (this.disposedValue) throw new ObjectDisposedException(nameof(SynchronizationContext));
                return this.ctx;
            }
        }
    }
}
