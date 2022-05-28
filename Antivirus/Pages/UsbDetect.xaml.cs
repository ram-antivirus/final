using FirstFloor.ModernUI.Windows.Controls;
using MahApps.Metro.Controls;
using nClam;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Windows.Threading;


namespace Antivirus.Pages
{
    /// <summary>
    /// Interaction logic for UsbDetect.xaml
    /// </summary>
    public partial class UsbDetect : MetroWindow
    {
        private DispatcherTimer dispatcherTimer;
        public UsbDetect()
        {
            InitializeComponent();
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;


            //Create a timer with interval of 2 secs
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);

            dispatcherTimer.Start(); 
           
            //if (a != null)
            //{
            //    Scan(a);
            //}
            //if (a == null)
            //{
            //    this.Close();
            //}
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Things which happen after 1 timer interval
           // MessageBox.Show("Show some data");
            this.Close();

            //Disable the timer
            dispatcherTimer.IsEnabled = false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
     

        
    }
}
