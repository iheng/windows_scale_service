using System;
using log4net;
//using System.IO.Ports;
using ScaleService.Shared;
using System.Collections.Generic;
using RJCP.IO.Ports;

namespace ScaleService.Scale_Models
{
    public sealed class XiangPing_ES_T:Scale_Model
    {
        static XiangPing_ES_T Xping_ES = null;
        static readonly object padlock = new object();
        public string s_Model { get; private set; }
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private byte terminator = 107; //'k'
        private byte StartByte = 43; // '+'
        
        public XiangPing_ES_T(string Model = "XiangPing_ES_T") : base(Model)
        {
            try
            {
                PortName = System.Configuration.ConfigurationManager.AppSettings[Model].ToString();
            }
            catch (Exception e)
            {
                log.Error(e.Message.ToString());
            }
            s_Model = Model;
            BaudRate = 9600;
            StopBits = StopBits.One;
            Parity = Parity.None;
            DataBits = 8;

        }

        public static XiangPing_ES_T _XiangPing
        {
            get
            {
                lock (padlock)
                {
                    if (Xping_ES== null)
                    {

                        Xping_ES = new XiangPing_ES_T();
                    }
                }
                return Xping_ES;

            }
        }

        public void data_Recieved(byte[] recieved)
        {
            string currString = System.Text.Encoding.ASCII.GetString(recieved);
            string workingString;
            Double roundValue;
            Double parsedValue;
            
            int startIndex = currString.IndexOf((char)StartByte);
            int endIndex = currString.IndexOf((char)terminator);
            if (startIndex > -1 && endIndex > -1)
            {
                workingString = currString.Substring(startIndex, endIndex);
                if (Double.TryParse(workingString.TrimStart('+'), out parsedValue))
                {
                    roundValue = Math.Round(parsedValue, 2);
                    Scale_Value = roundValue.ToString();
                    data_recieved = true;
                } 
                 
            }
            else
            {
                Scale_Value = "0.00";
                data_recieved = false;
                return;
            }          
        }

        public List<Balance_Result> Create_Response(string status, string ScaleWeight, string message)
        {
            List<Balance_Result> Xiang_Result = new List<Balance_Result>();
            if (status == "Success")
            {
                Xiang_Result.Add(new Balance_Result
                {
                    Status = status,
                    Scale_Model = s_Model,
                    Message = message,
                    Weight = ScaleWeight,
                });
            }
            else
            {
                Xiang_Result.Add(new Balance_Result
                {
                    Status = status,
                    Scale_Model = s_Model,
                    Message = message,
                    Weight = ScaleWeight,
                });
            }
            return Xiang_Result;
        }
      
    }
}