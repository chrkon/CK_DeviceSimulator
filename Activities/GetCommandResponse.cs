using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebcDeviceSimulator.Activities
{
    public class GetCommandResponse
    {
        private bool isDeviceDefinitionAvailable = false;
        private Data.CommandAnswers ErrorAnswers = null;
        private Data.DeviceCommandDictionary CommandDict = null;
        private Data.DeviceSeparatorList Seperators = null;

        public void in_DeviceCommandsDefinition(Data.DeviceDefinitionData DeviceDefinitionData)
        {
            this.ErrorAnswers = DeviceDefinitionData.ErrorAnswers;
            this.Seperators = DeviceDefinitionData.Seperators;
            this.CommandDict = DeviceDefinitionData.CommandDictionary;
            this.isDeviceDefinitionAvailable = true;
        }

        public void in_GetCommandResponseList(string Command)
        {
            if (this.isDeviceDefinitionAvailable)
            {
                // Antwort im Datenbestand suchen
                bool isElementFound = false;
                isElementFound = this.CommandDict.ContainsKey(Command);
                if (isElementFound)
                {
                    out_CommandResponse(this.CommandDict[Command]);
                }
                else
                {
                    // Errormeldung im Datenbestand suchen und senden
                    List<Data.CommandAnswers> ErrList = new List<Data.CommandAnswers>();
                    ErrList.Add(this.ErrorAnswers);
                    out_CommandResponse(ErrList);
                    ErrList.Clear();
                }
            }
            else
            {
                throw new InvalidOperationException("No Device Definition found!");
            }
        }

        public event Action<List<Data.CommandAnswers>> out_CommandResponse;
    }
}
