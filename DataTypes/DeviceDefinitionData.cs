using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebcDeviceSimulator.Data
{
    public class DeviceDefinitionData
    {
        private Data.DeviceSeparatorList _Seperators = new DeviceSeparatorList();
        public Data.DeviceSeparatorList Seperators
        {
            get { return _Seperators; }
            set { _Seperators = value; }
        }

        private Data.DeviceCommandDictionary _CommandDictionary = new DeviceCommandDictionary();
        public Data.DeviceCommandDictionary  CommandDictionary
        {
            get { return _CommandDictionary; }
            set { _CommandDictionary = value; }
        }

        private Data.CommandAnswers _ErrorAnswers = new CommandAnswers();
        public Data.CommandAnswers ErrorAnswers
        {
            get { return _ErrorAnswers; }
            set { _ErrorAnswers = value; }
        }

        private string _FileName = string.Empty;
        public string FileNameWithPath
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        private string _DeviceName = string.Empty;
        public string DeviceName
        {
            get { return _DeviceName; }
            set { _DeviceName = value; }
        }
    }
}
