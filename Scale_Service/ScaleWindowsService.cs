using System.ServiceModel;
using System.ServiceProcess;
using log4net;
using ScaleService.Scale_Models;
using System.Threading;

namespace ScaleService
{

    public class ScaleWindowsService : ServiceBase
    {
        
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ServiceHost serviceHost = null;

        public ScaleWindowsService()
        {
            // Name the Windows Service
            ServiceName = "ScaleWindowsService";

        }
        public static void Main()
        {           
            Run(new ScaleWindowsService());
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
            log.Info("Program Starting");
            serviceHost.Open();
            Brecknell._M335.Open_Port();

        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {        
                serviceHost.Close();
                serviceHost = null;
                Brecknell._M335.Close_Port();
                Thread.Sleep(100);

            }
            log.Info("Program exist");
        }
    }
        
}