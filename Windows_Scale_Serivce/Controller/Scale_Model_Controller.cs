using System.Collections.Generic;
using System.IO.Ports;
using ScaleService.Scale_Models;

namespace ScaleService.Controller
{
    public class Scale_Controllers
    {
        static SerialPort _sPort;
        string ScaleModel;
        static BreckNell_335 BNell;
        public Scale_Controllers() {
            ScaleModel = "BreckNell_335";
            _sPort = new SerialPort("COM3")
            {
                BaudRate = 2400,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.Two,
                Handshake = Handshake.None,
                ReadTimeout = 300
            };
            BNell = new BreckNell_335(_sPort, ScaleModel);
        }
        public List<Weightinfo> BkNell_335()
        {       
            return BNell.Get_Weight();
        }    
    }   
   

}