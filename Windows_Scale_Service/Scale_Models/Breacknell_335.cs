using ScaleService.Scale_Models;
using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace weightScaleService.Scale_Models
{
 
    public class Brecknell_335:Scale_Model
    {
        private SerialPort _SerialPort;
        public Brecknell_335(SerialPort _sPort, string Model) :base(_sPort,Model)
        {
            _SerialPort = _sPort;

        }

        public List<Scale_Result> Create_Response(string status, string ScaleWeight, string message)
        {
            List<Scale_Result> BreckNell_335_Result = new List<Scale_Result>();
            if (status == "Success")
            {
                BreckNell_335_Result.Add(new Scale_Result
                {
                    Status = status,
                    Scale_Model = ScaleModel,
                    Message = message,
                    Weight = ScaleWeight
                });
            }
            else
            {
                BreckNell_335_Result.Add(new Scale_Result
                {
                    Status = status,
                    Scale_Model = ScaleModel,
                    Message = message,
                    Weight = ScaleWeight
                });
            }
            return BreckNell_335_Result;
        }

        public void Parse(byte[] dataToParse)
        {
            string rawweight = System.Text.Encoding.ASCII.GetString(dataToParse);
            int weight = Convert.ToInt16(rawweight);
            double d = weight / 100.0;
            Scale_Weight = d.ToString();
        }
        public void Recieved(byte [] buffer, byte [] DataRecived,int length)
        {
            if (length >= 6 && buffer[1] == 160)
            {
                Array.Copy(buffer, 4, DataRecived, 0, 6);
                Parse(DataRecived);
                data_recieved = true;
            }
        }
    }
}