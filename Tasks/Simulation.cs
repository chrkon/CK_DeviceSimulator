using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebcDeviceSimulator.Tasks
{
    public class Simulation
    {
        // Vervendete Activities definieren
        private Activities.SplitDeviceData splitDevData = null;
        private ebc.Activities.ConvertControlBytes CCB = null;
        private Activities.SeparateDataBlock separateCommand = null;
        private ebc.Activities.Asynchronizer<string> async = null;
        private ebc.Activities.SplitMessage<string> splitData = null;
        private Activities.Counter<string> count = null;
        private ebcDeviceSimulator.Activities.GetCommandResponse GetCmdResponse = null;
        private ebc.Activities.SelectItem<Data.CommandAnswers> selectAnswer = null;
        private Activities.SendAnswers sendAnswer = null;

        public Simulation()
        {
            // Activities instantieren
            splitDevData = new Activities.SplitDeviceData();
            CCB = new ebc.Activities.ConvertControlBytes();
            separateCommand = new Activities.SeparateDataBlock();
            async = new ebc.Activities.Asynchronizer<string>();
            splitData = new ebc.Activities.SplitMessage<string>();
            count = new Activities.Counter<string>();
            GetCmdResponse = new Activities.GetCommandResponse();
            selectAnswer = new ebc.Activities.SelectItem<Data.CommandAnswers>();
            sendAnswer = new Activities.SendAnswers();

            // Verdrahtung der Activities (Initialisierung)
            this.SetDeviceDefinition += splitDevData.in_DeviceDefinitionData;
            splitDevData.out_Seperators += separateCommand.in_SetSeparator;
            splitDevData.out_DeviceDefinitionData += GetCmdResponse.in_DeviceCommandsDefinition;

            // Verdrahtung der Activities (Simulation)
            this.receiveData += CCB.in_encodeString;
            CCB.out_encodedString += separateCommand.in_Data;
            separateCommand.out_SeparatedData += async.in_ProcessAsync;
            async.out_AsyncProcessing += splitData.in_SplitMessage;
            splitData.out_A += count.in_Count;
            count.out_CountIndex += selectAnswer.in_Index;
            splitData.out_B += GetCmdResponse.in_GetCommandResponseList;
            GetCmdResponse.out_CommandResponse += selectAnswer.in_Elements;
            selectAnswer.out_selectedItem += sendAnswer.in_AnswerList;
            sendAnswer.out_AnswerString += CCB.in_decodeString;
            CCB.out_decodedString += this.simulatedAnswer;
        }

        private event Action<Data.DeviceDefinitionData> SetDeviceDefinition;
        public void in_DeviceDefinition(Data.DeviceDefinitionData DevDefinitionData)
        {
            this.SetDeviceDefinition(DevDefinitionData);
        }

        private event Action<string> receiveData;
        public void in_receivedData(string data)
        {
            this.receiveData(data);
        }

        private void simulatedAnswer(string Data)
        {
            if (out_simulatedAnswer != null)
            {
                out_simulatedAnswer(Data);
            }
        }
        public event Action<string> out_simulatedAnswer;

    }
}
