using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ebc.Activities
{
    public class Asynchronizer<T>
    {
        public void in_ProcessAsync(T msg)
        {
            ThreadPool.QueueUserWorkItem(x => {
                try {
	                this.out_AsyncProcessing(msg);
                }
                catch (Exception ex) {
                    if (this.out_ExceptionReceived != null)
                    {
                        this.out_ExceptionReceived(ex);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                }
            });
        }
        public event Action<T> out_AsyncProcessing;
        public event Action<Exception> out_ExceptionReceived;
    }
}
