using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ebc.Activities
{
    public class ConvertControlBytes
    {
        private Dictionary<string, string> ClearTextDict = null;

        public ConvertControlBytes()
        {
            ClearTextDict = new Dictionary<string, string>(32);
            ClearTextDict.Add("\u0000", "[NUL]"); ClearTextDict.Add("\u0001", "[SOH]");
            ClearTextDict.Add("\u0002", "[STX]"); ClearTextDict.Add("\u0003", "[ETX]");
            ClearTextDict.Add("\u0004", "[EOT]"); ClearTextDict.Add("\u0005", "[ENQ]");
            ClearTextDict.Add("\u0006", "[ACK]"); ClearTextDict.Add("\u0007", "[BEL]");
            ClearTextDict.Add("\u0008", "[BS]");  ClearTextDict.Add("\u0009", "[TAB]");
            ClearTextDict.Add("\u000A", "[LF]");  ClearTextDict.Add("\u000B", "[VT]");
            ClearTextDict.Add("\u000C", "[FF]");  ClearTextDict.Add("\u000D", "[CR]");
            ClearTextDict.Add("\u000E", "[SO]");  ClearTextDict.Add("\u000F", "[SI]");
            ClearTextDict.Add("\u0010", "[DLE]"); ClearTextDict.Add("\u0011", "[DC1]");
            ClearTextDict.Add("\u0012", "[DC2]"); ClearTextDict.Add("\u0013", "[DC3]");
            ClearTextDict.Add("\u0014", "[DC4]"); ClearTextDict.Add("\u0015", "[NAK]");
            ClearTextDict.Add("\u0016", "[SYN]"); ClearTextDict.Add("\u0017", "[ETB]");
            ClearTextDict.Add("\u0018", "[CAN]"); ClearTextDict.Add("\u0019", "[EM]");
            ClearTextDict.Add("\u001A", "[SUB]"); ClearTextDict.Add("\u001B", "[ESC]");
            ClearTextDict.Add("\u001C", "[FS]");  ClearTextDict.Add("\u001D", "[GS]");
            ClearTextDict.Add("\u001E", "[RS]");  ClearTextDict.Add("\u001F", "[US]");
        }

        public void in_encodeString(string text)
        {
            string result = text;
            foreach (var item in ClearTextDict)
            {
                result = result.Replace(item.Key, item.Value);
            }
            if (out_encodedString != null) { this.out_encodedString(result); }
        }

        public event Action<string> out_encodedString;

        public void in_decodeString(string text)
        {
            string result = text;
            foreach (var item in ClearTextDict)
            {
                result = result.Replace(item.Value, item.Key);
            }
            if (out_decodedString != null) { this.out_decodedString(result); }
        }

        public event Action<string> out_decodedString;
    }
}
