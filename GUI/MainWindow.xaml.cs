using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using ebcDeviceSimulator.GUI.Properties;

namespace ebcDeviceSimulator.GUI
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Variable, in der die Device Definitions Daten abgelegt werden
        private Data.DeviceDefinitionData GUI_DDD = null;

        // die seriellen Ports
        private SerialPort RS232_PC = null;
        private SerialPort RS232_Device = null;  // nur für den Lernmodus benötigt

        // Umschalter zwischen Simulation (A) und Lern Modus (B)
        private ebc.Activities.Switch<string> TaskSwitch = null;

        // Actions, um die eingehenden Daten vom Steuerrechner in der Textbox anzeigen zu können
        private ebc.Activities.SplitMessage<string> Split = null;
        private ebc.Activities.SyncWithGUI<string> syncInput = null;
        private ebc.Activities.ConvertControlBytes CCB_InA = null;
        private ebc.Activities.SendTriggerSignal<string> Trigger = null;
        private ebc.Activities.SyncWithGUI<string> syncClear = null;

        // Converter für die Anzeige der Steuerzeichen (A = PC, B = Device)
        private ebc.Activities.ConvertControlBytes CCB_A = null;
        private ebc.Activities.ConvertControlBytes CCB_B = null;

        private ebcDeviceSimulator.Tasks.Simulation Sim = null;
        private ebcDeviceSimulator.Tasks.LearningMode Learn = null;
        private ebc.Activities.SyncWithGUI<string> sync = null;
        private ebcDeviceSimulator.Activities.LoadData load = null;
        private ebcDeviceSimulator.Activities.SaveData save = null;

        public MainWindow()
        {
            InitializeComponent();
            
            // Serielle Schnittstellen
            RS232_PC = new SerialPort();
            RS232_PC.DataReceived += new SerialDataReceivedEventHandler(RS232_PC_DataReceived);
            RS232_Device = new SerialPort();
            RS232_Device.DataReceived += new SerialDataReceivedEventHandler(RS232_Device_DataReceived);

            // Splitter, um die empfangenen Daten auch in der Textbox anzuzeigen
            Split = new ebc.Activities.SplitMessage<string>();
            syncInput = new ebc.Activities.SyncWithGUI<string>();
            CCB_InA = new ebc.Activities.ConvertControlBytes();

            // Anlegen der benötigten Actions
            TaskSwitch = new ebc.Activities.Switch<string>();

            CCB_A = new ebc.Activities.ConvertControlBytes();
            CCB_B = new ebc.Activities.ConvertControlBytes();

            Sim = new Tasks.Simulation();
            Learn = new Tasks.LearningMode();
            sync = new ebc.Activities.SyncWithGUI<string>();
            load = new Activities.LoadData();
            save = new Activities.SaveData();

            // Initialisierung
            this.loadDefinition += load.in_LoadData;
            load.out_DeviceDefinitionData += Sim.in_DeviceDefinition;
            load.out_DeviceDefinitionData += Learn.in_DeviceDefinitionData;
            load.out_DeviceDefinitionData += this.showSeperatorsAndErrorAnswerInTextBox;

            // Input and Output from/to RS232 Port A (PC)
            this.DataFromPC += Split.in_SplitMessage;
            // Weg A = Ausgabe in der TextBox
            Split.out_A += syncInput.in_SwitchToGuiThread;
            syncInput.out_SwitchedToGuiThread += CCB_InA.in_encodeString;
            CCB_InA.out_encodedString += this.showInputInTextBox;
            // Weg B = weiter an den Simulator Task
            Split.out_B += TaskSwitch.in_Message;
            // A = Simulation
            TaskSwitch.out_MessageToA += Sim.in_receivedData;
            Sim.out_simulatedAnswer += this.RS232_PC_SendData;
            // B = Learning Mode
            TaskSwitch.out_MessageToB += Learn.in_DataFromPC;
            Learn.out_DataToPC += this.RS232_PC_SendData;

            // store new XML file imediately
            Learn.out_NewDeviceDefinition += this.saveXML;
            Learn.out_NewDeviceDefinition += Sim.in_DeviceDefinition;

            // Input and Output from/to RS232 Port B (Device) (only learning mode!)
            Learn.out_DataToDevice += this.RS232_Device_SendData;
            this.DataFromDevice += Learn.in_DataFromDevice;

            // Input from TextBox (emulating serial data from PC)
            // (Simulation and Learning Mode!)
            this.dataFromTextBoxPC += CCB_A.in_decodeString;
            CCB_A.out_decodedString += TaskSwitch.in_Message;

            // Output of the simulated Answer to TextBox
            // including GUISync and converting ControlByte into readable terms
            // (only Simulation mode)
            Sim.out_simulatedAnswer += sync.in_SwitchToGuiThread;
            sync.out_SwitchedToGuiThread += CCB_A.in_encodeString;
            CCB_A.out_encodedString += this.showSimulatedAnswerInTextBox;

            // Input from TextBox (emulating serial data from Device)
            // (only Learning mode)
            this.dataFromTextBoxDevice += CCB_B.in_decodeString;
            CCB_B.out_decodedString += Learn.in_DataFromDevice;
            this.CommandComplete += Learn.in_CommandCompleted;

            // Input from TextBox (Separators)
            // (only Learning mode)
            this.DeviceDefinitionDataChanged += Sim.in_DeviceDefinition;
            this.DeviceDefinitionDataChanged += Learn.in_DeviceDefinitionData;
            this.DeviceDefinitionDataChanged += this.saveXML;

            Init();

        }

		private event Action<string> loadDefinition;
        
        private void Init()
        {
            // Start process by loading the DeviceDefinitionFile
            string filename = Settings.Default.DeviceDefinitionFile ?? @".\DemoData\Test.xml";
            this.loadDefinition(filename);

            // In den Simulations Modus schalten
            this.GUI_ToSimulationMode();

            // Com Ports öffnen
            RS232_PC.PortName = Settings.Default.COMPort_Simulator_PC;
            RS232_PC.BaudRate = Settings.Default.COMPort_Simulator_PC_Baud;
            RS232_PC.Open();

            RS232_Device.PortName = Settings.Default.COMPort_Simulator_Device;
            RS232_Device.BaudRate = Settings.Default.COMPort_Simulator_Device_Baud;
            // RS232_Device.Open();

        }
        
		private void saveXML(Data.DeviceDefinitionData DDD)
        {
            this.save.in_SaveData(DDD);
        }

        private event Action<string> dataFromTextBoxPC;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataFromTextBoxPC != null) { this.dataFromTextBoxPC(textBox1.Text); }
        }

        public void showInputInTextBox(string Ans)
        {
            this.textBox1.Text = this.textBox1.Text + Ans;
        }

        public void clearTextBox1()
        {
            this.textBox1.Text = string.Empty;
        }


        public void showSimulatedAnswerInTextBox(string Ans)
        {
            this.textBox2.Text = Ans + Environment.NewLine + this.textBox2.Text;
            this.clearTextBox1();
        }

        private event Action<Data.DeviceDefinitionData> DeviceDefinitionDataChanged;
      
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            var SepList = new Data.DeviceSeparatorList();
            // lese SeparatorList aus der TextBox
            var lines = textBox4.Text.Split(Environment.NewLine.ToCharArray());
            foreach (var line in lines)
            {
                if (line != string.Empty)
                {
                    SepList.Add(line);
                }
            }
            GUI_DDD.Seperators = SepList;

            // Lese Error Answer aus Textbox
            var ErrorTerms = new Data.CommandAnswers();
            var ErrLines = textBox5.Text.Split(Environment.NewLine.ToCharArray());
            TimeSpan delay = new TimeSpan(0, 0, 0, 0, 100); // 100 ms
            foreach (var line in ErrLines)
            {
                if (line != string.Empty)
                {
                    var ErrCodeBlock = new Data.CommandDataBlock(delay, line);
                    ErrorTerms.Add(ErrCodeBlock);
                }
            }
            GUI_DDD.ErrorAnswers = ErrorTerms;

            if (DeviceDefinitionDataChanged != null) { DeviceDefinitionDataChanged(GUI_DDD); }
        }        
        private void showSeperatorsAndErrorAnswerInTextBox(Data.DeviceDefinitionData DDD)
        {
            GUI_DDD = DDD;
            ShowSeperators(DDD);
            ShowErrorAnswer(DDD);
        }

        private void ShowErrorAnswer(Data.DeviceDefinitionData DDD)
        {
            textBox5.Text = string.Empty;
            foreach (var Err in DDD.ErrorAnswers)
            {
                textBox5.AppendText(Err.Item2);
                textBox5.AppendText(Environment.NewLine);
            }
        }

        private void ShowSeperators(Data.DeviceDefinitionData DDD)
        {
            textBox4.Text = string.Empty;
            foreach (var Sep in DDD.Seperators)
            {
                textBox4.AppendText(Sep);
                textBox4.AppendText(Environment.NewLine);
            }
        }

        private event Action<string> dataFromTextBoxDevice;
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataFromTextBoxDevice != null) { this.dataFromTextBoxDevice(textBox3.Text); }
        }

        private event Action CommandComplete;
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (this.CommandComplete != null) { this.CommandComplete(); }
        }


        // RS232 PC Anschluß
        private event Action<string> DataFromPC;
        void RS232_PC_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = RS232_PC.ReadExisting();
            if (this.DataFromPC != null) { this.DataFromPC(data); }
        }
        private void RS232_PC_SendData(string data)
        {
            try
            {
                RS232_PC.Write(data);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("RS232_PC_SendData :" + ex.Message);
            }
        }

        // RS232 Device Anschluß
        private event Action<string> DataFromDevice;
        void RS232_Device_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = RS232_Device.ReadExisting();
            if (this.DataFromDevice != null) { this.DataFromDevice(data); }
        }
        private void RS232_Device_SendData(string data)
        {
            try
            {
                RS232_Device.Write(data);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("RS232_Device_SendData :" + ex.Message);
            }
        }


        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            string CRLF = "[CR][LF]";
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                if (!textBox1.Text.Contains(CRLF))
                {
                    textBox1.Text += CRLF;
                }
                button1_Click(this, new RoutedEventArgs());
            }
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            GUI_ToLearnMode();
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            GUI_ToSimulationMode();
        }

        private void GUI_ToSimulationMode()
        {
            this.Title = "Device Simulation GUI - Simulation Mode";
            button2.IsEnabled = false;
            button3.IsEnabled = false;
            button4.IsEnabled = false;
            textBox2.IsEnabled = true;
            textBox3.IsEnabled = false;
            textBox4.IsEnabled = false;
            textBox5.IsEnabled = false;
            this.TaskSwitch.in_SetSwitch(ebc.Activities.SwitchPath.SwitchToA);
        }

        private void GUI_ToLearnMode()
        {
            this.Title = "Device Simulation GUI - Learning Mode";
            button2.IsEnabled = true;
            button3.IsEnabled = true;
            button4.IsEnabled = true;
            textBox2.IsEnabled = false;
            textBox3.IsEnabled = true;
            textBox4.IsEnabled = true;
            textBox5.IsEnabled = true;
            this.TaskSwitch.in_SetSwitch(ebc.Activities.SwitchPath.SwitchToB);
        }

    }
}
