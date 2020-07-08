using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ebc.Activities
{
    public class SyncWithGUI<T>
    {
        private readonly SynchronizationContext ctx;
        public SyncWithGUI() : this(SynchronizationContext.Current) { }
        internal SyncWithGUI(SynchronizationContext ctx)
        {
            this.ctx = ctx;
        }

        public void in_SwitchToGuiThread(T msg)
        {
            if (this.ctx != null)
                this.ctx.Send(x => this.out_SwitchedToGuiThread(msg), null);
            else
                this.out_SwitchedToGuiThread(msg);
        }
        public event Action<T> out_SwitchedToGuiThread;
    }
}
