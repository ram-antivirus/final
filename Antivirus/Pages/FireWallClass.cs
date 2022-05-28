using Antivirus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApplication1MahApp
{
    public class FireWallClass
    {
        public readonly MainWindow MainWindowControl;
        MainWindow MainWindowObj = new MainWindow();
        private Antivirus.Pages.setting setting;

        public FireWallClass(MainWindow Board)
         {
            this.MainWindowControl = Board;
         }

        public FireWallClass(Antivirus.Pages.setting setting)
        {
            // TODO: Complete member initialization
            this.setting = setting;
        }

        public void focused()
        {
            if (this.getCheck())
            {
                 MessageBox.Show("Enabled");
               
             //  MainWindowControl.ToggleSwitchBtn2.IsChecked = true;
            }
            else
            {
                 MessageBox.Show("Not Enabled");
               
               // MainWindowControl.ToggleSwitchBtn2.IsChecked = false;
            }
        }
        public void  Checked()
        {
            if (!this.getCheck())
            {
                try
                {
                    Process proc1 = new Process();
                    Process proc2 = new Process();

                    string top = "netsh.exe";
                    proc1.StartInfo.Arguments = "firewall set opmode mode=enable exceptions=disable profile=all";
                    proc1.StartInfo.FileName = top;
                    proc1.StartInfo.UseShellExecute = false;
                    proc1.StartInfo.RedirectStandardOutput = true;
                    proc1.StartInfo.CreateNoWindow = true;
                    proc1.Start();
                    proc1.WaitForExit();

                    proc2.StartInfo.Arguments = "firewall set opmode mode=enable exceptions=disable profile=current";
                    proc2.StartInfo.FileName = top;
                    proc2.StartInfo.UseShellExecute = false;
                    proc2.StartInfo.RedirectStandardOutput = true;
                    proc2.StartInfo.CreateNoWindow = true;
                    proc2.Start();
                    proc2.WaitForExit();
                   

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in Toggle Switch btn2 enabled");
                }
            }
        }
        public void UnChecked()
        {
            if (this.getCheck())
            {
                try
                {
                    Process proc1 = new Process();
                    Process proc2 = new Process();

                    string top = "netsh.exe";
                    proc1.StartInfo.Arguments = "firewall set opmode mode=disable exceptions=disable profile=all";
                    proc1.StartInfo.FileName = top;
                    proc1.StartInfo.UseShellExecute = false;
                    proc1.StartInfo.RedirectStandardOutput = true;
                    proc1.StartInfo.CreateNoWindow = true;
                    proc1.Start();
                    proc1.WaitForExit();

                    proc2.StartInfo.Arguments = "firewall set opmode mode=disable exceptions=disable profile=current";
                    proc2.StartInfo.FileName = top;
                    proc2.StartInfo.UseShellExecute = false;
                    proc2.StartInfo.RedirectStandardOutput = true;
                    proc2.StartInfo.CreateNoWindow = true;
                    proc2.Start();
                    proc2.WaitForExit();
                  

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in Toggle Switch btn2 disabled");
                }
            }
        }
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

            // Check the status of the firewall.
            //  MessageBox.Show("The Domain firewall is turned on: " + CheckDomain + "\nThe Private firewall is turned on: " + CheckPrivate + "\nThe Public firewall is turned on: " + CheckPublic);
            return (CheckDomain && CheckPrivate && CheckPublic);
        }
    }
}
