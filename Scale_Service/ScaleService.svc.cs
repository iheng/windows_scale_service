using System.Collections.Generic;
using ScaleService.Controller;
using System.IO.Ports;
using ScaleService.Shared;

namespace ScaleService
{
    public class Scale: IScaleService
    {
        private Device_Controller _Controller = new Device_Controller();

        public List<Balance_Result> Get_Weight()
        {
            //string Scale_Model = "Brecknell_335";
            //List<Balance_Result> Balance_Result = _Controller.Get_Scale_Value(Scale_Model);
            return _Controller.Get_Scale_Value();  
        }

        public List<Ports> Get_Available_Ports()
        {
            string[] ports = SerialPort.GetPortNames();
            List<Ports> Avaiable_Ports = new List<Ports>();
            foreach (var aports in ports)
            {
                Avaiable_Ports.Add(new Ports { port_Name = aports });
            }
            return Avaiable_Ports;
        }

        public void Set_Device(string Port_Name,string Device_Name)
        {
            _Controller.Set_Device(Port_Name,Device_Name);
        }
        /*
        public List<byte[]> Upload_Image()
        {
            return _Controller.Image_Recivied();
        }
        */
    }      
}

