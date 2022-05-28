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

namespace Antivirus.Pages
{
    /// <summary>
    /// Interaction logic for safe.xaml
    /// </summary>
    public partial class safe : UserControl
    {
        public safe()
        {
            InitializeComponent();
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = "http://www.google.com"+ " --incognito";
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(path);
                startInfo.Verb = "runas";
                startInfo.UseShellExecute = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.RedirectStandardInput = false;
                process.StartInfo.FileName = "chrome.exe";
                process.StartInfo.Arguments = string.Format(path);
                startInfo.WorkingDirectory = @"C:\Windows\System32";
                //WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent()); bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator); 
                process.Start();
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardInput = true;

            }
            catch (Exception j)
            { }
        }

        private void Tile_Click_1(object sender, RoutedEventArgs e)
        {
            try {
                string path = @"/C firefox.exe -private-window";
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(path);
                startInfo.Verb = "runas";
                startInfo.UseShellExecute = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.RedirectStandardInput = false;
                process.StartInfo.FileName = "firefox.exe";
                process.StartInfo.Arguments = string.Format(path);
                startInfo.WorkingDirectory = @"C:\Windows\System32";
                //WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent()); bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator); 
                process.Start();
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardInput = true;
                
            }
            catch (Exception l) { }
        }

        private void Tile_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = @"/C iexplore.exe -extoff";
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(path);
                startInfo.Verb = "runas";
                startInfo.UseShellExecute = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.RedirectStandardInput = false;
                process.StartInfo.FileName = "iexplore.exe";
                process.StartInfo.Arguments = string.Format(path);
                startInfo.WorkingDirectory = @"C:\Windows\System32";
                //WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent()); bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator); 
                process.Start();
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardInput = true;
            }

            catch (Exception l)
            { 
               
            }
          
        }
    }
}
