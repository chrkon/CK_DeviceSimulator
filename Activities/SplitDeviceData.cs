using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebcDeviceSimulator.Activities
{
    public class SplitDeviceData
    {
        public event Action<Data.CommandAnswers> out_DeviceError;
        public event Action<Data.DeviceSeparatorList> out_Seperators;
        public event Action<Data.DeviceCommandDictionary> out_DeviceCommandDictionary;
        public event Action<Data.DeviceDefinitionData> out_DeviceDefinitionData;

        public void in_DeviceDefinitionData(Data.DeviceDefinitionData DefData)
        {
            if (out_Seperators != null) 
            {
                out_Seperators(DefData.Seperators); 
            }

            if (out_DeviceError != null)
            {
                out_DeviceError(DefData.ErrorAnswers);
            }

            if (out_DeviceCommandDictionary != null) 
            {
                out_DeviceCommandDictionary(DefData.CommandDictionary); 
            }

            if (out_DeviceDefinitionData != null)
            {
                out_DeviceDefinitionData(DefData);
            }

        }

    }
}
