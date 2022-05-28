using System;
using System.Collections.Generic;
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
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows.Controls.Primitives;
using WUApiLib;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace WindowsServiceUpdate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string unametemp = "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Temp";
        public static System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(unametemp);
        public static TaskbarIcon notifyIcon;
        public static string[] filepath = Directory.GetFiles(@"C:\Windows\Prefetch");
        public static string[] dirpref = Directory.GetDirectories(@"C:\Windows\Prefetch");
        public static System.IO.DirectoryInfo dir1 = new System.IO.DirectoryInfo(@"C:\Windows\Prefetch");

        public MainWindow()
        {
            InitializeComponent();
           
            //Checking Windows Update in Popup
            Update();
            //Checking Temp Files
            JunkFiles();
            //Firewall Status
            Firewall();

           // MessageBox.Show(System.Reflection.Assembly.GetExecutingAssembly().Location);

           // string s = "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"";
            //MessageBox.Show(System.Reflection.Assembly.GetExecutingAssembly().Location);

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("WinApp","@" + System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            
         }

        public void Update()
        {
            var updateSession = new UpdateSession();
            var updateSearcher = updateSession.CreateUpdateSearcher();
            updateSearcher.Online = false; //set to true if you want to search online
            try
            {
                var searchResult = updateSearcher.Search("IsInstalled=0 And IsHidden=0");
                Console.WriteLine(searchResult.Updates.Count);
                if (searchResult.Updates.Count < 0)
                {
                    TaskbarIcon balloon = new TaskbarIcon();
                    FancyBalloon f = new FancyBalloon();
                    f.PopupText.Text = "Please Update Your Windows";
                    f.btnResolve.Content = "Resolve";
                    f.btnResolve.Visibility = Visibility.Visible;
                    balloon.ShowCustomBalloon(f, PopupAnimation.Slide, 5000);

                }
                else
                {

                }
                this.Hide();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Error");
            }


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
                TaskbarIcon balloon = new TaskbarIcon();
                FancyBalloon f = new FancyBalloon();
                f.PopupText.Text = "Your Firewall is Off.";
                f.btnResolve.Content = "Turn On";
                f.btnResolve.Visibility = Visibility.Visible;
                balloon.ShowCustomBalloon(f, PopupAnimation.Slide, 5000);
                this.Hide();
            }

            this.Hide();




        }

        //Funtion to check Firewall Status

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

        public void JunkFiles()
        {

            int count1 = dir.GetDirectories().Length + dir.GetFiles().Length;
            int count2 = dir1.GetDirectories().Length + dir1.GetFiles().Length;
            //   MessageBox.Show(count1.ToString());
            if (count1 > 1000 || count2 > 1000 )
            {
                TaskbarIcon balloon1 = new TaskbarIcon();
                FancyBalloon f1 = new FancyBalloon();
                f1.PopupText.Text = "You have Junk Files in your System. Please Click below button to clean your pc ";
                f1.btnResolve.Content = "Clean";
                f1.btnResolve.Visibility = Visibility.Visible;
                balloon1.ShowCustomBalloon(f1, PopupAnimation.Slide, 5000);

            }
            
            
            this.Hide();
        }

        

    }
}

