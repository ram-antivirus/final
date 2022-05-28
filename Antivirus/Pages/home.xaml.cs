using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.ServiceProcess;
using WUApiLib;

namespace Antivirus.Pages
{
    /// <summary>
    /// Interaction logic for home.xaml
    /// </summary>
    public partial class home : System.Windows.Controls.UserControl
    {
        string xmlPath;
        string currentPathOfDatabase = AppDomain.CurrentDomain.BaseDirectory + @"\db\";

        public home()
        {
            InitializeComponent();
            //if (Antivirus.Properties.Settings.Default.FancyPop < 0)
            //{
            //    updateTile.IsEnabled = false;
            //}

            if (Antivirus.Properties.Settings.Default.scantile == false)
            {
                tileScan.IsEnabled = false;
            }
            else if (Antivirus.Properties.Settings.Default.scantile == true)
            {
                tileScan.IsEnabled = true;

            }
            if (Antivirus.Properties.Settings.Default.updateavail != 'a')
            {
                if (Antivirus.Properties.Settings.Default.updateavail == 'c')
                {
                    updatetxt.Visibility = Visibility.Hidden;
                }
                else
                    if (Antivirus.Properties.Settings.Default.updateavail == 'b')
                    {
                        updatetxt.Text = "Updating..";
                    }


            }

            getVersion();
            CheckAtRisk();
            checWnkUpdate();

            long b = GetDirectorySize();
            if (b > 1024 || Antivirus.Properties.Settings.Default.updateVersion)
            {
                ristbtn.Visibility = Visibility.Visible;
                ristbtn.Content = "Attention";

            }

            double c = (Antivirus.Properties.Settings.Default.Expiration - DateTime.Now).TotalDays;
            int x = (Int32)c;
            if (x <= 0)
            {
                this.Substype.Text = "Expired";
                
                this.Substype.Foreground =Brushes.Red;
               // updateTile.IsEnabled = false;


            }
            NavigationCommands.GoToPage.Execute("Pages/scan.xaml", this);
            this.TotalFiles.Text = Antivirus.Properties.Settings.Default.TotScanHome.ToString();

        }
        //Check Real Time Service,Firewall,Ransomeware,antiphishing hence set Text Atrisk

        public void CheckAtRisk()
        {
          //  System.Windows.MessageBox.Show("cal at risk");
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                var service = services.FirstOrDefault(s => s.ServiceName == "Real Time Scan");
                if (service.Status == ServiceControllerStatus.Stopped || !getCheck() || Antivirus.Properties.Settings.Default.antifishing == false || Antivirus.Properties.Settings.Default.randsome == false)
                {
                    pcProt.Text = "At Risk";
                    pcProt.Foreground = Brushes.OrangeRed;
                 //   System.Windows.MessageBox.Show("Called From At risk");
                    ristbtn.Visibility = Visibility.Visible;
                    ImageBrush myBrush = new ImageBrush();
                    myBrush.ImageSource =
                        new BitmapImage(new Uri("pack://application:,,,/Images/warning2.png", UriKind.Absolute));
                    grd1.Background = myBrush;

                }
                else if (service.Status == ServiceControllerStatus.Running || getCheck() || Antivirus.Properties.Settings.Default.antifishing == true || Antivirus.Properties.Settings.Default.randsome == true)
                {
                    pcProt.Text = "Secure";
                   // System.Windows.MessageBox.Show("Called From Secure");
                    pcProt.Foreground = Brushes.Green;
                    ristbtn.Visibility = Visibility.Visible;
                    ImageBrush myBrush = new ImageBrush();
                    myBrush.ImageSource =
                        new BitmapImage(new Uri("pack://application:,,,/Images/green-tick.jpg", UriKind.Absolute));
                    grd1.Background = myBrush;

                }

            }
            catch (Exception l)
            {
               // System.Windows.Forms.MessageBox.Show("Exception at " + l); 
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

        public Version getLocalVersion()
        {

            Version localVersion = null;
            // and in this variable we will put the url we  
            // would like to open so that the user can  
            // download the new version  
            // it can be a homepage or a direct  
            // link to zip/exe file  
            string url = "";
            XmlTextReader reader;
            try
            {
                // provide the XmlTextReader with the URL of  
                // our xml document  
                xmlPath = @"" + currentPathOfDatabase + "\\local.xml";
                // MessageBox.Show(xmlPath);
                reader = new XmlTextReader(xmlPath);
                // simply (and easily) skip the junk at the beginning  
                reader.MoveToContent();
                // internal - as the XmlTextReader moves only  
                // forward, we save current xml element name  
                // in elementName variable. When we parse a  
                // text node, we refer to elementName to check  
                // what was the node name  
                string elementName = "";
                // we check if the xml starts with a proper  
                // "ourfancyapp" element node  
                if ((reader.NodeType == XmlNodeType.Element) &&
                    (reader.Name == "ourfancyapp"))
                {
                    while (reader.Read())
                    {
                        // when we find an element node,  
                        // we remember its name  
                        if (reader.NodeType == XmlNodeType.Element)
                            elementName = reader.Name;
                        else
                        {
                            // for text nodes...  
                            if ((reader.NodeType == XmlNodeType.Text) &&
                                (reader.HasValue))
                            {
                                // we check what the name of the node was  
                                switch (elementName)
                                {
                                    case "version":
                                        // thats why we keep the version info  
                                        // in xxx.xxx.xxx.xxx format  
                                        // the Version class does the  
                                        // parsing for us  
                                        localVersion = new Version(reader.Value);

                                        Console.WriteLine("local" + localVersion);

                                        break;
                                    case "url":
                                        url = reader.Value;
                                        break;
                                }
                            }
                        }
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }

            return localVersion;
        }

        public void getVersion()
        {
            Version localVersion = getLocalVersion();
            Version newVersion = null;
            string url = "";
            XmlTextReader reader;
            try
            {
                // provide the XmlTextReader with the URL of  
                // our xml document  
                string xmlURL = "https://ramantivirus.com/database/xml/update.xml";
                reader = new XmlTextReader(xmlURL);
                // simply (and easily) skip the junk at the beginning  
                reader.MoveToContent();
                // internal - as the XmlTextReader moves only  
                // forward, we save current xml element name  
                // in elementName variable. When we parse a  
                // text node, we refer to elementName to check  
                // what was the node name  
                string elementName = "";
                // we check if the xml starts with a proper  
                // "ourfancyapp" element node  
                if ((reader.NodeType == XmlNodeType.Element) &&
                    (reader.Name == "ourfancyapp"))
                {
                    while (reader.Read())
                    {
                        // when we find an element node,  
                        // we remember its name  
                        if (reader.NodeType == XmlNodeType.Element)
                            elementName = reader.Name;
                        else
                        {
                            // for text nodes...  
                            if ((reader.NodeType == XmlNodeType.Text) &&
                                (reader.HasValue))
                            {
                                // we check what the name of the node was  
                                switch (elementName)
                                {
                                    case "version":
                                        // thats why we keep the version info  
                                        // in xxx.xxx.xxx.xxx format  
                                        // the Version class does the  
                                        // parsing for us  
                                        newVersion = new Version(reader.Value);

                                        Console.WriteLine(newVersion);

                                        break;
                                    case "url":
                                        url = reader.Value;
                                        break;
                                }
                            }
                        }
                    }


                }


                if (localVersion.CompareTo(newVersion) < 0)
                {

                    Console.WriteLine("New version detected.");
                    //   System.Windows.MessageBox.Show("new Version detected");
                    Antivirus.Properties.Settings.Default.updateVersion = true;

                    updatetxt.Visibility = Visibility.Visible;


                }
                else
                {
                    Antivirus.Properties.Settings.Default.updateVersion = false;
                }


            }
            catch (Exception e)
            {

            }

        }

        //Check For Database, Junk Files,Temp Files

        public static long GetDirectorySize()
        {
            // 1.
            // Get array of all file names.
            string[] a = Directory.GetFiles(@"C:\Windows\Prefetch", "*.*", SearchOption.AllDirectories);
            string[] temp = Directory.GetFiles(@"C:\Windows\Temp", "*.*", SearchOption.AllDirectories);
            string[] LTEMP = Directory.GetFiles("C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Temp");
            // 2.
            // Calculate total bytes of all files in a loop.
            long b = 0;
            foreach (string name in a)
            {
                // 3.
                // Use FileInfo to get length of each file.
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            foreach (string name in temp)
            {
                // 3.
                // Use FileInfo to get length of each file.
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            foreach (string name in LTEMP)
            {
                // 3.
                // Use FileInfo to get length of each file.
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            // 4.
            // Return total size
            long c = b / 1000000;
            //   System.Windows.MessageBox.Show("Size    "+b);

            //   System.Windows.MessageBox.Show("Size    " + c);
            return c;

        }

        // function to check windows update

        public static void checWnkUpdate()
        {

            UpdateSession uSession = new UpdateSession();
            IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
            uSearcher.Online = false;
            try
            {
                ISearchResult sResult = uSearcher.Search("IsInstalled=1 And IsHidden=0");
                Console.WriteLine("Found " + sResult.Updates.Count + " updates" + Environment.NewLine);
                //foreach (IUpdate update in sResult.Updates)
                //{
                //    Console.WriteLine(update.Title + Environment.NewLine);
                //}

                if (sResult.Updates.Count > 0)
                {
                    Console.WriteLine("Requires Update");
                    Antivirus.Properties.Settings.Default.winUpdate = true;


                }
                else if (sResult.Updates.Count <= 0)
                {
                    Antivirus.Properties.Settings.Default.winUpdate = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: " + ex.Message);
            }
        }
        private void Tile_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/quarantine.xaml", this);
        }

        private void Tile_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/scan.xaml", this);

        }

        private void Tile_Click_5(object sender, RoutedEventArgs e)
        {
            //UsbDetect u = new UsbDetect();
            //u.Show();
            NavigationCommands.GoToPage.Execute("Pages/update.xaml", this);

        }

        private void Tile_Click_6(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/ApplicationScan.xaml", this);
            // NavigationCommands.GoToPage.Execute("Pages/setting.xaml", this);
        }

        private void Tile_Click_7(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/aboutus.xaml", this);
        }

        private void Tile_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/cleanup.xaml", this);
          // NavigationCommands.GoToPage.Execute("Pages/support.xaml", this);
            
        }

        private void Tile_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/myacc.xaml", this);
        }

        private void Tile_Click_8(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/myacc.xaml", this);
        }

        private void Tile_Click_9(object sender, RoutedEventArgs e)
        {

            NavigationCommands.GoToPage.Execute("Pages/scan.xaml", this);
        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {


            
            try
            {
                CheckAtRisk();
                if (Antivirus.Properties.Settings.Default.scantile == false)
                {
                    tileScan.IsEnabled = false;
                }
                else if (Antivirus.Properties.Settings.Default.scantile == true)
                {
                    tileScan.IsEnabled = true;
                    //  updatetxt.Visibility = Visibility.Hidden;

                }
                if (Antivirus.Properties.Settings.Default.updateavail != 'a')
                {
                    if (Antivirus.Properties.Settings.Default.updateavail == 'c')
                    {
                        updatetxt.Visibility = Visibility.Hidden;
                    }
                    else
                        if (Antivirus.Properties.Settings.Default.updateavail == 'b')
                        {
                            updatetxt.Text = "Updating..";
                        }

                }
                SolidColorBrush co = new SolidColorBrush(Antivirus.Properties.Settings.Default.Color);
                SolidColorBrush co1 = new SolidColorBrush(Antivirus.Properties.Settings.Default.StatusColor);
                //this.pcProt.Text = Antivirus.Properties.Settings.Default.Status;
                //this.pcProt.Foreground = co1;
                if (Antivirus.Properties.Settings.Default.ScanRun)
                {
                    scanring.Visibility = Visibility.Visible;
                }
                else { scanring.Visibility = Visibility.Hidden; }
                //if (Antivirus.Properties.Settings.Default.atrisk == 1)
                //{
                //    ImageBrush myBrush = new ImageBrush();
                //    myBrush.ImageSource =
                //        new BitmapImage(new Uri("pack://application:,,,/Images/green-tick.jpg", UriKind.Absolute));
                //    grd1.Background = myBrush;

                //}
                //else if (Antivirus.Properties.Settings.Default.atrisk == 2)
                //{
                //    ImageBrush myBrush = new ImageBrush();
                //    myBrush.ImageSource =
                //       new BitmapImage(new Uri("pack://application:,,,/Images/green-tick.jpg", UriKind.Absolute));
                //    grd1.Background = myBrush;
                //}
                //else if (Antivirus.Properties.Settings.Default.atrisk == 3)
                //{
                //    //ImageBrush myBrush = new ImageBrush();
                //    //myBrush.ImageSource =
                //    //    new BitmapImage(new Uri("pack://application:,,,/Images/warning2.png", UriKind.Absolute));
                //    //grd1.Background = myBrush;

                //}
                //if (Antivirus.Properties.Settings.Default.Img)
                //{
                //    ImageBrush myBrush = new ImageBrush();
                //    myBrush.ImageSource =
                //        new BitmapImage(new Uri("pack://application:,,,/Images/green-tick.jpg", UriKind.Absolute));
                //    grd1.Background = myBrush;

                //}
                //else
                //{
                //    ImageBrush myBrush = new ImageBrush();
                //    myBrush.ImageSource =
                //       new BitmapImage(new Uri("pack://application:,,,/Images/Red-Cross.png", UriKind.Absolute));
                //    grd1.Background = myBrush;
                //}
                //   this.pcProt.Background = co;
                //   //this.StatusText.Text = Antivirus.Properties.Settings.Default.Status;
                //this.TotalFiles.Text = Antivirus.Properties.Settings.Default.TotalScanned.ToString();
                this.TotalFiles.Text = Antivirus.Properties.Settings.Default.TotScanHome.ToString();
                this.TotalIssues.Text = Antivirus.Properties.Settings.Default.Totalssues.ToString();
                if (Antivirus.Properties.Settings.Default.Totalssues > 0)
                {
                    this.TotalIssues.Text = Antivirus.Properties.Settings.Default.Totalssues.ToString();
                    this.TotalIssues.Foreground = Brushes.Red;
                }
                if (Antivirus.Properties.Settings.Default.SubType == "Trial")
                {
                    this.Substype.Text = "Trial";
                }
                else if (Antivirus.Properties.Settings.Default.SubType == "Yearly")
                {
                    this.Substype.Text = "Yearly";
                }


            }
            catch (Exception jkk) { }
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/parentalControl.xaml", this);

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Tile_Click_10(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://www.facebook.com/Ram-Antivirus-677206782468895/");
            }
            catch (Exception j) { }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // System.Windows.MessageBox.Show("Hello");
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
                Process.Start("https://ramantivirus.com/");
            }
            catch (Exception k)
            {

            }
        }

        private void Tile_Click_16(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/aboutus.xaml", this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //NavigationCommands.GoToPage.Execute("Pages/PUA.xaml", this);
        }

        private void Tile_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void tile1_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // tile1.Background = Brushes.Coral;
        }

        private void tile1_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //   tile1.Background = Brushes.Red;
        }

        private void tile1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(39, 146, 218);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            tile1.Background = backgroundBrush;
        }

        private void tile1_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(0, 122, 204);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            tile1.Background = backgroundBrush;
        }

        private void Tile_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(39, 146, 218);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            tileScan.Background = backgroundBrush;
        }

        private void tileScan_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(0, 122, 204);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            tileScan.Background = backgroundBrush;
        }

        private void tileMyCleaner_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(39, 146, 218);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            tileMyCleaner.Background = backgroundBrush;
        }

        private void tileMyCleaner_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(0, 122, 204);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            tileMyCleaner.Background = backgroundBrush;
        }

        private void updateTile_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(39, 146, 218);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            updateTile.Background = backgroundBrush;
        }

        private void updateTile_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(0, 122, 204);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            updateTile.Background = backgroundBrush;
        }

        private void tileApp_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(39, 146, 218);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);

            tileApp.Background = backgroundBrush;
        }

        private void tileApp_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(0, 122, 204);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            tileApp.Background = backgroundBrush;
        }

        private void tileParental_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(39, 146, 218);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            tileParental.Background = backgroundBrush;
        }

        private void tileParental_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(0, 122, 204);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            tileParental.Background = backgroundBrush;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Dashboard k = new Dashboard();
            k.Show();
        }

        private void Tile_Click_17(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://twitter.com/ramantivirus");
            }
            catch (Exception df) { }
        }

        private void OnTargetUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void pcProt_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (pcProt.Text == "At Risk")
            //{
            //    try
            //    {
            //        ristbtn.Visibility = Visibility.Visible;
            //    }
            //    catch (Exception j)
            //    { }
            //}
            //else if (pcProt.Text == "Attention")
            //{
            //    try
            //    {
            //        ristbtn.Visibility = Visibility.Hidden;
            //    }
            //    catch (Exception j)
            //    { }
            //}
            //else if (pcProt.Text == "Secured")
            //{
            //    try
            //    {
            //        ristbtn.Visibility = Visibility.Hidden;
            //    }
            //    catch (Exception j)
            //    { }

            //}

        }

        private void ristbtn_Click(object sender, RoutedEventArgs e)
        {

            NavigationCommands.GoToPage.Execute("Pages/Report.xaml", this);
            //diagns d = new diagns();
            // d.Show();
        }

        private void mail_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(160, 159, 159);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            mail.Background = backgroundBrush;
        }

        private void mail_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(97, 96, 96);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            mail.Background = backgroundBrush;
        }

        private void msg_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(160, 159, 159);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            msg.Background = backgroundBrush;
        }

        private void msg_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(97, 96, 96);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            msg.Background = backgroundBrush;
        }

        private void remote_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(160, 159, 159);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            remote.Background = backgroundBrush;
        }

        private void remote_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(97, 96, 96);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            remote.Background = backgroundBrush;
        }

        private void phone_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(160, 159, 159);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            phone.Background = backgroundBrush;
        }

        private void phone_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(97, 96, 96);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            phone.Background = backgroundBrush;
        }

        private void web_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(160, 159, 159);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            web.Background = backgroundBrush;
        }

        private void web_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(97, 96, 96);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            web.Background = backgroundBrush;
        }

        private void us_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(160, 159, 159);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            us.Background = backgroundBrush;
        }

        private void us_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(97, 96, 96);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            us.Background = backgroundBrush;
        }




    }
}
