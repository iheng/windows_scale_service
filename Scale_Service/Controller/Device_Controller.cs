using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Threading;
using log4net;
using ScaleService.Scale_Models;
using ScaleService.Shared;

namespace ScaleService.Controller
{
    public class Device_Controller
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string Current_Balance_Value;
        private string Scale_ModelN;

        public Device_Controller()
        {
            Current_Balance_Value = "";
            Brecknell._M335.DataReceived += _M335_DataReceived;
            XiangPing_ES_T._XiangPing.DataReceived += _XiangP_DataReceived;
            Scale_ModelN = "XiangPing_ES_T";
        }
        private void _XiangP_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] temp = new byte[8];
            byte[] buffer;
            try
            {
                
                int byteToRead = XiangPing_ES_T._XiangPing.BytesToRead;
                if (byteToRead > 13)
                {
                    buffer = new byte[byteToRead];
                    int length = XiangPing_ES_T._XiangPing.Read(buffer, 0, byteToRead);
                    XiangPing_ES_T._XiangPing.data_Recieved(buffer);

                }
            }

            catch (Exception ex)
            {
                XiangPing_ES_T._XiangPing.data_recieved = false;
                log.Error(ex.ToString());
            }

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
        /*
        public List<byte[]> Image_Recivied()
        {
            //return 
        }*/

        public List<Balance_Result> Get_Scale_Value()
        {
            if (Scale_ModelN == "Brecknell_335")
            {
                return _M335_Value_Recieved();
            }
            else if (Scale_ModelN == "XiangPing_ES_T")
            {
                return _Xiang_Value_Recieved();
            }

            return Brecknell._M335.Create_Response("Error", "-1", "No match Scale found");
        }
     
        public List<Balance_Result> _M335_Value_Recieved()
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
        public List<Balance_Result> _Xiang_Value_Recieved()
        {
            if (!XiangPing_ES_T._XiangPing.IsOpen)
            {
                XiangPing_ES_T._XiangPing.Open_Port();
            }
            if (XiangPing_ES_T._XiangPing.data_recieved)
            {
                Thread.Sleep(250);
                return XiangPing_ES_T._XiangPing.Create_Response("Success", XiangPing_ES_T._XiangPing.Scale_Value, "Success");
            }
            return XiangPing_ES_T._XiangPing.Create_Response("Error", "-1", "Plase look scale status. Value not recived,");

        }
        public void Set_Device(string port_name,string Device_Name)
        {
           Brecknell._M335.Set_Device(port_name,Device_Name);
        }
     



    }   
}