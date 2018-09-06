using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Threading;
using log4net;
using ScaleService.Scale_Models;
using ScaleService.Lib;

namespace ScaleService.Controller
{
    public class Scale_Controllers
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string Current_Balance_Value;

        public Scale_Controllers()
        {
            Current_Balance_Value = "";
            Brecknell._M335.DataReceived += _M335_DataReceived;
        }
    

        private void _M335_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] temp = new byte[6];
            byte[] buffer;
            int length;
            int size;
            try
            { 
                size = Brecknell._M335.BytesToRead;
                if (size > 0)
                {
                    buffer = new byte[size];
                    length = Brecknell._M335.Read(buffer, 0, size);
                    Brecknell._M335.Brecknell_335_data_Recieved(buffer, temp, length);

                }
            }

            catch (Exception ex)
            {
                Brecknell._M335.data_recieved = false;
                log.Error(ex.ToString());
            }
          
        }

        public List<Balance_Result> Get_Scale_Value(string Scale_Model)
        {
            if (Scale_Model == "Brecknell_335")
            {
                return Value_Recieved();
            }
            return Brecknell._M335.Create_Response("Error", "-1", "No match Scale found");
        }
     
        public List<Balance_Result> Value_Recieved()
        {
            if (!Brecknell._M335.IsOpen)
            {
                Brecknell._M335.Open_Port();
            }
            if (Brecknell._M335.data_recieved)
                {
                    Current_Balance_Value = Brecknell._M335.Scale_Value;
                    Thread.Sleep(250);
                    return Brecknell._M335.Create_Response("Success", Current_Balance_Value, "Success");
                }                
            return Brecknell._M335.Create_Response("Error", "-1","Plase look scale status. Value not recived," );
            
        }
        public void Set_Port(string port_name)
        {
           Brecknell._M335.Set_Port(port_name);
        }
  
        
    }   
}