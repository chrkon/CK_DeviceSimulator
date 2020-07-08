using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebc.Activities
{
    public enum SwitchPath
    {
        SwitchToA,
        SwitchToB
    }

    public class Switch<T>
    {
        private SwitchPath thePath = SwitchPath.SwitchToA;

        public void in_SetSwitch(SwitchPath Path)
        {
            thePath = Path;
        }
        public void in_Message(T Msg)
        {
            switch (thePath)
            {
                case SwitchPath.SwitchToA:
                    out_MessageToA(Msg);
                    break;
                case SwitchPath.SwitchToB:
                    out_MessageToB(Msg);
                    break;
                default:
                    out_MessageToA(Msg);
                    break;
            }
        }
        public event Action<T> out_MessageToA;
        public event Action<T> out_MessageToB;
    }
}
