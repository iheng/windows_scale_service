using System.Collections.Generic;
using ScaleService.Controller;
using ScaleService.Scale_Models;
using System.Threading;

namespace ScaleService
{
    public class Scale: IScaleService
    {
        private Scale_Controllers SC_Controller = new Scale_Controllers();

        public List<Scale_Result> Get_Weight(string Scale_Model)
        {
         
            return SC_Controller.Get_Weight(Scale_Model);  
        }

    }      
}

