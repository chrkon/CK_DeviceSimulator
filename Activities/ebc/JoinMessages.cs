using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebc.Activities
{
    public class JoinMessages<T1,T2>
    {
        private T1 MsgA = default(T1);
        private bool isSetMsgA = false;
        private T2 MsgB = default(T2);
        private bool isSetMsgB = false;

        public void in_MsgA(T1 Msg)
        {
            MsgA = Msg;
            isSetMsgA = true;
            MessageCompleteCheck();
        }
        public void in_MsgB(T2 Msg)
        {
            MsgB = Msg;
            isSetMsgB = true;
            MessageCompleteCheck();
        }
        public event Action<Tuple<T1, T2>> out_JoinedMessage;

        private void MessageCompleteCheck()
        {
            if ((isSetMsgA == true) && (isSetMsgB == true))
            {
                isSetMsgA = false;
                isSetMsgB = false;
                var Msg = new Tuple<T1, T2>(MsgA, MsgB);
                this.out_JoinedMessage(Msg);
            }
        }

    }
}
