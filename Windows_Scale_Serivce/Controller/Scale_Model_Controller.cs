using System.IO.Ports;
using ScaleService.Scale_Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ScaleService.Controller
{
    public class Scale_Controllers
    {
        static SerialPort _sPort;
        private string ScaleModel;
        private Scale_Model BreckNell_335;
        private string Scale_Weight;
        
        public Scale_Controllers()
        {
            ScaleModel = "";
            Scale_Weight = "";
            _sPort = null;
            BreckNell_335 = null;
        }

        private List<Scale_Result> Create_Response(string status,string ScaleWeight)
        {
            List<Scale_Result> BreckNell_335_Result = new List<Scale_Result>();
            if(status == "Success")
            {
                BreckNell_335_Result.Add(new Scale_Result
                {
                    status = status,
                    ScaleModel = ScaleModel,
                    UnitOfMesure = "KG",
                    weight = ScaleWeight
                });
            }
            else
            {
                BreckNell_335_Result.Add(new Scale_Result
                {
                    status = status,
                    ScaleModel = ScaleModel,
                    UnitOfMesure = "KG",
                    weight = ScaleWeight
                });
            }
            return BreckNell_335_Result;
            
        }

        public List<Scale_Result> Get_Weight()
        { 
            Init_BreckNell_335();
            _sPort.DataReceived += new SerialDataReceivedEventHandler(BkNell335_PortDataReceived);
            BreckNell_335.Open();
            Thread.Sleep(1000);
            BreckNell_335.Close();
            if (BreckNell_335.data_recieved)
                return Create_Response("Success",Scale_Weight);
            return Create_Response("Error","-1");
                 
        }
        private void BkNell335_PortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer;
            int length;
            if (!_sPort.IsOpen) { return; }
            int size = _sPort.BytesToRead;
            buffer = new byte[size];
            length = _sPort.Read(buffer, 0, size);
            if (length >= 6 && buffer[1] == 160)
            {
                byte[] temp = new byte[6];
                Array.Copy(buffer, 4, temp, 0, 6);
                Parse(temp);
                BreckNell_335.data_recieved = true;
            }
        }

        private void Parse(byte[] dataToParse)
        {
            string rawweight = System.Text.Encoding.ASCII.GetString(dataToParse);
            int weight = Convert.ToInt16(rawweight);
            double d = weight / 100.0;
            Scale_Weight = d.ToString();
        }

        private void ScalePort_Config(SerialPort _sPort,string model) {
            ScaleModel = model;
            _sPort = new SerialPort(_sPort.PortName)
            {
                BaudRate =_sPort.BaudRate,
                Parity =_sPort.Parity,
                DataBits = _sPort.DataBits,
                StopBits = _sPort.StopBits,
                Handshake = _sPort.Handshake,
                ReadTimeout = _sPort.ReadTimeout,
                WriteTimeout = _sPort.WriteTimeout
            };   
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
            ScalePort_Config(_sPort, ScaleModel);
            BreckNell_335 = new Scale_Model(_sPort, ScaleModel);     
        }        
    }   
}