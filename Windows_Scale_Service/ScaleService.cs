using System.Collections.Generic;
using System.ServiceModel;
using ScaleService.Lib;
using System.ServiceModel.Web;

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
    }
   

}
