using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
    /// Interaction logic for browser.xaml
    /// </summary>
    public partial class browser : UserControl
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow); const int SW_HIDE = 0; const int SW_SHOW = 5;
        string pathinetcookie = "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Microsoft\\Windows\\INetCookies";
        string pathchromecookie = "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Local Storage";
        string pathchromecache = "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Cache";
        string inetcache = "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Microsoft\\Internet Explorer\\DOMStore";
        string mozilacookies = "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Mozilla\\Firefox\\Profiles\\j1qf14w6.default";
        int i, j, k, l, m = 0;
        public browser()
        {
            InitializeComponent();

            string softwareRegLoc = @"SOFTWARE\Clients\StartMenuInternet";
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(softwareRegLoc, false);
            try
            {

                foreach (string subKeyName in regKey.GetSubKeyNames())
                {
                    // Open Registry Sub Key
                    if (subKeyName == "Firefox-308046B0AF4A39CB")
                    {
                        tlmozila.IsEnabled = true;
                        chkfire.IsEnabled = true;
                        chkfire.IsChecked = true;
                    }
                    if (subKeyName == "IEXPLORE.EXE")
                    { tlinet.IsEnabled = true; chkinet.IsEnabled = true; chkinet.IsChecked = true; }
                    if (subKeyName == "Google Chrome")
                    { tlchrome.IsEnabled = true; chkchrome.IsEnabled = true; chkchrome.IsChecked = true; }



                }
                string name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                               select x.GetPropertyValue("Caption")).FirstOrDefault().ToString();
                if (name.Contains("Microsoft Windows 10"))
                {
                    chkedge.IsEnabled = true;
                    chkedge.IsChecked = true;
                    tledhe.IsEnabled = true;
                }
            }
            catch (Exception)
            { }




        }

        private void Tile_Click_1(object sender, RoutedEventArgs e)
        {


        }

        private void tlinet_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tlchrome_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lstbrowser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //var a = new ModernDialog
            //{

            //    Content = "This will close the selected browsers",
            //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
            //};
            //a.Buttons = new System.Windows.Controls.Button[] { a.OkButton, a.CancelButton };
            //a.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
            //a.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
            //a.CancelButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
            //a.CancelButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
            //a.ShowDialog();



            if (chkchrome.IsChecked == false && chkedge.IsChecked == false && chkinet.IsChecked == false && chkfire.IsChecked == false)
            {
                MyPopup m = new MyPopup();
                m.txt.Text = "No Option Selected";
                m.ShowDialog();
            }
            else
            {
                MyPopup d = new MyPopup();
                d.txt.Text = "This will close the selected browsers";
                d.ShowDialog();
                pbring.IsActive = true;
                // tbclean.Text = "";



                // pbring.IsActive = true;


                if (chkchrome.IsChecked == true && his.IsChecked == true)
                {

                    Process[] AllProcesses = Process.GetProcesses();
                    foreach (var process in AllProcesses)
                    {
                        if (process.MainWindowTitle != "")
                        {
                            string s = process.ProcessName.ToLower();
                            if (s == "chrome")
                                process.Kill();
                        }
                    }
                    //try
                    //{
                    //    System.Diagnostics.Process[] procchrome = System.Diagnostics.Process.GetProcessesByName("chrome");
                    //    foreach (System.Diagnostics.Process proc in procchrome)
                    //        proc.Kill();
                    //}
                    //catch(Exception h)
                    //{}
                    Thread.Sleep(2500);
                    try
                    {
                        DeleteDirectory(pathchromecache);
                        File.SetAttributes(@"C:\Users\" + Environment.UserName + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\History", FileAttributes.System);

                        File.Delete(@"C:\Users\" + Environment.UserName + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\History");
                    }
                    catch (Exception jk)
                    { }


                    try
                    {
                        DeleteDirectory(pathchromecookie);
                        Thread.Sleep(2500);
                    }
                    catch (Exception hj)
                    { //MessageBox.Show("" + hj); 

                    }
                }
                if (chkinet.IsChecked == true)
                {
                    try
                    {
                        Process[] AllProcesses = Process.GetProcesses();
                        foreach (var process in AllProcesses)
                        {
                            if (process.MainWindowTitle != "")
                            {
                                string s = process.ProcessName.ToLower();
                                if (s == "iexplore")
                                    process.Kill();
                            }
                        }
                    }
                    catch (Exception h)
                    {

                    }
                    try
                    {
                        DeleteDirectory(pathinetcookie);
                    }
                    catch (Exception hj)
                    { //MessageBox.Show("" + hj); 

                    }
                    try
                    {
                        DeleteDirectory(inetcache);
                    }
                    catch (Exception hj)
                    { //MessageBox.Show("" + hj); 

                    }
                }
                if (chkedge.IsChecked == true && his.IsChecked == true)
                {
                    try
                    {
                        Process[] AllProcesses = Process.GetProcesses();
                        foreach (var process in AllProcesses)
                        {
                            if (process.MainWindowTitle != "")
                            {
                                string s = process.ProcessName;
                                if (s == "MicrosoftEdge")
                                    process.Kill();
                            }
                        }
                    }
                    catch (Exception h)
                    {

                    }
                    try
                    {


                        var path = Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%\Packages\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\AC\");
                        var dirs = Directory.GetDirectories(path, "#!*").ToList();
                        Parallel.ForEach(dirs, (dir) =>
                        {
                            try
                            {
                                Directory.Delete(dir, true);
                            }
                            catch { }
                        });
                    }
                    catch (Exception hj)
                    { }
                   
                   
                }
                if (chkfire.IsChecked == true)
                {
                    System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName("Firefox");
                    try
                    {
                        Process[] AllProcesses = Process.GetProcesses();
                        foreach (var process in AllProcesses)
                        {
                            if (process.MainWindowTitle != "")
                            {
                                string s = process.ProcessName.ToLower();
                                if (s == "Firefox")
                                    process.Kill();
                            }
                        }
                    }
                    catch (Exception h)
                    {

                    }
                    try
                    {
                        DeleteDirectory(mozilacookies);
                    }
                    catch (Exception hj)
                    { //MessageBox.Show("" + hj); 

                    }
                }



                Thread th = new Thread(filecountmethod);
                th.Start();
            }


        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {

                if (chkchrome.IsChecked == false && chkedge.IsChecked == false && chkinet.IsChecked == false && chkfire.IsChecked == false)
                {
                    MyPopup m = new MyPopup();
                    m.txt.Text = "No Option Selected";
                    m.ShowDialog();
                }
                else
                {
                    chkchrome.Visibility = Visibility.Visible;
                    chkfire.Visibility = Visibility.Visible;
                    chkinet.Visibility = Visibility.Visible;

                    if (chkinet.IsChecked == true)
                        Process.Start("inetcpl.cpl");
                    if (chkchrome.IsChecked == true)
                    {
                        string path = @"/C chrome://settings/resetProfileSettings";
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
                    if (chkfire.IsChecked == true)
                    {
                        try
                        {
                            string path = @"/C firefox.exe -safe-mode";
                            Process process = new Process();
                            ProcessStartInfo startInfo = new ProcessStartInfo(path);
                            startInfo.Verb = "runas";
                            startInfo.UseShellExecute = false;
                            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            process.StartInfo.RedirectStandardInput = false;
                            process.StartInfo.FileName = @"Firefox.exe";
                            process.StartInfo.Arguments = string.Format(path);

                            //WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent()); bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator); 
                            process.Start();
                            process.StartInfo.RedirectStandardOutput = false;
                            process.StartInfo.RedirectStandardInput = true;
                        }
                        catch (Exception)
                        { }


                    }

                    if (chkedge.IsChecked == true)
                    {
                        //  MessageBox.Show("checked");
                        
                    }
                }
            }
            catch (Exception k)
            { }
        }

        //text animate thread 
        private void filecountmethod()
        {
            int i = 0;


            try
            {
                while (i <= 100)
                {
                    Thread.Sleep((5000));
                    this.Dispatcher.Invoke(delegate
                    {
                        pbring.IsActive = false;



                    });

                }
                MyPopup m = new MyPopup();
                m.txt.Text = "Successfully Cleaned";
                m.ShowDialog();
            }

            catch (Exception)
            {// MessageBox.Show("" + o); 
            }

        }

        private void chkfire_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

    }
}
