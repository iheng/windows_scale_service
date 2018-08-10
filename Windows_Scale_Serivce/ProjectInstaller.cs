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
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}