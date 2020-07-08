using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebc.Activities
{
    public class TimeSpanBetweenSignals
    {
        private DateTime LastSignal = DateTime.MaxValue;

        public void in_TiggerSignal()
        {               
            if (LastSignal < DateTime.MaxValue)
            {
                TimeSpan TS = DateTime.Now - LastSignal;
                if (out_TimeSpan != null) { out_TimeSpan(TS); }
            }

            LastSignal = DateTime.Now;
        }

        public event Action<TimeSpan> out_TimeSpan;
    }
}
