using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebcDeviceSimulator.Activities
{
    public class SeparateDataBlock
    {
        private List<string> Seperators = new List<string>();
        private StringBuilder InputBuffer = new StringBuilder();
        public SeparateDataBlock()
        {
            InputBuffer.Clear();
            Seperators.Clear();
        }

        public void in_SetSeparator(List<string> b)
        {
            if (b == null) return; // Abbruch, wenn null übergeben wurde
            if (b.Count() == 0) return; // Abbruch, da kein Seperator im Array vorhanden            
            Seperators = b;
        }
        public void in_Data(string b)
        {
            if (b == null) return; // Abbruch, wenn null übergeben wurde

            // Prüfen, ob ein TrennzeichenTerm darin enthalten ist
            if (Seperators.Count == 0)
            {
                // Wenn nicht, dann sind noch keine Trennzeichen definiert.
                // In diesem Fall alle Zeichen einzeln zurückgeben
                foreach (char item in b)
                {
                    InputBuffer.Append(item);
                    out_SeparatedData(InputBuffer.ToString());
                    InputBuffer.Clear();
                }
            }
            else
            {
                // Trennzeichen bzw. TrennTerme sind vorhanden. Die Daten in entsprechende
                // Blöcke zusammenfassen.

                int idx = -1; // -1 = kein Trennzeichen gefunden
                // Empfangene Daten in den Puffer schreiben
                // Nach jedem Zeichen Prüfen, ob am Ende des Strings ein Seperator
                // erkannt wird.
                foreach (var item in b)
                {
                    InputBuffer.Append(item);
                    foreach (var sep in Seperators)
                    {
                        idx = InputBuffer.ToString().LastIndexOf(sep);
                        if (idx >= 0) // ein TrennzeichenTerm wurde gefunden
                        {
                            this.out_SeparatedData(InputBuffer.ToString());
                            InputBuffer.Clear();
                            idx = -1;
                        }
                    }
                }
            }
        }
        public event Action<string> out_SeparatedData;

    }
}
