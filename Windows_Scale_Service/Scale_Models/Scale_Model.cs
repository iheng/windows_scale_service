using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace ScaleService.Scale_Models
{
    public class Scale_Model
    {
        public SerialPort _serialPort;
        private string ScaleModel;
        public bool data_recieved { get; set; }
        public Scale_Model(SerialPort SPort, string Model) 
        {
             ScaleModel = Model;
            _serialPort = SPort;
            data_recieved = false;
        }

        public void Open() {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
                
        }
        public void Close()
        {
            if (_serialPort.IsOpen)
            {
               _serialPort.Close();
            }               
        }
    }
}