using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using WpfApplication1MahApp;
using NATUPNPLib;
using NETCONLib;
using NetFwTypeLib;
using System.Windows.Threading;
using System.ServiceProcess;
using Microsoft.Win32;
using FirstFloor.ModernUI.Presentation;

namespace Antivirus.Pages
{
    /// <summary>
    /// Interaction logic for setting.xaml
    /// </summary>
    public partial class setting : UserControl
    {
        public setting()
        {
            InitializeComponent();
            if (!getCheck())
            {
                tofire.IsChecked = false;
            }
            else if (getCheck())
            {
                tofire.IsChecked = true;
            }

        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        public static bool getCheck()
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

        private void toreal_IsCheckedChanged(object sender, EventArgs e)
        {
            ServiceController[] services = ServiceController.GetServices();
            var service = services.FirstOrDefault(s => s.ServiceName == "Real Time Scan");


            // Show At Risk Status





            if ((bool)toreal.IsChecked)
            {

                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    service.Start();
                    Antivirus.Properties.Settings.Default.Color = System.Windows.Media.Color.FromRgb(0, 122, 204);
                    Antivirus.Properties.Settings.Default.Status = "Secure";
                    Antivirus.Properties.Settings.Default.StatusColor = System.Windows.Media.Color.FromRgb(48, 138, 15);
                    Antivirus.Properties.Settings.Default.Img = true;
                    Antivirus.Properties.Settings.Default.atrisk = 1;

                }
            }
            else
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    Antivirus.Properties.Settings.Default.Color = System.Windows.Media.Color.FromRgb(0, 122, 204);
                    //Antivirus.Properties.Settings.Default.StatusColor = System.Windows.Media.Color.FromRgb(255, 0, 0);
                    Antivirus.Properties.Settings.Default.Status = "Secure";
                    Antivirus.Properties.Settings.Default.StatusColor = System.Windows.Media.Color.FromRgb(48, 138, 15);
                    Antivirus.Properties.Settings.Default.Img = false;
                    Antivirus.Properties.Settings.Default.atrisk = 2;
                    Antivirus.Properties.Settings.Default.Save();
                    PopUp p = new PopUp();
                  
                    p.txt.Text = "By turning real time protection off, you sacrifice system security. We recommend to turn on real time protection for security purpose.";


                    p.ShowDialog();
                }
            }
            if (!(bool)toreal.IsChecked || !(bool)tofire.IsChecked || !(bool)anti.IsChecked || !(bool)ransome.IsChecked)
            {

                Antivirus.Properties.Settings.Default.Color = System.Windows.Media.Color.FromRgb(0, 122, 204);
                Antivirus.Properties.Settings.Default.Status = "At Risk";
                Antivirus.Properties.Settings.Default.StatusColor = System.Windows.Media.Color.FromRgb(255, 0, 0);
                Antivirus.Properties.Settings.Default.Img = true;
                Antivirus.Properties.Settings.Default.atrisk = 3;




            }


        }

        private void tofire_IsCheckedChanged(object sender, EventArgs e)
        {
            Process proc1 = new Process();
            proc1.StartInfo.FileName = "netsh.exe";
            proc1.StartInfo.UseShellExecute = false;
            proc1.StartInfo.RedirectStandardOutput = true;
            proc1.StartInfo.CreateNoWindow = true;

            if ((bool)tofire.IsChecked)
            {
                if (!getCheck())
                {
                    proc1.StartInfo.Arguments = "advfirewall set AllProfiles state on";
                    proc1.Start();
                    proc1.WaitForExit();
                }
            }
            else
            {
                if (getCheck())
                {
                    proc1.StartInfo.Arguments = "advfirewall set AllProfiles state off";
                    proc1.Start();
                    proc1.WaitForExit();
                    //AppearanceManager.Current.AccentColor = Colors.Green;
                    //ModernWindow1 f = new ModernWindow1();
                    //f.txt.Text = "ashu";
                    ////f.Buttons = new System.Windows.Controls.Button[] { };
                    //f.Show();
                    PopUp p = new PopUp();
                    p.txt.Text = "You are trying to turn off firewall which is not recommended. Please turn on firewall protection.";
                    p.ShowDialog();


                }
            }
            if (!(bool)toreal.IsChecked || !(bool)tofire.IsChecked || !(bool)anti.IsChecked || !(bool)ransome.IsChecked)
            {

                Antivirus.Properties.Settings.Default.Color = System.Windows.Media.Color.FromRgb(0, 122, 204);
                Antivirus.Properties.Settings.Default.Status = "At Risk";
                Antivirus.Properties.Settings.Default.StatusColor = System.Windows.Media.Color.FromRgb(255, 69, 0);
                Antivirus.Properties.Settings.Default.Img = true;
                Antivirus.Properties.Settings.Default.atrisk = 3;


            }
        }


        private void SettingsControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!getCheck())
            {
                tofire.IsChecked = false;
            }
            else if (getCheck())
            {
                tofire.IsChecked = true;
            }
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                var service = services.FirstOrDefault(s => s.ServiceName == "Real Time Scan");
                if (service.Status == ServiceControllerStatus.Running)
                    this.toreal.IsChecked = true;
                else
                {
                    this.toreal.IsChecked = false;

                }
                this.tofire.IsChecked = getCheck();
            }
            catch (Exception ex)
            {
            }


        }

        private void webadv_IsCheckedChanged(object sender, EventArgs e)
        {
            if (webadv.IsChecked == false)
            {
                try
                {
                    if (Environment.Is64BitOperatingSystem) //if 64-bit PC
                    {
                        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Google\Chrome\Extensions", true);

                        key.DeleteSubKey("onjjgnoeofkonifcjelacffcfgdhhdbb");
                        PopUp p = new PopUp();
                        p.txt.Text = "Web Advisor is an extra security measures that help prevent unauthorized third-party activity while you're surfing the web. We recommend that you do not change these settings.";
                        p.ShowDialog();
                    }
                    else
                    {

                        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Google\Chrome\Extensions", true);

                        key.DeleteSubKey("onjjgnoeofkonifcjelacffcfgdhhdbb");
                        PopUp p = new PopUp();
                        p.txt.Text = "Web Advisor is an extra security measures that help prevent unauthorized third-party activity while you're surfing the web. We recommend that you do not change these settings.";
                        p.ShowDialog();
                    }
                }
                catch (Exception d)
                {
                }
            }
            else if (webadv.IsChecked == true)
            {

                if (Environment.Is64BitOperatingSystem) //if 64-bit PC
                {
                    //MessageBox.Show("first");
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Google\Chrome\Extensions", true);

                    key.CreateSubKey("onjjgnoeofkonifcjelacffcfgdhhdbb");
                    key = key.OpenSubKey("onjjgnoeofkonifcjelacffcfgdhhdbb", true);

                    key.SetValue("update_url", "https://clients2.google.com/service/update2/crx");
                }
                else //if 32-bit
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Google\Chrome\Extensions", true);

                    key.CreateSubKey("onjjgnoeofkonifcjelacffcfgdhhdbb");
                    key = key.OpenSubKey("onjjgnoeofkonifcjelacffcfgdhhdbb", true);

                    key.SetValue("update_url", "https://clients2.google.com/service/update2/crx");
                }
            }
        }

        private void intru_IsCheckedChanged(object sender, EventArgs e)
        {
            if ((bool)intru.IsChecked)
            {
                Microsoft.Win32.Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR", "Start", 3, Microsoft.Win32.RegistryValueKind.DWord);
            }
            else if (!(bool)intru.IsChecked)
            {
                Microsoft.Win32.Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR", "Start", 4, Microsoft.Win32.RegistryValueKind.DWord);
                PopUp p = new PopUp();
                p.txt.Text = "USB protection feature provides the best protection against any threats when using USB drives. We urge you to kindly turn this feature ON for better protection.";
                p.ShowDialog();
            }
        }



        private void tostartup_IsCheckedChanged(object sender, EventArgs e)
        {
            if ((bool)tostartup.IsChecked)
            {

            }
            else if (!(bool)tostartup.IsChecked)
            {
                PopUp p = new PopUp();
                p.txt.Text = "The advanced settings help you to keep your computer secure. We recommend that you do not change these settings.";


                p.ShowDialog();

            }
        }

        private void ransome_IsCheckedChanged(object sender, EventArgs e)
        {
            if (!(bool)ransome.IsChecked)
            {
                Antivirus.Properties.Settings.Default.randsome = false;
                PopUp p = new PopUp();
                p.txt.Text = "Cloud based process monitoring observes malicious processes at real time and prevent them from executing thus protecting your computer. Kindly turn this feature ON.";
                p.ShowDialog();
            }
            else if ((bool)ransome.IsChecked)
            {
                Antivirus.Properties.Settings.Default.randsome = true;
            }
        }
        private void emailscn_IsCheckedChanged(object sender, EventArgs e)
        {
            if (!(bool)emailscn.IsChecked)
            {

                PopUp p = new PopUp();
                p.txt.Text = "Data-Backup feature allows data protection and prevent loss of your important files by allowing you to take regular backup with the help of ram expert on Ram cloud. Kindly turn ON this feature.";
                p.ShowDialog();
            }
            else if ((bool)emailscn.IsChecked)
            {

            }
        }

        private void emailscn_Copy_IsCheckedChanged(object sender, EventArgs e)
        {
            if (!(bool)emailscn_Copy.IsChecked)
            {

                PopUp p = new PopUp();
                p.txt.Text = "Ram Antivirus safe search chrome extension keeps you away from malicious and phishing websites we recommend you to turn on and enable the extension for advanced level web protection ";
                p.ShowDialog();
            }
            else if ((bool)emailscn_Copy.IsChecked)
            {

            }
        }
        private void emailscn_Copy1_IsCheckedChanged(object sender, EventArgs e)
        {
            if (!(bool)emailscn_Copy1.IsChecked)
            {

                PopUp p = new PopUp();
                p.txt.Text = "Browser protection feature protects your data and privacy from cyber criminals who monitor your online behaviour and collect your personal data to misuse it. We recommend you to turn ON this feature.";
                p.ShowDialog();
            }
            else if ((bool)emailscn_Copy1.IsChecked)
            {

            }
        }


        private void anti_IsCheckedChanged(object sender, EventArgs e)
        {
            if ((bool)anti.IsChecked)
            {
                Antivirus.Properties.Settings.Default.antifishing = true;
               
            }
            else if (!(bool)anti.IsChecked)
            {
                Antivirus.Properties.Settings.Default.antifishing = false;
                PopUp p = new PopUp();
                p.txt.Text = "Ram Antivirus anti-phishing feature blocks Phishing and malicious websites which are potentially harmful for your computer. We urge you to kindly turn this feature ON for better protection.";
                p.ShowDialog();
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void intru1_IsCheckedChanged(object sender, EventArgs e)
        {
            if (!(bool)selpf.IsChecked)
            {

                PopUp p = new PopUp();
                p.txt.Text = "Ram Antivirus helps against real-time ransomware samples and clean-up all traces of ransomware using cloud-based process monitoring. We suggest you to turn on this feature";
                p.ShowDialog();
            }
            else if ((bool)selpf.IsChecked)
            {

            }

        }

        private void mal_IsCheckedChanged(object sender, EventArgs e)
        {
            if (!(bool)maleware.IsChecked)
            {

                PopUp p = new PopUp();
                p.txt.Text = "Ram Antivirus anti-malware feature acts as an advanced layer protective tool against potentially harmful malwares by providing zero-day malware protection. We advise you to turn ON this feature.";
                p.ShowDialog();
            }
            else if ((bool)emailscn_Copy.IsChecked)
            {

            }
        }
    }
}
