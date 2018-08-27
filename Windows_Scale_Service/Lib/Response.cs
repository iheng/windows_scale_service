using System.IO.Ports;

namespace ScaleService.Lib
{
    public class Balance_Result
    {
        public string UnitOfMesure { get; set; } = "kg";
        public string Weight { get; set; }
        public string Scale_Model { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Scale_Timeout { get; set; } = "300";
        public string Scale_Status { get; internal set; }
    }
}