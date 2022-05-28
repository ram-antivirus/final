using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ChromeExtension.xaml
    /// </summary>
    public partial class ChromeExtension : UserControl
    {
        private bool isClosing = false;
        public ChromeExtension()
        {
            InitializeComponent();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process[] procchrome = System.Diagnostics.Process.GetProcessesByName("chrome");
                foreach (System.Diagnostics.Process proc in procchrome)
                {
                    proc.Kill();
                }
                File.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data\Default\Cookies");
                File.Delete(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data\Default\History");
                addChromeExtension();
                Process.Start("chrome");
                Popup pp = (Popup)Parent;
                pp.IsOpen = false;
            }
            catch (Exception kk) { }

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Popup pp = (Popup)Parent;
            pp.IsOpen = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Popup pp = (Popup)Parent;
            pp.IsOpen = false;
        }


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
    }
}
