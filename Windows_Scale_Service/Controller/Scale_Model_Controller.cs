using System.IO.Ports;
using ScaleService.Scale_Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using weightScaleService.Scale_Models;

namespace ScaleService.Controller
{
    public class Scale_Controllers
    {
        private SerialPort _sPort;
        private string ScaleModel;
        public Brecknell_335 _Brecknell_335;
        private string Scale_Weight;
        
        public Scale_Controllers()
        {
            ScaleModel = "";
            Scale_Weight = "";
            _sPort = null;
            _Brecknell_335 = null;
            Init_BreckNell_335();
        }

        private void Init_BreckNell_335()
        {

            ScaleModel = "BreckNell_335";
            _sPort = new SerialPort("COM3")
            {
                BaudRate = 2400,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.Two,
                Handshake = Handshake.None,
                ReadTimeout = 300,
                WriteTimeout = 300
            };
            _Brecknell_335 = new Brecknell_335(_sPort, ScaleModel);

        }

        public List<Scale_Result> Get_Weight(string Scale_Model)
        {
            if (Scale_Model == "Brecknell_335")
            {
                return Brecknell_335();
            }
            return _Brecknell_335.Create_Response("Error", "-1", "No match Scale found");
        }

        public List<Scale_Result> Brecknell_335()
        {
            
            string state = _Brecknell_335.Open();
            _sPort.DataReceived += new SerialDataReceivedEventHandler(BkNell335_PortDataReceived);
            if (state != "Success")
            {
                return _Brecknell_335.Create_Response("Error", "-1", "Scale is not turn on");
            }

            Thread.Sleep(1000);
            return IsData_Recieved();
        }
        
        private List<Scale_Result> IsData_Recieved()
        {
            if (_Brecknell_335.data_recieved)
            {
                _Brecknell_335.Close();
                return _Brecknell_335.Create_Response("Success", _Brecknell_335.Scale_Weight, "Success");
            }
            return _Brecknell_335.Create_Response("Error", Scale_Weight, "data has not yet recived");
        }

        private void BkNell335_PortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer;
            int length;
            int size;
            try
            { 
                if (!_sPort.IsOpen) { return; }
                size = _sPort.BytesToRead;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return;
            }
            buffer = new byte[size];
            byte[] temp = new byte[6]; 
            length = _sPort.Read(buffer, 0, size);
            _Brecknell_335.Recieved(buffer, temp, length);          
        }
    }   
}