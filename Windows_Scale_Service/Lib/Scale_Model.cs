using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace ScaleService.Scale_Models
{
    public class Scale_Model
    {
        private SerialPort _serialPort;
        protected string ScaleModel;
        public bool data_recieved { get; set; }
        public string Scale_Weight { get; set; }

        public Scale_Model(SerialPort SPort, string Model) 
        {
             ScaleModel = Model;
            _serialPort = SPort;
            data_recieved = false;
        }

        public string Open() {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    _serialPort.Open();
                }
            }
            catch (Exception ex) {return ex.ToString();};
            return "Success";
                
        }
        public void Close()
        {
            if (_serialPort.IsOpen)
            {
               _serialPort.Close();
            }               
        }
        public void AsnycReadEvent()
        {
            const int blockLimit = 4096;
            byte[] buffer = new byte[blockLimit];
            Action kickoffRead = null;
            kickoffRead = delegate {
                _serialPort.BaseStream.BeginRead(buffer, 0, buffer.Length, delegate (IAsyncResult ar) {
                    try
                    {
                        int actualLength = _serialPort.BaseStream.EndRead(ar);
                        byte[] received = new byte[6];
                        if (buffer.Length >= 6 && buffer[1] == 160)
                        {
                            Buffer.BlockCopy(buffer, 4, received, 0, 6);
                            Parse(received);
                            data_recieved = true;
                        }
                    }
                    catch (IOException exc)
                    {
                        handleAppSerialError(exc);
                    }
                    kickoffRead();
                }, null);
                kickoffRead();
            };
        }
        private void Parse(byte[] dataToParse)
        {
            string rawweight = System.Text.Encoding.ASCII.GetString(dataToParse);
            int weight = Convert.ToInt16(rawweight);
            double d = weight / 100.0;
            Scale_Weight = d.ToString();
        }

        private void handleAppSerialError(IOException exc)
        {
            exc.ToString();
        }
    }
}