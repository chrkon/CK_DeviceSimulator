using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebcDeviceSimulator.Data
{
    public class CommandDataBlock : Tuple<TimeSpan,string>
    {
        public CommandDataBlock()
            : base(TimeSpan.MinValue, string.Empty)
        {
        }

        public CommandDataBlock(TimeSpan Delay, string Data)
            : base(Delay, Data)
        {
        }
    }
}
