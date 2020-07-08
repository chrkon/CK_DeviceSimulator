using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebcDeviceSimulator.Activities
{
    public class Counter<T> 
    {
        private static Dictionary<T, int> CounterDict = new Dictionary<T, int>();

        public void in_Reset(T Msg)
        {
            if (Msg == null)
            {
                // alle Countereinträge löschen
                CounterDict.Clear();
            }
            else
            {
                // nur den angegebenen Counter zurücksetzen
                if (CounterDict.ContainsKey(Msg)) { CounterDict[Msg] = -1; }
            }
        }
        public void in_Count(T Msg)
        {
            // Counter hochzählen
            if (CounterDict.ContainsKey(Msg)) { CounterDict[Msg]++; }
            else { CounterDict.Add(Msg, 0); }
            out_CountIndex(CounterDict[Msg]);
        }
        public event Action<int> out_CountIndex;
    }
}
