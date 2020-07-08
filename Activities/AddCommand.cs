using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebcDeviceSimulator.Activities
{
    public class AddCommand
    {
        private Data.DeviceDefinitionData DDD = null;
        private string activeCmd = string.Empty;
        private Data.CommandAnswers CmdAns = null;

        public void in_DeviceDefinitionData(Data.DeviceDefinitionData DevDefinitionData)
        {
            DDD = DevDefinitionData;
        }

        public event Action<Data.DeviceDefinitionData> out_NewDeviceDefinitionData;

        public void in_Command(string Cmd)
        {
            // neues Command empfangen
            // damit ist das vorherige Command komplett abgearbeitet!
            // also den "CommandComplete" Trigger aufrufen
            this.in_CommandComplete();

            // Erst ab hier das neue Command verarbeiten
            activeCmd = Cmd;
            CmdAns = new Data.CommandAnswers();

        }

        public void in_CommandComplete()
        {
            // Wenn Daten vorhanden sind, diese dem DDD hinzufügen
            if ((this.activeCmd != string.Empty) && (this.CmdAns != null))
            {
                this.AppendData(this.activeCmd, this.CmdAns);
            }
        }

        public void in_Answer(Tuple<TimeSpan,string> Answer)
        {
            var DeviceAnswer = new Data.CommandDataBlock(Answer.Item1,Answer.Item2);

            // Prüfen, ob die Antwort gültig ist
            if (DDD.ErrorAnswers.Contains(DeviceAnswer))
            {
                // Antwort entspricht einer Error Antwort = Command nicht merken = keine Aktion
                return;
            }

            // Wenn es bereits eine CommandAnswer Liste gibt, dann DatenBlock darin eintragen
            if (CmdAns != null)
            {
                CmdAns.Add(DeviceAnswer);
            }
        }


        private void AppendData(string Cmd, Data.CommandAnswers CmdAns)
        {
            if (this.DDD.CommandDictionary.Keys.Contains(Cmd) == true)
            {
                // es exitiert bereits ein Eintrag zu diesem Command
                // also die Daten hinzufügen
                this.DDD.CommandDictionary[Cmd].Add(CmdAns);
            }
            else
            {
                // einen neuen Eintrag erstellen. Dazu eine neue Liste anlegen
                List<Data.CommandAnswers> newList = new List<Data.CommandAnswers>();
                newList.Add(CmdAns);
                this.DDD.CommandDictionary.Add(Cmd, newList);
            }

            // Die neuen Daten ausgeben
            if (this.out_NewDeviceDefinitionData != null)
            {
                this.out_NewDeviceDefinitionData(DDD);
            }
        }
    }
}
