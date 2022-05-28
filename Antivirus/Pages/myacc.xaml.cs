using AccessFile;
using Hardcodet.Wpf.TaskbarNotification;
using Newtonsoft.Json;
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
using System.Windows.Threading;

namespace Antivirus.Pages
{
    /// <summary>
    /// Interaction logic for aboutus.xaml
    /// </summary>
    public partial class myacc : UserControl
    {
        String SubscriptionType;
        
        public myacc()
        {
           
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();
            

            
        }
        private bool BlinkOn = false;
        
        private void timer_Tick(object sender, EventArgs e)
        {
            if (BlinkOn)
            {
                lblTimer.Foreground = Brushes.Black;
                lblTimer.Background = Brushes.White;
            }
            else
            {
                lblTimer.Foreground = Brushes.Red;
                //       lblTimer.Background = Brushes.Black;
            }
            BlinkOn = !BlinkOn;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamReader reader = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\accountultimate.abc");
                var accountJSON = reader.ReadToEnd();

                Account account = JsonConvert.DeserializeObject<Account>(accountJSON);
                // this.Email.Text = Antivirus.Properties.Settings.Default.Email;
                this.Activation.Text = account.activation_date.ToString();
                this.Expiration.Text = account.exp_date.ToString();
                double c = (Antivirus.Properties.Settings.Default.Expiration - DateTime.Now).TotalDays;
                int x = (Int32)c;
                if (x <= 0)
                {
                    this.daysLeftN.Text = "Expired";
                    this.txtrect.Text = "Subscription Expired! ";
                    rect.Fill = Brushes.Red;


                }
                else
                {
                    this.daysLeftN.Text = x.ToString();
                    Active.Visibility = Visibility.Hidden;
                    lblTimer.Visibility = Visibility.Hidden;
                    CheckSubscription();
                }
                
               

                
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.StackTrace);
            }
        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();
        }
        public void CheckSubscription()
        {

            double subType = (Antivirus.Properties.Settings.Default.Expiration - Antivirus.Properties.Settings.Default.Activation).TotalDays;
          
            
            double c = (Antivirus.Properties.Settings.Default.Expiration - DateTime.Now).TotalDays;
           // MessageBox.Show(c.ToString());
            if ((Antivirus.Properties.Settings.Default.Activation - Antivirus.Properties.Settings.Default.Expiration).TotalDays > 300)
            {
                SubscriptionType = "Yearly";
            }
            else
            {
                SubscriptionType = "Trial";
            }
            if (Antivirus.Properties.Settings.Default.SubType == "Trial")
            {
                if (c < 7 && c > 0)
                {
                    int d = (Int32)c;
                    Active.Visibility = Visibility.Visible;
                    lblTimer.Visibility = Visibility.Visible;
                    string s = "Your subscription ends in" + d + " days please activate to continue support";
                    lblTimer.Content = s;
                }
            }
            else if (Antivirus.Properties.Settings.Default.SubType == "Trial")

            {
                if (c < 7 && c > 0)
                {
                    int d = (Int32)c;
                    Active.Visibility = Visibility.Visible;
                    lblTimer.Visibility = Visibility.Visible;
                    lblTimer.Content = "Your subscription ends in" + d + " days please activate to continue support";
                }
            }

        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Active_Click(object sender, RoutedEventArgs e)
        {
            try {
                Process.Start("https://ramantivirus.com/ram-ultimate-antivirus/");
            }
            catch(Exception j)
            {
                MessageBox.Show("Server is down");
            }
        }

        private void Tile_Click_13(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://ramantivirus.com/contact-us/");
            }
            catch (Exception k)
            {

            }
        }

        private void Tile_Click_12(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://ramantivirus.com");
            }
            catch (Exception k)
            {

            }
        }

        private void Tile_Click_14(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://ramantivirus.com/remote-support/");
            }
            catch (Exception k)
            {

            }
        }

        private void Tile_Click_15(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://ramantivirus.com/contact-us/");
            }
            catch (Exception k)
            {

            }
        }

        private void Tile_Click_11(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://ramantivirus.com/support/");
            }
            catch (Exception k)
            {

            }
        }

        private void Tile_Click_16(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/aboutus.xaml", this);
        }

        private void Tile_Click_10(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://www.facebook.com/Ram-Antivirus-677206782468895/");
            }
            catch (Exception j) { }
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://twitter.com/ramantivirus");
            }
            catch (Exception df) { }
        }
    }

    public class Account
    {
        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                }
            }
        }

        private string email;
        public string Email
        {
            get { return this.email; }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                }
            }
        }

        private DateTime activation;
        public DateTime activation_date
        {
            get { return this.activation; }
            set
            {
                if (this.activation != value)
                {
                    this.activation = value;
                }
            }
        }

        private DateTime expiration;
        public DateTime exp_date
        {
            get { return this.expiration; }
            set
            {
                if (this.expiration != value)
                {
                    this.expiration = value;
                }
            }
        }
    }
}
