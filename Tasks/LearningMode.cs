using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebcDeviceSimulator.Tasks
{
    public class LearningMode
    {
        // Vervendete Activities definieren (Reihenfolge entspricht der Zeichnung)
        private ebc.Activities.SplitMessage<string> splitData_A = null;
        private ebc.Activities.ConvertControlBytes CCB_A = null;
        private Activities.SeparateDataBlock separateCommand = null;
        private ebc.Activities.SendTriggerSignal<string> CommandTrigger = null;

        private Activities.SplitDeviceData splitDevData = null;
        private Activities.AddCommand AppendCommand = null;

        private ebc.Activities.JoinMessages<TimeSpan,string> DataBlock = null;
        private ebc.Activities.TimeSpanBetweenSignals Delay = null;
        private ebc.Activities.SendTriggerSignal<string> AnswerTrigger = null;
        private Activities.SeparateDataBlock separateAnswer = null;
        private ebc.Activities.ConvertControlBytes CCB_B = null;
        private ebc.Activities.SplitMessage<string> splitData_B = null;
        
        public LearningMode()
        {
            // Activities instantieren
            splitData_A = new ebc.Activities.SplitMessage<string>();
            CCB_A = new ebc.Activities.ConvertControlBytes();
            separateCommand = new Activities.SeparateDataBlock();
            CommandTrigger = new ebc.Activities.SendTriggerSignal<string>();

            splitDevData = new Activities.SplitDeviceData();
            AppendCommand = new Activities.AddCommand();

            DataBlock = new ebc.Activities.JoinMessages<TimeSpan,string>();
            Delay = new ebc.Activities.TimeSpanBetweenSignals();
            AnswerTrigger = new ebc.Activities.SendTriggerSignal<string>();
            separateAnswer = new Activities.SeparateDataBlock();
            CCB_B = new ebc.Activities.ConvertControlBytes();
            splitData_B = new ebc.Activities.SplitMessage<string>();

            // Initialisierung (gestrichelte Linien in der Zeichnung)
            this.DeviceDefinitionDataReceived += splitDevData.in_DeviceDefinitionData;
            splitDevData.out_Seperators += separateCommand.in_SetSeparator;
            splitDevData.out_Seperators += separateAnswer.in_SetSeparator;
            splitDevData.out_DeviceDefinitionData += AppendCommand.in_DeviceDefinitionData;

            // Learning (durchgehende Linien in der Zeichnung)
            this.DataFromPC += splitData_A.in_SplitMessage;
            splitData_A.out_A += this.out_DataToDevice;
            splitData_A.out_B += CCB_A.in_encodeString;
            CCB_A.out_encodedString += separateCommand.in_Data;
            separateCommand.out_SeparatedData += CommandTrigger.in_Message;
            CommandTrigger.out_TriggerSignal += Delay.in_TiggerSignal;
            CommandTrigger.out_Message += AppendCommand.in_Command;

            AppendCommand.out_NewDeviceDefinitionData += this.NewDeviceDataDef;
            CommandCompleted += AppendCommand.in_CommandComplete;

            // Diesen Block bitte von unten nach oben lesen
            DataBlock.out_JoinedMessage += AppendCommand.in_Answer;
            Delay.out_TimeSpan += DataBlock.in_MsgA;
            AnswerTrigger.out_Message += DataBlock.in_MsgB;
            AnswerTrigger.out_TriggerSignal += Delay.in_TiggerSignal;
            separateAnswer.out_SeparatedData += AnswerTrigger.in_Message;
            CCB_B.out_encodedString += separateAnswer.in_Data;
            splitData_B.out_B += CCB_B.in_encodeString;
            splitData_B.out_A += this.out_DataToPC;
            this.DataFromDevice += splitData_B.in_SplitMessage;
            // Leserichtung von unten nach oben entspricht der Anordnung in der Zeichnung
        }

        // In / Out Ports definieren
        private event Action<string> DataFromPC;
        public void in_DataFromPC(string data)
        {
            DataFromPC(data);
        }

        public event Action<string> out_DataToDevice;

        private event Action<string> DataFromDevice;
        public void in_DataFromDevice(string data)
        {
            DataFromDevice(data);
        }

        public event Action<string> out_DataToPC;

        private event Action<Data.DeviceDefinitionData> DeviceDefinitionDataReceived;
        public void in_DeviceDefinitionData(Data.DeviceDefinitionData DevData)
        {
           this.DeviceDefinitionDataReceived(DevData);
        }

        public event Action<Data.DeviceDefinitionData> out_NewDeviceDefinition;
        private void NewDeviceDataDef(Data.DeviceDefinitionData DDD)
        {
            if (out_NewDeviceDefinition != null) { out_NewDeviceDefinition(DDD); }
        }

        private event Action CommandCompleted;
        public void in_CommandCompleted()
        {
            this.CommandCompleted();
        }

    }
}
