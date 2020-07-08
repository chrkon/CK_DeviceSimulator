using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ebcDeviceSimulator.Activities
{
    public class LoadData
    {        
        Data.DeviceDefinitionData DevDefinitionData = new Data.DeviceDefinitionData();
            
        public void in_LoadData(string PathAndFileName)
        {
            if (PathAndFileName != string.Empty)
            {
                // XML Dokument laden
                XDocument doc = XDocument.Load(PathAndFileName);

                this.DevDefinitionData.FileNameWithPath = PathAndFileName;

                // Device Name auslesen
                XElement DevName = doc.Element("Device").Element("Name");
                if (DevName != null)
                {
                    this.DevDefinitionData.DeviceName = (string)DevName;
                }

                // Error Answer auslesen
                XElement Out = doc.Element("Device").Element("ErrorAnswer").Element("Out");
                if (Out != null)
                {
                    this.DevDefinitionData.ErrorAnswers.Clear();
                    string Delay = (string)Out.Attribute("Delay");
                    TimeSpan TS = new TimeSpan(0, 0, 0, 0, XmlConvert.ToInt32(Delay));
                    string Term = (string)Out.Value;
                    this.DevDefinitionData.ErrorAnswers.Add(new Data.CommandDataBlock(TS, Term));
                }

                // Seperator Elements auslesen
                XElement seperators = doc.Element("Device").Element("Seperators");
                if (seperators != null)
                {
                    this.DevDefinitionData.Seperators.Clear();
                    IEnumerable<XElement> sepTerms = seperators.Elements("Seperator");
                    foreach (var Term in sepTerms)
                    {
                        string TZ = (string)Term.Value;
                        this.DevDefinitionData.Seperators.Add(TZ);
                    }
                }

                // Commands und zugehörige Antwortlisten einlesen
                XElement commands = doc.Element("Device").Element("Commands");
                if (commands != null)
                {
                    string cmdIn = string.Empty;
                    this.DevDefinitionData.CommandDictionary.Clear();
                    IEnumerable<XElement> CmdElements = commands.Elements("Command");
                    foreach (var CMD in CmdElements)
                    {
                        IEnumerable<XElement> InElements = CMD.Elements("In");
                        foreach (var InElem in InElements)
                        {
                            cmdIn = (string)InElem.Value;
                        }

                        var AnswerList = new List<Data.CommandAnswers>();
                        IEnumerable<XElement> AnswerElements = CMD.Elements("Answer");
                        foreach (var AnswerElem in AnswerElements)
                        {
                            var OutList = new Data.CommandAnswers();
                            IEnumerable<XElement> OutElements = AnswerElem.Elements("Out");
                            foreach (var OutElem in OutElements)
                            {
                                string Delay = (string)OutElem.Attribute("Delay");
                                string[] DelayPart = Delay.Split('.');
                                TimeSpan TS = new TimeSpan(0, 0, 0, 0, XmlConvert.ToInt32(DelayPart[0]));
                                string TermOut = (string)OutElem.Value;

                                var Tup = new Data.CommandDataBlock(TS, TermOut);
                                OutList.Add(Tup);
                            }
                            AnswerList.Add(OutList);
                        }
                        this.DevDefinitionData.CommandDictionary.Add(cmdIn, AnswerList);
                    }
                }                    

                if (this.out_DeviceDefinitionData != null)
                {
                    this.out_DeviceDefinitionData(DevDefinitionData);
                }
            }
        }

        public event Action<Data.DeviceDefinitionData> out_DeviceDefinitionData;
    }
}
