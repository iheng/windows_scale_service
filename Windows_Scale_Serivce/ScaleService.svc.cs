using System.Collections.Generic;
using ScaleService.Controller;
using ScaleService.Scale_Models;

namespace ScaleService
{
    public class Scale: IScaleService
    {
        static Scale_Controllers SC_Controller = new Scale_Controllers();

        public List<Weightinfo> Get_Weight()
        {                     
            return SC_Controller.BkNell_335();  
        }

    }      
}

