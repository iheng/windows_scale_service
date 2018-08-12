using System.IO.Ports;

namespace ScaleService.Scale_Models
{
    public class Scale_Result
    {
        public string UnitOfMesure { get; set; }
        public string weight { get; set; }
        public string ScaleModel { get; set; }
        public string status { get; set; }
    }
}