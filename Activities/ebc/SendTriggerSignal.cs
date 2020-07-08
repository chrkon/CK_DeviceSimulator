using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebc.Activities
{
    public class SendTriggerSignal<T>
    {
        public void in_Message(T Msg)
        {
            if (out_Message != null) { out_Message(Msg); }
            if (out_TriggerSignal != null) { out_TriggerSignal(); }
        }

        public event Action<T> out_Message;

        public event Action out_TriggerSignal;

    }
}
