using System;
using System.IO.Ports;
using log4net;
using System.Configuration;


namespace ScaleService.Shared
{
    public class Scale_Model : SerialPort
    {
        protected enum Connect_Status:int
        {
            SCALE_OFFLINE = 0,
            SCALE_ONLINE = 1
        }

        protected enum Scale_Status
        {
            wtNORMAL_MODE = 0,
            wtTEST_MODE = 1,
            wtCALIB_MODE = 2,
            wtSHOWING_TARE = 3,
            wtSHOWING_LO = 4,
            wtSHOWING_ERR = 5,
            wtSHOWING_ERRL = 6,
            wtSHOWING_DASHES = 7,
            wtNOT_USED1 = 8,
            wtNOT_USED2 = 9,
            wtNOT_USED3 = 10,
            wtNOT_USED4 = 11,
            wtSHOWING_8888 = 12,
            wtSHOWING_TARE_ERR = 13,
            wtCALIB_MODE_TARE = 14,
            wtSHOWING_CAL = 15
        }
        protected enum Scale_Comm_Event
        {
            wtWTCOMM_EV_WEIGHT = 1,      //(weight updated or changed)
            wtWTCOMM_EV_STATUS = 2,      //(status changed)
            wtWTCOMM_EV_DISCONNECT = 3,  //(scale disconnected, went offline)
            wtWTCOMM_EV_RECONNECT = 4    //(scale reconnected, back online)
        }
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected string ScaleModel;
        public bool data_recieved { get; set; }
        public string Scale_Value { get; set; }
        public bool Is_Value_Changed { get; set; }
        //protected string COM_Port;
       
        public Scale_Model(string Model): base()
        {
            ScaleModel = Model;
            data_recieved = false;   
        }

        public void Open_Port()
        {
            try
            {
                if (!IsOpen)
                {
                    Open();
                }
            }
            catch (Exception ex)
            {
                //DiscardInBuffer();
                //Dispose(true);
                //GC.SuppressFinalize(this);
                //ReleaseCOMObject(this);
                data_recieved = false;
                log.Error("Could not open port connection. Exception: " + ex.ToString());
            }

        }
        private void ReleaseCOMObject(object o)
        {
            Int32 countDown = 1;
            while (countDown > 0)
                countDown = System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
        }
        public void Close_Port()
        {
            try
            {
                if (IsOpen)
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                data_recieved = false;
                log.Error("Could not close port connection. Exception: " + ex.ToString());
            }

        }
        public void Set_Device(string port_name, string Device_Name)
        {
            try
            {
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (configuration.AppSettings.Settings[Device_Name] == null)
                {
                    configuration.AppSettings.Settings.Add(Device_Name, port_name);
                }
                else {
                    configuration.AppSettings.Settings[Device_Name].Value = port_name;
                }
                if (configuration.AppSettings.Settings["Default_Scale_Option"] == null)
                {
                    configuration.AppSettings.Settings.Add("Default_Scale_Option", Device_Name);
                }
                else {
                    configuration.AppSettings.Settings["Default_Scale_Option"].Value = Device_Name;
                }
                configuration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (ConfigurationErrorsException e)
            {
                log.Error(e.ToString());
            }
        }

        /*
        private void Port_Value_Changed()
        {
        }
        */
    }
}
