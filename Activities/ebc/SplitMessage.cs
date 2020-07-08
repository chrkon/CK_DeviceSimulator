using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebc.Activities
{
    public class SplitMessage<T>
    {
        public void in_SplitMessage(T Msg)
        {
            if (out_A != null) { this.out_A(Msg); }
            if (out_B != null) { this.out_B(Msg); }
        }
        public event Action<T> out_A;
        public event Action<T> out_B;
    }
}
