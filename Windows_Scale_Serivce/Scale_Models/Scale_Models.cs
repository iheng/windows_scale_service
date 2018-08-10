using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace ScaleService.Scale_Models
{
    
    public class BreckNell_335
    {
        private SerialPort _serialPort;
        public string ScaleWeight { get; set; }
        static string ScaleModel;
        private bool dataReceived;
        public BreckNell_335(SerialPort SPort, string Model) 
        {
             dataReceived = false;
             ScaleWeight = "";
             ScaleModel = Model;
            _serialPort = new SerialPort(SPort.PortName)
            {

                BaudRate = SPort.BaudRate,
                Parity = SPort.Parity,
                DataBits = SPort.DataBits,
                StopBits = SPort.StopBits,
                Handshake = SPort.Handshake,
                ReadTimeout = SPort.ReadTimeout
            };
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
            byte[] buffer;
            int length = _serialPort.BytesToRead;
            /*
            if (length == 0)
            {
                buffer = null;
                ScaleWeight = "0";
                return;
            }*/
            buffer = new byte[length];
            int len = 0;
            while (len < length)
            {
                len += _serialPort.Read(buffer, 0, length);
            };
            //undifined
            //192 --gram
            //160 -kg
            //176 -ib           
            if (buffer[1] == 160)
            {
                byte[] temp = new byte[6];
                Array.Copy(buffer, 4, temp, 0, 5);
                string rawweight = System.Text.Encoding.ASCII.GetString(temp);
                int weight = Convert.ToInt16(rawweight);
                double d = weight / 100.0;
                ScaleWeight = d.ToString();
            }
            dataReceived = true;
            _serialPort.Close();
        }
        public List<Weightinfo> Get_Weight()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            while (!dataReceived)
            {
                Thread.Sleep(500);
            }
            List<Weightinfo> Weight = new List<Weightinfo>();
            Weight.Add(new Weightinfo { ScaleModel = ScaleModel, UnitOfMesure = "KG", weight = ScaleWeight});
            return Weight;
        }   
    }
}