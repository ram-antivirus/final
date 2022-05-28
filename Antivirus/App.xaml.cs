using AccessFile;
using IWshRuntimeLibrary;
using Antivirus.Pages;
using ContractLibrary;
using FirstFloor.ModernUI.Windows.Controls;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.ApplicationServices;
using MS.WindowsAPICodePack.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Shell32;
using System.Windows.Forms;
using System.ServiceModel;

namespace Antivirus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public TaskbarIcon notifyIcon;
        public static String ashu = "";
        public string b;
       

        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceController service = new ServiceController("ClamD");

            try
            {
                if ((service.Status.Equals(ServiceControllerStatus.Stopped)) ||

                      (service.Status.Equals(ServiceControllerStatus.StopPending)))

                    service.Start();

            }
            catch (Exception err)
            {

                Console.WriteLine(err.Message);
            }
           

            // write reg to exclude path from windows defender 
            try
            {
                foreach (Process proc in Process.GetProcessesByName("SystemTray"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            //try
            //{
            //    using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows Defender\\Exclusions\\Paths", true))
            //    {
            //        key.SetValue(Directory.GetCurrentDirectory(), 0);

            //    }
            //}
            //catch (Exception kjkji) { }
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            notifyIcon.TrayBalloonTipClicked += new RoutedEventHandler(balloonclicked);


            //Read RegName registry to check if it is a first Instance
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\YippeeTech");
                b = (string)registryKey.GetValue("RegName");

            }
            catch (Exception k) { }


            //Execute if First Instance
            #region
            if (b == "T")
            {
                try
                {
                    
                    //Put WindowsServiceUpdate on Startup

                    Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "StartupFolderShortcut.exe");//to add in startup to show popup
                    Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "SystemTray.exe");
                    Antivirus.Properties.Settings.Default.TotalScanned = 0;
                    Antivirus.Properties.Settings.Default.Totalssues = 0;
                    Antivirus.Properties.Settings.Default.TotScanHome = 0;
                    StreamReader reader = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\accountultimate.abc");
                    var accountJSON = reader.ReadToEnd();

                    Account account = JsonConvert.DeserializeObject<Account>(accountJSON);
                    Antivirus.Properties.Settings.Default.Name = account.Name;
                    Antivirus.Properties.Settings.Default.Email = account.Email;

                    Antivirus.Properties.Settings.Default.Activation = account.activation_date;
                    Antivirus.Properties.Settings.Default.Expiration = account.exp_date;

                    double subType = (Antivirus.Properties.Settings.Default.Expiration - Antivirus.Properties.Settings.Default.Activation).TotalDays;
                    if (subType < 32)
                    {
                        Antivirus.Properties.Settings.Default.SubType = "Trial";

                    }
                    else
                    {
                        Antivirus.Properties.Settings.Default.SubType = "Yearly";

                    }

                    try
                    {
                        RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet", false);
                        foreach (string subKeyName in regKey.GetSubKeyNames())
                        {
                            if (subKeyName == "Google Chrome")
                            {
                                TaskbarIcon balloon = new TaskbarIcon();
                                try
                                {
                                    System.Diagnostics.Process[] procchrome = System.Diagnostics.Process.GetProcessesByName("chrome");
                                    foreach (System.Diagnostics.Process proc in procchrome)
                                    {
                                        proc.Kill();
                                    }
                                    System.IO.File.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data\Default\Cookies");
                                    System.IO.File.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data\Default\History");
                                    addChromeExtension();
                                    Process.Start("chrome");
                                }
                                catch (Exception kk) { }
                            }
                        }

                    }
                    catch (Exception j) { }

                    //Setting Last Open Date on First Instance.

                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\SubscriptionAntivirus", true))
                    {
                        key.SetValue("Last Open Date", DateTime.Now);
                        key.SetValue("Expiration Date", account.exp_date);
                        key.SetValue("Install Date", account.activation_date);
                    }
                }
                catch (Exception exx)
                {

                }

                finally
                {
                    Antivirus.Properties.Settings.Default.FirstTime = "no";
                    Antivirus.Properties.Settings.Default.Save();
                }

                try
                {
                    // registry entry fro right click
                    using (RegistryKey key = Registry.ClassesRoot.CreateSubKey("Directory\\shell\\Scan With RAM\\command"))
                    {
                        key.SetValue("", System.AppDomain.CurrentDomain.BaseDirectory + "Antivirus.exe %1");
                    }
                    using (RegistryKey key = Registry.ClassesRoot.CreateSubKey("Directory\\shell\\Scan With RAM"))
                    {
                        key.SetValue("Icon", System.AppDomain.CurrentDomain.BaseDirectory + "logo.ico");
                    }
                    //For File

                    using (RegistryKey key = Registry.ClassesRoot.CreateSubKey("*\\shell\\Scan With RAM\\command"))
                    {
                        key.SetValue("", System.AppDomain.CurrentDomain.BaseDirectory + "Antivirus.exe %1");
                    }
                    using (RegistryKey key = Registry.ClassesRoot.CreateSubKey("*\\shell\\Scan With RAM"))
                    {
                        key.SetValue("Icon", System.AppDomain.CurrentDomain.BaseDirectory + "logo.ico");
                    }
                }
                catch (Exception y)
                { }
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\YippeeTech", true))
                    {
                        key.SetValue("RegName", "F");

                    }
                }
                catch (Exception kjkji) { }

                try
                {
                    Restart r = new Restart();
                    r.txt.Text = "Your Computer must restart in order to complete the installation of application and software updates. If you are ready to restart, save your changes before clicking restart now. If you want to delay the restart click on restart later.";
                    r.Show();
                    r.Activate();
                    r.Topmost = true;
                }

                catch (Exception l) { }




            }

            #endregion
            #region  Subcription region
            try
            {

                using (RegistryKey k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\SubscriptionAntivirus"))
                {
                    var s = k.GetValue("Last Open Date").ToString();
                    DateTime d = DateTime.Parse(s);
                    if (DateTime.Compare(d, DateTime.Now) > 0)
                    {
                        System.Windows.MessageBox.Show("You are trying to cheat please change the system date");
                        System.Windows.Application.Current.Shutdown();
                        return;
                    }
                    else
                    {

                        // MessageBox.Show("Honest");

                        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\SubscriptionAntivirus", true);
                        key.SetValue("Last Open Date", DateTime.Now);
                    }
                    try
                    {
                        using (RegistryKey kk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\SubscriptionAntivirus"))
                        {
                            var sk = kk.GetValue("Expiration Date").ToString();
                            DateTime dd = DateTime.Parse(sk);
                            if (DateTime.Now >= dd)
                            {
                                Antivirus.Properties.Settings.Default.expSub = false;
                                //  MessageBox.Show("Your Subscription is expired please purchase a new Subscription");

                            }

                        }

                    }
                    catch (Exception l) { }
                }

                #endregion

                try
                {
                    Antivirus.Properties.Settings.Default.FancyPop = (Antivirus.Properties.Settings.Default.Expiration - DateTime.Now).TotalDays;
                    if (Antivirus.Properties.Settings.Default.FancyPop < 7 && Antivirus.Properties.Settings.Default.FancyPop > 0)
                    {
                        TaskbarIcon balloon = new TaskbarIcon();
                        FancyPopupSubscription fb = new FancyPopupSubscription();
                        fb.Subs.Text = "Your Subscription Ends in " + (Int32)Antivirus.Properties.Settings.Default.FancyPop + " Please Activate";
                        balloon.ShowCustomBalloon(fb, PopupAnimation.Slide, 10000);

                    }
                }
                catch (Exception k)
                {
                    // MessageBox.Show("Error Process startup-->>"+k);
                }
                try
                {
                    base.OnStartup(e);
                    if (e.Args[0] != null)
                    {
                        ashu = e.Args[0];
                        //  MessageBox.Show("starts...." + e.Args[0]);

                    }
                    else
                    {
                        //if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
                        //{
                        //    MessageBox.Show("Another instance is running");
                        //    Application.Current.Shutdown();
                        //}
                    }
                }
                catch (Exception k)
                {

                }
                notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

                notifyIcon.TrayBalloonTipClicked += new RoutedEventHandler(balloonclicked);



                //   GC.KeepAlive(mutex);
            }
            catch (Exception fg)
            {
                //MessageBox.Show(fg.ToString());
            }


            try
            {
                string[] lines = { "TCPSocket 3310", "MaxThreads 10", "StreamMaxLength 5M", "ScanArchive 0", "MaxFileSize 5M", "LogFile " + AppDomain.CurrentDomain.BaseDirectory + "clamd.log", "DatabaseDirectory " + AppDomain.CurrentDomain.BaseDirectory + "db" };
                if (!System.IO.File.Exists("clamd.conf"))
                {
                    Console.WriteLine("creating");
                    using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "clamd.conf"))
                    {
                        foreach (string line in lines)
                        {
                            // If the line doesn't contain the word 'Second', write the line to the file.
                            Console.WriteLine(line);
                            file.WriteLine(line);
                        }
                    }
                }

                //installing and starting clamd service, if not already
                ServiceController[] services = ServiceController.GetServices();
                var service1 = services.FirstOrDefault(s => s.ServiceName == "ClamD");
                if (service1 != null)
                    Console.WriteLine(service1);
                else
                {
                    Process p = new Process();
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\clamd.exe";
                    p.StartInfo.Arguments = "--install";
                    p.Start();
                    p.WaitForExit();
                    
                    //services = ServiceController.GetServices();
                    service = services.FirstOrDefault(s => s.ServiceName == "ClamD");
                    //service.Start();
                    //service.DisplayName = "Ram Antivirus Service";
                    //service.ServiceName = "Ram Antivirus Service";

                }



                switch (service.Status)
                {
                    case ServiceControllerStatus.Stopped:
                        service.Start();
                        break;
                }
            }
            catch (Exception jhyu)
            {
            }

         
            
            ServiceHost serviceHost;
            ServiceHost serviceUsb;
            try
            {
                string address = "net.pipe://localhost/Techesper/NotifyIcon";
                serviceHost = new ServiceHost(typeof(ClientEndPoint));
                NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
                serviceHost.AddServiceEndpoint(typeof(IDesktopAppContract), binding, address);
                serviceHost.Open();
                Task.Delay(2000).Wait();
                Console.WriteLine(serviceHost.State);
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Notifiicon excep" + ex.InnerException);
            }
            try
            {
                string address = "net.pipe://localhost/Techesper/NotifyIconUsb";
                serviceHost = new ServiceHost(typeof(ClientEndPoint));
                NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
                serviceHost.AddServiceEndpoint(typeof(IDesktopAppContract), binding, address);
                serviceHost.Open();
                Task.Delay(2000).Wait();
                Console.WriteLine(serviceHost.State);
            }
            catch (Exception a)
            {
            }
        }

        void addChromeExtension()
        {

            try
            {
                if (Environment.Is64BitOperatingSystem) //if 64-bit PC
                {
                    //MessageBox.Show("first");
                    RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Google\Chrome\Extensions", true);

                    rkey.CreateSubKey("glphfgnkljdeaogdmokbldckgmndddkd");
                    rkey = rkey.OpenSubKey("glphfgnkljdeaogdmokbldckgmndddkd", true);

                    rkey.SetValue("update_url", "https://clients2.google.com/service/update2/crx");
                }
                else //if 32-bit
                {
                    RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Google\Chrome\Extensions", true);

                    rkey.CreateSubKey("glphfgnkljdeaogdmokbldckgmndddkd");
                    rkey = rkey.OpenSubKey("glphfgnkljdeaogdmokbldckgmndddkd", true);

                    rkey.SetValue("update_url", "https://clients2.google.com/service/update2/crx");
                }
            }
            catch (Exception lk)
            { }
        }


        private void balloonclicked(object sender, EventArgs e)
        {
            //  MessageBox.Show("Bye Bye :*");
        }


       
        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            //sched.Shutdown();
            base.OnExit(e);
        }
    }

}


