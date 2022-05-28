using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Antivirus.Pages
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : UserControl
    {
        public Report()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            setting ko = new setting();

            try
            {
                ServiceController[] services = ServiceController.GetServices();
                var service = services.FirstOrDefault(s => s.ServiceName == "Real Time Scan");
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    realtxt.Text = "OFF";
                    realtxt.Foreground = Brushes.Red;
                    btnreal.Visibility = Visibility.Visible;
                }
                else if (service.Status == ServiceControllerStatus.Running)
                {
                    realtxt.Text = "ON";
                    realtxt.Foreground = Brushes.Green;
                    btnreal.Visibility = Visibility.Hidden;
                }
            }catch(Exception l){}
            try{
                if (!getCheck())
                {
                    firetxt.Text = "OFF";
                    firetxt.Foreground = Brushes.Red;
                    btnfire.Visibility = Visibility.Visible;
                }
                else if (getCheck())
                {
                    firetxt.Text = "ON";
                    firetxt.Foreground = Brushes.Green;
                    btnfire.Visibility = Visibility.Hidden;
                }
                if (Antivirus.Properties.Settings.Default.antifishing==true)
                {
                    btnAnti.Visibility = Visibility.Hidden;
                    antifishtxt.Text = "ON";
                    antifishtxt.Foreground = Brushes.Green;
                }
                else if (Antivirus.Properties.Settings.Default.antifishing == false)
                {
                    btnAnti.Visibility = Visibility.Visible;
                    antifishtxt.Text = "OFF";
                    antifishtxt.Foreground = Brushes.Red;
                }
                if (Antivirus.Properties.Settings.Default.randsome==false)
                {
                    btnRand.Visibility = Visibility.Visible;
                    Randtxt.Text = "OFF";
                    Randtxt.Foreground = Brushes.Red;
                }
                else if (Antivirus.Properties.Settings.Default.randsome == true)
                {
                    btnRand.Visibility = Visibility.Hidden;
                    Randtxt.Text = "ON";
                    Randtxt.Foreground = Brushes.Green;
                }

                var k = home.GetDirectorySize();
              //  MessageBox.Show(k.ToString());
                if (k >= 500)
                {
                    btnJunk.Visibility = Visibility.Visible;
                    junktxt.Text = "Need Cleanup";
                    junktxt.Foreground = Brushes.Red;

                }
                else {
                    btnJunk.Visibility = Visibility.Hidden;
                    junktxt.Text = "Junks Cleaned";
                    junktxt.Foreground = Brushes.Green;
                }

                
                if (Antivirus.Properties.Settings.Default.updateVersion == true)
                {
                    btnDB.Visibility = Visibility.Visible;
                    dbupdatetxt.Text = "Require Updates";
                    dbupdatetxt.Foreground = Brushes.Red;
                }
                else if (Antivirus.Properties.Settings.Default.updateVersion == false)
                {
                    btnDB.Visibility = Visibility.Hidden;
                    dbupdatetxt.Text = "Up To Date";
                    dbupdatetxt.Foreground = Brushes.Green;
                }
            }
            catch (Exception l)
            { }
        }
        
      



        //firewall

        private bool getCheck()
        {
            // Create consts for firewall types.
            const int NET_FW_PROFILE2_DOMAIN = 1;
            const int NET_FW_PROFILE2_PRIVATE = 2;
            const int NET_FW_PROFILE2_PUBLIC = 4;

            // Create the firewall type.
            Type FWManagerType = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");

            // Use the firewall type to create a firewall manager object.
            dynamic FWManager = Activator.CreateInstance(FWManagerType);

            // Get the firewall settings.
            bool CheckDomain = FWManager.FirewallEnabled(NET_FW_PROFILE2_DOMAIN);
            bool CheckPrivate =
                FWManager.FirewallEnabled(NET_FW_PROFILE2_PRIVATE);
            bool CheckPublic =
                FWManager.FirewallEnabled(NET_FW_PROFILE2_PUBLIC);


            return CheckPrivate;
        }

        public void Firewall()
        {
            Process proc1 = new Process();
            proc1.StartInfo.FileName = "netsh.exe";
            proc1.StartInfo.UseShellExecute = false;
            proc1.StartInfo.RedirectStandardOutput = true;
            proc1.StartInfo.CreateNoWindow = true;


            if (!getCheck())
            {
                firetxt.Text = "OFF";
                firetxt.Foreground = Brushes.Red;

            }
            else {

                firetxt.Text = "ON";
                firetxt.Foreground = Brushes.Green;
            }





        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/setting.xaml", this);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/setting.xaml", this);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/setting.xaml", this);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/setting.xaml", this);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/update.xaml", this);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/pcclean.xaml", this);
        }

        
    }
}
