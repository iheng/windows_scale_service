using ScaleService.Scale_Models;
using System.ServiceModel;
using System.ServiceProcess;

namespace ScaleService
{
    public class ScaleWindowsService : ServiceBase
    {

        public ServiceHost serviceHost = null;
        public ScaleWindowsService()
        {
            // Name the Windows Service
            ServiceName = "ScaleWindowsService";
        }

        public static void Main()
        {
            ServiceBase.Run(new ScaleWindowsService());
        }
        // Start the Windows service.
        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            // Create a ServiceHost for the BreckNell_335 type and 
            // provide the base address.
            serviceHost = new ServiceHost(typeof(Scale));

            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
        
}