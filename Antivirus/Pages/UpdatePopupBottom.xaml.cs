using MahApps.Metro.Controls;
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
using System.Windows.Threading;

namespace Antivirus.Pages
{
    /// <summary>
    /// Interaction logic for UpdatePopupBottom.xaml
    /// </summary>
    public partial class UpdatePopupBottom : MetroWindow
    {
        private DispatcherTimer dispatcherTimer;
        public UpdatePopupBottom()
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
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Things which happen after 1 timer interval
            // MessageBox.Show("Show some data");
            this.Close();

            //Disable the timer
            dispatcherTimer.IsEnabled = false;
        }

       
    }
}
