using ScaleService.Shared;
using System;
using System.Collections.Generic;
using log4net;
using System.Configuration;
using System.IO.Ports;

namespace ScaleService.Scale_Models
{

    public sealed class Brecknell : Scale_Model
    {
        static Brecknell _335 = null;
        static readonly object padlock = new object();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string s_Model { get; private set; }
        public int ConnectStatus { get; private set; }
        public string S_Status { get; set; }


        public Brecknell(string Model = "Brecknell_335") : base(Model)
        {
            try
            {
                PortName = ConfigurationManager.AppSettings[Model].ToString();
            }
            catch (Exception e)
            {
                log.Error(e.Message.ToString());
            }

            BaudRate = 2400;
            ReadTimeout = 300;
            StopBits = StopBits.Two;
            WriteTimeout = 300;
            Handshake = Handshake.None;

            s_Model = Model;
            ConnectStatus = (int)Connect_Status.SCALE_OFFLINE;
           // ScaleStatus = (int)Scale_Status.wtNORMAL_MODE;

        }

        public static Brecknell _M335
        {
            get {
                lock (padlock)
                {
                    if (_335 == null)
                    {
                        
                        _335 = new Brecknell();
                    }
                }
                return _335;

            }
        }
        /*
        public void Set_Port(string port_name)
        {
            try
            {
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (configuration.AppSettings.Settings["port"] == null)
                {
                    configuration.AppSettings.Settings.Add("port", port_name);
                }
                else {
                    configuration.AppSettings.Settings["port"].Value = port_name;
                }
                configuration.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (ConfigurationErrorsException e)
            {
                log.Error(e.ToString());
            }
        }
        */
        public List<Balance_Result> Create_Response(string status, string ScaleWeight, string message)
        {
            List<Balance_Result> BreckNell_335_Result = new List<Balance_Result>();
            if (status == "Success")
            {
                BreckNell_335_Result.Add(new Balance_Result
                {
                    Status = status,
                    Scale_Model = s_Model,
                    Message = message,
                    Weight = ScaleWeight,
                    Scale_Status = S_Status
                });
            }
            else
            {
                BreckNell_335_Result.Add(new Balance_Result
                {
                    Status = status,
                    Scale_Model = s_Model,
                    Message = message,
                    Weight = ScaleWeight,
                    Scale_Status = S_Status
            });
            }
            return BreckNell_335_Result;
        }
        private void Parse(byte[] dataToParse)
        {
            string rawweight = System.Text.Encoding.ASCII.GetString(dataToParse);
            int weight = Convert.ToInt16(rawweight);
            double d = weight / 100.0;  
            Scale_Value = d.ToString();
        }
        public void Brecknell_335_data_Recieved(byte[] buffer, byte[] Value_Recived, int length)
        {
            if (length >= 6 && buffer[1] == 160)
            {
                Array.Copy(buffer, 4, Value_Recived, 0, 6);
                Parse(Value_Recived);
                data_recieved = true;
            }
           
        }

    }
      
    
}