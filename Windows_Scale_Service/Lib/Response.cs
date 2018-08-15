using System.IO.Ports;

namespace ScaleService.Scale_Models
{
    public class Scale_Result
    {
        public string UnitOfMesure { get; set; } = "kg";
        public string Weight { get; set; }
        public string Scale_Model { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Scale_Timeout { get; set; } = "300";
    }
}