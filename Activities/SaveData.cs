using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ebcDeviceSimulator.Activities
{
    public class SaveData
    {
        public void in_SaveData(Data.DeviceDefinitionData DDD)
        {
            // Wenn ein Dateiname bekannt ist, dann Struktur speichern
            if (DDD.FileNameWithPath != string.Empty)
            {
                try
                {
                    UTF8Encoding enc = new UTF8Encoding();
                    XElement Answer = null;
                    XElement Out = null;

                    // Trennzeichen einfügen
                    XElement Seperators = new XElement("Seperators");
                    foreach (var item in DDD.Seperators)
                    {
                        Seperators.Add(new XElement("Seperator", item));
                    }

                    // Error result einfügen
                    XElement Error = new XElement("ErrorAnswer");
                    foreach (var elem in DDD.ErrorAnswers)
                    {
                        Out = new XElement("Out", elem.Item2);
                        Out.Add(new XAttribute("Delay", Math.Round(elem.Item1.TotalMilliseconds)));
                        Error.Add(Out);
                    }

                    // Commands einfügen
                    XElement Commands = new XElement("Commands");
                    XElement Command = null;
                    foreach (var CMD in DDD.CommandDictionary)
                    {
                        Command = new XElement("Command",
                            new XElement("In", CMD.Key));

                        foreach (var AnswerList in CMD.Value)
                        {
                            Answer = new XElement("Answer");
                            foreach (var elem in AnswerList)
                            {
                                Out = new XElement("Out", new XAttribute("Delay", Math.Round(elem.Item1.TotalMilliseconds)));
                                Out.Add(elem.Item2);
                                Answer.Add(Out);
                            }
                            Command.Add(Answer);
                        }
                        Commands.Add(Command);
                    }

                    XElement DeviceDefinition =
                        new XElement("Device",
                            new XElement("Name", DDD.DeviceName),
                            Seperators,
                            Error,
                            Commands);

                    XDocument doc = new XDocument(
                        new XDeclaration("1.0", "utf-8", "yes"),
                        new XComment("  RS232 Device Simulator Definition File  "),
                        new XComment("  (C) Christof Konstantinopoulos, September 2010  "),
                        new XComment("-"),
                        new XComment("  Control bytes have be encoded into default string terms:  "),
                        new XComment("  Examples : ASCII $0D = [CR], ASCII $06 = [ACK]  "),
                        new XComment("  see http://en.wikipedia.org/wiki/ASCII for more details  "),
                        DeviceDefinition
                        );

                    doc.Save(DDD.FileNameWithPath);
                }
                catch (Exception ex)
                {
                    if (out_Exception != null)
                    {
                        var E = new Exception("SaveData Action failed (" + DDD.FileNameWithPath + ")" + Environment.NewLine + ex.Message, ex);
                        this.out_Exception(E);
                    }
                }
            }
        }

        public event Action<Exception> out_Exception;
    }
}
