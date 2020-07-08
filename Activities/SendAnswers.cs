using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebcDeviceSimulator.Activities
{
    public class SendAnswers
    {

        public void in_AnswerList(Data.CommandAnswers Answers)
        {
            // Wenn Antwort nicht vorhanden (oder leer), dann Abbruch
            if (Answers == null) { return; }
            if (Answers.Count == 0) { return; }

            // Wenn eine Antwort gefunden wurde, dann einzelne Blöcke senden
            int WaitTimeInMillisec = 0;
            string result = string.Empty;

            foreach (var Element in Answers)
            {
                WaitTimeInMillisec = Convert.ToInt32(Element.Item1.TotalMilliseconds);
                result = Element.Item2.ToString();
                System.Threading.Thread.Sleep(WaitTimeInMillisec);  // angegebene Zeit warten
                out_AnswerString(result); // Antwort senden
            }

        }

        public event Action<string> out_AnswerString;

    }
}
