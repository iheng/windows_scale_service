using System;
using System.Windows.Forms;
using System.ServiceProcess;
using Newtonsoft.Json;
using log4net;

namespace ScaleMonitor
{
    public partial class Form1 : Form
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ServiceController sc;
        ScaleService.ScaleServiceClient client;
        public Form1()
        {
            InitializeComponent();
            sc = new ServiceController("ScaleWindowsService");
            client = new ScaleService.ScaleServiceClient();
            DeviceNameTextBox.Text += sc.ServiceName.ToString();
            StatusTextBox.Text += sc.Status.ToString();
            progressBar1.Hide();
            try
            {
                ScaleService.Ports[] Availble_Ports = client.Get_Available_Ports();
                foreach (ScaleService.Ports port in Availble_Ports)
                {
                    Ports_Combo_Box.Items.Add(port.port_Name);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
            }

           
          
           
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            //if the form is minimized  
            //hide it from the task bar  
            //and show the system tray icon (represented by the NotifyIcon control)  
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                if (StatusTextBox.Text != null)
                {
                    StatusTextBox.Clear();
                }
                StatusTextBox.Text += ServiceControllerStatus.Stopped.ToString();
                Restart_Button.Enabled = false;
            }
        }

        private void Start_Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (sc.Status == ServiceControllerStatus.Stopped)
                {

                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                    if (StatusTextBox.Text != null)
                    {
                        StatusTextBox.Clear();
                    }
                    StatusTextBox.Text += ServiceControllerStatus.Running.ToString();
                    Restart_Button.Enabled = true;
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }



        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            string Device_Name = Device_Combo_Box.SelectedItem.ToString();
            string port_Name = Ports_Combo_Box.SelectedItem.ToString();
            client.Set_Device(port_Name,Device_Name);
            
        }

        private void restart_Click(object sender, EventArgs e)
        {
            using (ServiceController service = new ServiceController("ScaleWindowsService"))
            {
                try
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);

                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                    timer1.Enabled = true;
                    progressBar1.Show();
                    if (StatusTextBox.Text != null)
                    {
                        StatusTextBox.Clear();
                    }
                    StatusTextBox.Text += ServiceControllerStatus.Running.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Can not restart the Windows Service {service}", ex);
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value += 2;
            if (progressBar1.Value > 99) {
                timer1.Enabled = false;
                progressBar1.Value = 0;
                progressBar1.Hide();
            }
        }
    }
}
