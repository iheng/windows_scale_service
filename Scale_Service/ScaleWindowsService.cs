using System.ServiceModel;
using System.ServiceProcess;
using log4net;
using ScaleService.Scale_Models;
using System.Management;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;

namespace ScaleService
{

    public class ScaleWindowsService : ServiceBase
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ServiceHost serviceHost = null;

        public ScaleWindowsService()
        {
            // Name the Windows Service
            ServiceName = "ScaleService";

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
            //log.Info("Program Starting");
            InitDevices();
            serviceHost.Open();
            // Brecknell._M335.Open_Port();
            XiangPing_ES_T._XiangPing.Open_Port();


        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
                //Brecknell._M335.Close_Port();
                XiangPing_ES_T._XiangPing.Close_Port();
            }
            //log.Info("Program exist");
        }

        private void SaveToconfig(Dictionary<string,string> Scales)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            try
            {
                foreach (var scale in Scales)
                {
                    if (configuration.AppSettings.Settings[scale.Key] == null)
                    {
                        configuration.AppSettings.Settings.Add(scale.Key, scale.Value);
                    }
                    else {
                        configuration.AppSettings.Settings[scale.Key].Value = scale.Value;
                    }


                }
                if (configuration.AppSettings.Settings["Default_Scale_Option"] == null)
                {
                    configuration.AppSettings.Settings.Add("Default_Scale_Option", "XiangPing_ES_T");
                }
                else {
                    configuration.AppSettings.Settings["Default_Scale_Option"].Value = "XiangPing_ES_T";
                }
                configuration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (ConfigurationErrorsException e)
            {
                log.Error(e.ToString());
            }
        }

        private void InitDevices()
        {
            Dictionary<string, string> Devices = new Dictionary<string, string>();
            try
            {
                ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_PnPEntity where  ClassGuid='{4d36e978-e325-11ce-bfc1-08002be10318}'");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["Caption"].ToString().Contains("(COM"))
                    {
                        if (queryObj["Description"].ToString().Contains("CH340"))
                        {
                            Devices.Add("Brecknell_335", queryObj["Caption"].ToString().Substring(queryObj["Caption"].ToString().IndexOf('(') + 1, 4));
                        }
                        else {
                            Devices.Add("XiangPing_ES_T", queryObj["Caption"].ToString().Substring(queryObj["Caption"].ToString().IndexOf('(') + 1, 4));
                        } 

                    }
                }
                SaveToconfig(Devices);
               
            }
            catch (ManagementException e)
            {
                log.Error(e.ToString());
            }
        }
    }
}