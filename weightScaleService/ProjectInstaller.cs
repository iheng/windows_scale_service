using System.Configuration.Install;
using System.ServiceProcess;
using System.ComponentModel;



namespace ScaleService
{
    [RunInstaller(true)]
    public class ProjectInstaller:Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;
       
        public ProjectInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = "ScaleWindowsService";
            service.StartType = ServiceStartMode.Automatic;
            Installers.Add(process);
            Installers.Add(service);
            service.AfterInstall += new InstallEventHandler(ServiceInstaller_AfterInstall);
            
            
        }
        void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            ServiceInstaller serviceInstaller = (ServiceInstaller)sender;
            using (ServiceController sc = new ServiceController(serviceInstaller.ServiceName))
            {
                sc.Start();
                
            }
        }
    }
}