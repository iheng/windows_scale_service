using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace weightScaleService
{
    class Program
    {
       
        static SerialPort _serialPort;
        static void Main(string[] args)
        {
            //string[] ports = SerialPort.GetPortNames();
            //Console.WriteLine("The following serial ports were found:" );
            // Display each port name to the console.
            /*
            foreach (string port in ports)

            {

                Console.WriteLine(port);
            }
            */
            //string weight;

            Encoding utf8 = new UTF8Encoding(true, true);
            _serialPort = new SerialPort("COM3")
            {
                PortName = "COM3",
                BaudRate = 2400,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.Two,
                Handshake = Handshake.None,
                
            };
            _serialPort.ReadTimeout = 300;
            _serialPort.WriteTimeout = 300;
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            
          
            Console.ReadLine();
       
            

        }
        static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            int length = _serialPort.BytesToRead;
            byte[] buffer = new byte[length];
            int len = 0;
            while (len < length)
            {
                len +=_serialPort.Read(buffer, 0, length);
            };
            

            //string x= BitConverter.ToString(buffer);
            //192 --gram
            //160 -kg
            //176 -ib
            //
            
           
            if (buffer[1] == 192)
                Console.Write("gram");
            else if (buffer[1] == 160)
            {
                Console.Write("unit of measure kg\n");
                
                 byte[] temp = new byte[6];
                Array.Copy(buffer, 4, temp,0, 5);
                string rawweight = Encoding.ASCII.GetString(temp);
                int weight = Convert.ToInt16(rawweight);
                double w = weight / 100.0;
                Console.WriteLine("int: {0}", w+"kg");

            }
            else if (buffer[1] == 176)
                Console.Write("unit of measure lb");
            else
                Console.Write("Error, unknow unit of measure");

         
            
            

            //_serialPort.Close();
        }



    }
}

