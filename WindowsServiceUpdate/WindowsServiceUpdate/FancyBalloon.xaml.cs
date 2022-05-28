using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;
using System.Diagnostics;
using System.IO;
using System.Windows.Threading;
namespace WindowsServiceUpdate
{
    /// <summary>
    /// Interaction logic for FancyBalloon.xaml
    /// </summary>
    public partial class FancyBalloon : UserControl
    {
        private bool isClosing = false;

        
            DispatcherTimer timer = new DispatcherTimer();
        
            
        #region BalloonText dependency property

        /// <summary>
        /// Description
        /// </summary>
        public static readonly DependencyProperty BalloonTextProperty =
            DependencyProperty.Register("BalloonText",
                typeof(string),
                typeof(FancyBalloon),
                new FrameworkPropertyMetadata(""));

        /// <summary>
        /// A property wrapper for the <see cref="BalloonTextProperty"/>
        /// dependency property:<br/>
        /// Description
        /// </summary>
        public string BalloonText
        {
            get { return (string)GetValue(BalloonTextProperty); }
            set { SetValue(BalloonTextProperty, value); }
        }

        #endregion
        public FancyBalloon()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();
        }

        private bool BlinkOn = false;
        private void timer_Tick(object sender, EventArgs e)
        {
            if (BlinkOn)
            {
                PopupText.Foreground = Brushes.White;
              //  PopupText.Background = Brushes.White;
            }
            else
            {
                PopupText.Foreground = Brushes.Red;
               // PopupText.Background = Brushes.Black;
            }
            BlinkOn = !BlinkOn;
        }

            void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            }
        /// <summary>
        /// By subscribing to the <see cref="TaskbarIcon.BalloonClosingEvent"/>
        /// and setting the "Handled" property to true, we suppress the popup
        /// from being closed in order to display the custom fade-out animation.
        /// </summary>
        private void OnBalloonClosing(object sender, RoutedEventArgs e)
        {
            e.Handled = true; //suppresses the popup from being closed immediately
            isClosing = true;
        }


        /// <summary>
        /// Resolves the <see cref="TaskbarIcon"/> that displayed
        /// the balloon and requests a close action.
        /// </summary>
        private void imgClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //the tray icon assigned this attached property to simplify access
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.CloseBalloon();
        }

        /// <summary>
        /// If the users hovers over the balloon, we don't close it.
        /// </summary>
        private void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //if we're already running the fade-out animation, do not interrupt anymore
            //(makes things too complicated for the sample)
            if (isClosing) return;

            //the tray icon assigned this attached property to simplify access
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.ResetBalloonCloseTimer();
        }


        /// <summary>
        /// Closes the popup once the fade-out animation completed.
        /// The animation was triggered in XAML through the attached
        /// BalloonClosing event.
        /// </summary>
        private void OnFadeOutCompleted(object sender, EventArgs e)
        {
            Popup pp = (Popup)Parent;
            pp.IsOpen = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Popup pp = (Popup)Parent;
            pp.IsOpen = false;

        }
       

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (btnResolve.Content == "Resolve")
            {
                try
                {
                    Process.Start("ms-settings:windowsupdate");
                    Popup pp = (Popup)Parent;
                    pp.IsOpen = false;
                }
                catch (Exception)
                { }
            }
            if (btnResolve.Content == "Clean")
            {
                try
                {
                  
                    int count = Directory.GetFiles(MainWindow.unametemp).Length;
                    string[] tempfiles = Directory.GetFiles(MainWindow.unametemp, "*.*", SearchOption.AllDirectories);
                   

                    //MessageBox .Show ("Deleting files from {0}", tempfolder);

                    foreach (string tempfile in tempfiles)
                    {
                        try
                        {
                            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(MainWindow.unametemp);
                            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
                            // directory.Delete(true);
                            File.Delete(tempfile);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("File could not be deleted: {0}", tempfile);
                        }
                    }
                    foreach (string tempfile in MainWindow.filepath)
                    {
                        try
                        {
                            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(@"C:\Windows\Prefetch");
                            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
                            // directory.Delete(true);
                            File.Delete(tempfile);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("File could not be deleted: {0}", tempfile);
                        }
                    }

                    Popup pp = (Popup)Parent;
                    pp.IsOpen = false;
                }
                catch (Exception)
                { }
 
            }
            
            if (btnResolve.Content == "Turn On")
            {
                Process proc1 = new Process();
                proc1.StartInfo.FileName = "netsh.exe";
                proc1.StartInfo.UseShellExecute = false;
                proc1.StartInfo.RedirectStandardOutput = true;
                proc1.StartInfo.CreateNoWindow = true;

                if (!getCheck())
                {
                    proc1.StartInfo.Arguments = "advfirewall set AllProfiles state on";
                    proc1.Start();
                    proc1.WaitForExit();
                }



                try
                {
                    
                    Popup pp = (Popup)Parent;
                    pp.IsOpen = false;

                }
                catch (Exception)
                { }

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


            return CheckPrivate;
        }
    }
}
