﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ebcDeviceSimulator.GUI.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.6.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".\\DemoData\\Test.xml")]
        public string DeviceDefinitionFile {
            get {
                return ((string)(this["DeviceDefinitionFile"]));
            }
            set {
                this["DeviceDefinitionFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("COM1")]
        public string COMPort_PC_virtual {
            get {
                return ((string)(this["COMPort_PC_virtual"]));
            }
            set {
                this["COMPort_PC_virtual"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("COM2")]
        public string COMPort_Simulator_PC {
            get {
                return ((string)(this["COMPort_Simulator_PC"]));
            }
            set {
                this["COMPort_Simulator_PC"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("COM3")]
        public string COMPort_Simulator_Device {
            get {
                return ((string)(this["COMPort_Simulator_Device"]));
            }
            set {
                this["COMPort_Simulator_Device"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("COM4")]
        public string COMPort_Device_physical {
            get {
                return ((string)(this["COMPort_Device_physical"]));
            }
            set {
                this["COMPort_Device_physical"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9600")]
        public int COMPort_Simulator_PC_Baud {
            get {
                return ((int)(this["COMPort_Simulator_PC_Baud"]));
            }
            set {
                this["COMPort_Simulator_PC_Baud"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9600")]
        public int COMPort_Simulator_Device_Baud {
            get {
                return ((int)(this["COMPort_Simulator_Device_Baud"]));
            }
            set {
                this["COMPort_Simulator_Device_Baud"] = value;
            }
        }
    }
}
