using System.Collections.Generic;
using ScaleService.Controller;
using System.IO.Ports;
using ScaleService.Lib;

namespace ScaleService
{
    public class Scale: IScaleService
    {
        private Scale_Controllers SC_Controller = new Scale_Controllers();

        public List<Balance_Result> Get_Weight()
        {
            string Scale_Model = "Brecknell_335";
            List<Balance_Result> Balance_Result = SC_Controller.Get_Scale_Value(Scale_Model);
            return Balance_Result;  
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
    }      
}

