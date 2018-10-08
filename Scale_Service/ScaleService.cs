using System.Collections.Generic;
using System.ServiceModel;
using ScaleService.Shared;
using System.ServiceModel.Web;
using System.Runtime.Serialization;

namespace ScaleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IScaleService
    {
        // TODO: Add your service operations here
        [OperationContract]
        [WebGet]
        List<Balance_Result> Get_Weight();

        [OperationContract]
        [WebGet]
        List<Ports> Get_Available_Ports();

        [OperationContract]
        [WebGet]
        void Set_Device(string Port_Name, string Device_Name);
        /*
        [OperationContract]
        [WebGet]
        List<byte[]> Upload_Image();
        */
    }
    /*
    [DataContract]
    public class Device_Property
    {
        [DataMember]
        public string Port_Name { get; set; }

        [DataMember]
        public string Device_Name { get; set; }
       
    }*/

   

}
