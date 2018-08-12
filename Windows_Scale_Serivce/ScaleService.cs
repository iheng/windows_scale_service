using ScaleService.Scale_Models;
using System.Collections.Generic;
using System.ServiceModel;
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
        List<Scale_Result> Get_Weight();
    }
   

}
