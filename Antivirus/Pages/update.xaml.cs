using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Antivirus.Pages
{
    /// <summary>
    /// Interaction logic for update.xaml
    /// </summary>
    public partial class update : System.Windows.Controls.UserControl
    {
        string currentPathOfDatabase = AppDomain.CurrentDomain.BaseDirectory + @"\db\";
        CancellationTokenSource cancellationTokenSource;
        CancellationToken token;
        static bool cancel = true;
        private Action _cancelwork;
        string xmlPath;
        Task task;
        Task taskfirst;
        static float updateVersion;
        public update()
        {
            InitializeComponent();
            this.CancelButton.IsEnabled = false;
            version.Text = getLocalVersion().ToString();


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            try
            {


                Antivirus.Properties.Settings.Default.scantile = false;
                Antivirus.Properties.Settings.Default.updateavail = 'b';
                this.progress.Visibility = Visibility.Visible;
                checkForUpdates();
                downloadwhitelist();

            }
            catch (Exception l) { }


        }


        public async void checkForUpdates()
        {
            if (CheckForInternetConnection() == true)
            {
                cancellationTokenSource = new CancellationTokenSource();
                token = cancellationTokenSource.Token;

                this.StartButton.IsEnabled = false;
                this.CancelButton.IsEnabled = true;
                //this._cancelwork = (Action)(() =>
                //{
                //    cancellationTokenSource.Cancel();
                //    progress.Visibility = Visibility.Hidden;
                //    System.Windows.MessageBox.Show("Cancelled");
                //    Antivirus.Properties.Settings.Default.scantile = true;
                //    Antivirus.Properties.Settings.Default.updateavail = 'c';
                //});

                //cancellationTokenSource = new CancellationTokenSource();
                //token = cancellationTokenSource.Token;
                manualUpdate();
                // task = Task.Run(() =>  manualUpdate(),token);
                // await Task.WhenAll(task);
                //   if (this._cancelwork == null)
                //  {
                //MessageBoxResult r = System.Windows.MessageBox.Show("Update Finished....", "Message", MessageBoxButton.OK);
                //if (r == MessageBoxResult.OK)
                //{
                //    version.Text = updateVersion.ToString();
                //    progress.Visibility = Visibility.Hidden;
                //    Antivirus.Properties.Settings.Default.scantile = true;
                //    Antivirus.Properties.Settings.Default.updateavail = 'c';
                //    //  System.Windows.MessageBox.Show("In Ok");
                //    //  System.Windows.MessageBox.Show(updateVersion.ToString());
                //    Restart v = new Restart();
                //    v.txt.Text = "Your PC needs to restart";
                //    v.Show();
                //    v.Activate();
                //    v.Topmost = true;
                //}
                //this.StartButton.IsEnabled = true;
                //this.CancelButton.IsEnabled = false;
                //Antivirus.Properties.Settings.Default.updateDate = DateTime.Now;
                //  }
                //  else
                //  {

                //     System.Windows.MessageBox.Show("Cancelled");
                //  Antivirus.Properties.Settings.Default.scantile = true;
                //   Antivirus.Properties.Settings.Default.updateavail = 'c';
                // }

            }
            else
            {

                MyPopup lklk = new MyPopup();
                lklk.txt.Text = "Please Connect to internet to update database!";
                lklk.Show();

            }
        }



        //// check for internet 

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        public void xmlCopyServertoLocal()
        {
            try
            {
                string remoteUri = "https://ramantivirus.com/database/xml/";
                string fileName = "update.xml", myStringWebResource = null;
                string folderPath = currentPathOfDatabase;
                WebClient myWebClient = new WebClient();
                myStringWebResource = remoteUri + fileName;
                Console.WriteLine(">>>>>>" + myStringWebResource);
                Console.WriteLine(">>>>>>" + folderPath);

                Console.WriteLine("Downloading File \"{0}\" from \"{1}\" .......\n\n", fileName, myStringWebResource);
                // Download the Web resource and save it into the current filesystem folder.
                myWebClient.DownloadFile(myStringWebResource, folderPath);
                Console.WriteLine("Successfully Downloaded File \"{0}\" from \"{1}\"", fileName, myStringWebResource);
                Console.WriteLine("\nDownloaded file saved in the following file system folder:\n\t" + folderPath);

            }
            catch (Exception er)
            {

            }

        }

        public void manualUpdate()
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
                    // ask the user if he would like  
                    // to download the new version  
                    Console.WriteLine("New version detected.");

                    String st1 = localVersion.ToString();
                    String st2 = newVersion.ToString();

                    Console.WriteLine("------" + st1 + "----" + st2);

                    float local = float.Parse(localVersion.ToString());
                    updateVersion = float.Parse(newVersion.ToString());

                    while (local < updateVersion)
                    {
                        getThisVersionXML(local);
                        local = local + 0.1F;
                    }
                    updateFile(updateVersion);
                }
                else
                {
                    MyPopup n = new MyPopup();
                    n.txt.Text = "Your antivirus software is up to date. Ram Antivirus provides next-generation protection continually updates security intelligence definitions to protect you from the latest malware threats. There is no update available at this moment.Please check again later";
                    progress.Visibility = Visibility.Hidden;
                    n.Show();
                    Antivirus.Properties.Settings.Default.scantile = true;
                    Antivirus.Properties.Settings.Default.updateavail = 'c';
                    progress.Visibility = Visibility.Hidden;

                }


            }
            catch (Exception e)
            {
                // MessageBox.Show(e.Message);
            }
            // return (newVersion);
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

        public async void getThisVersionXML(float local)
        {

            try
            {
                local = local + 0.1F;
                String xmlLocation = "https://ramantivirus.com/database/xml/" + local.ToString() + ".xml";
                Console.WriteLine("=======" + xmlLocation);
                XmlTextReader textReader = new XmlTextReader(xmlLocation);
                while (textReader.Read())
                {
                    switch (textReader.NodeType)
                    {
                        case XmlNodeType.Element:


                            if (textReader.Name == "file")
                            {
                                this._cancelwork = (Action)(() =>
                                {
                                    cancellationTokenSource.Cancel();
                                    progress.Visibility = Visibility.Hidden;
                                    System.Windows.MessageBox.Show("Cancelled");
                                    Antivirus.Properties.Settings.Default.scantile = true;
                                    Antivirus.Properties.Settings.Default.updateavail = 'c';
                                    cancel = false;
                                });

                                cancellationTokenSource = new CancellationTokenSource();
                                token = cancellationTokenSource.Token;
                                task = Task.Run(() => downLoadFiles(textReader.GetAttribute("name"), textReader.GetAttribute("src"), textReader.GetAttribute("folder")), token);
                                await Task.WhenAll(task);
                                //  downLoadFiles(textReader.GetAttribute("name"), textReader.GetAttribute("src"), textReader.GetAttribute("folder"));
                                if (cancel)
                                {
                                    string updatever = updateVersion.ToString();
                                    string loc = local.ToString();
                                    //  System.Windows.MessageBox.Show("updateVersion" + updateVersion +"Local Version " + local );
                                    if (updatever.Equals(loc))
                                    {
                                        MessageBoxResult r = System.Windows.MessageBox.Show("Update Finished....", "Message", MessageBoxButton.OK);
                                        if (r == MessageBoxResult.OK)
                                        {
                                            version.Text = updateVersion.ToString();
                                            progress.Visibility = Visibility.Hidden;
                                            Antivirus.Properties.Settings.Default.scantile = true;
                                            Antivirus.Properties.Settings.Default.updateavail = 'c';
                                            //  System.Windows.MessageBox.Show("In Ok");
                                            //  System.Windows.MessageBox.Show(updateVersion.ToString());
                                            Restart v = new Restart();
                                            v.txt.Text = "Restart Required : Your computer needs to be restarted in order to complete software installation";
                                            v.Show();
                                            v.Activate();
                                            v.Topmost = true;
                                        }
                                    }

                                }
                            }


                            Console.WriteLine(textReader.Value);
                            Console.WriteLine(textReader.GetAttribute("name"));
                            break;

                    }
                }
            }
            catch (Exception e)
            {
            }
        }



        public void downloadwhitelist()
        {
            try
            {
                if (CheckForInternetConnection() == true)
                {
                    string remoteUri = "http://ramantivirus.com/white-list-publishers/";
                    string fileName = "publisher_RAM.bnm", myStringWebResource = null;
                    // Create a new WebClient instance.
                    WebClient myWebClient = new WebClient();
                    // Concatenate the domain with the Web resource filename.
                    myStringWebResource = remoteUri + fileName;
                    Console.WriteLine("Downloading File \"{0}\" from \"{1}\" .......\n\n", fileName, myStringWebResource);
                    // Download the Web resource and save it into the current filesystem folder.
                    myWebClient.DownloadFile(myStringWebResource, @"" + AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName);



                    Console.WriteLine("Successfully Downloaded File \"{0}\" from \"{1}\"", fileName, myStringWebResource);
                    //   System.Windows.MessageBox.Show("\nDownloaded file saved in the following file system folder:\n\t" + Directory.GetCurrentDirectory());

                    Console.Read();
                }
                else
                {
                    MyPopup p = new MyPopup();
                    p.txt.Text = "No Internet Connection";
                    p.Show();
                }
            }
            catch (Exception a)
            { }
        }

        public void downLoadFiles(String file, String src, String folder)
        {
            string remoteUri = src;
            string fileName = file, myStringWebResource = null;
            string folderPath = @"" + currentPathOfDatabase + "\\" + file;

            try
            {
                // Create a new WebClient instance.
                WebClient myWebClient = new WebClient();
                // Concatenate the domain with the Web resource filename.
                myStringWebResource = remoteUri + @"/" + fileName;
                // Console.WriteLine(">>>>>>" + myStringWebResource);
                //  Console.WriteLine(">>>>>>" + folderPath);

                // System.Windows.MessageBox.Show("Downloading File,It can take a While,Please Don't Close Window.......\n\n"+ fileName+ myStringWebResource);
                // Download the Web resource and save it into the current filesystem folder.
                myWebClient.DownloadFile(myStringWebResource, folderPath);
                // System.Windows.MessageBox.Show("Successfully Downloaded File \"{0}\" from \"{1}\""+ fileName+ myStringWebResource);
                // System.Windows.MessageBox.Show("\nDownloaded file saved in the following file system folder:\n\t" + folderPath);

            }
            catch (Exception e)
            {

            }
        }


        // this fun is to call in home .xaml

        public void OnlyUpdate()
        {
            try
            {
                if (CheckForInternetConnection() == true)
                {
                    cancellationTokenSource = new CancellationTokenSource();
                    token = cancellationTokenSource.Token;
                    //  this.progress.Visibility = Visibility.Visible;
                    this.StartButton.IsEnabled = false;
                    this.CancelButton.IsEnabled = true;
                    var updateTask = Task.Run(() => manualUpdateOnly());
                    //  downloadsign(); 
                    //  var downTask = Task.Run(() => downloadsign());

                    Task.WhenAll(updateTask);

                    //version.Text = manualUpdate().ToString();

                    this.StartButton.IsEnabled = true;
                    this.CancelButton.IsEnabled = false;
                    // this.progress.Visibility = Visibility.Hidden;
                    // MessageBox.Show("Update Finished------");
                    Antivirus.Properties.Settings.Default.updateDate = DateTime.Now;
                    //    MessageBox.Show(Antivirus.Properties.Settings.Default.updateDate.ToString());
                    //  NavigationCommands.GoToPage.Execute("Pages/scan.xaml", this);
                }
                else
                {

                    MyPopup lklk = new MyPopup();
                    lklk.txt.Text = "Please Connect to internet to update database!";
                    lklk.Show();

                }
            }
            catch (Exception g)
            { }
        }


        // to call in home.xaml

        public void manualUpdateOnly()
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
                    // ask the user if he would like  
                    // to download the new version  
                    Console.WriteLine("New version detected.");
                    //System.Windows.MessageBox.Show("new Version detected");
                    Antivirus.Properties.Settings.Default.updateVersion = true;




                }
                else
                {
                    Antivirus.Properties.Settings.Default.updateVersion = false;
                }



            }
            catch (Exception e)
            {
                // MessageBox.Show(e.Message);
            }

        }
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (this._cancelwork == null)
                return;
            this._cancelwork();

            //progress.Visibility = Visibility.Hidden;
            //progress.Visibility = Visibility.Hidden;
            //Antivirus.Properties.Settings.Default.scantile = true;
            //Antivirus.Properties.Settings.Default.updateavail = 'c';

            //cancellationTokenSource.Cancel();
            //this.StartButton.IsEnabled = true;
            //this.CancelButton.IsEnabled = false;
        }

        public void updateFile(float update)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string path = xmlPath;

                doc.Load(path);
                IEnumerator ie = doc.SelectNodes("/ourfancyapp/version").GetEnumerator();

                while (ie.MoveNext())
                {
                    if ((ie.Current as XmlNode).Name == "version")
                    {
                        (ie.Current as XmlNode).InnerText = update.ToString();
                    }

                }

                doc.Save(path);
            }
            catch (Exception r)
            { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog choofdlog = new OpenFileDialog();
                string sFileName = "";
                choofdlog.Filter = "All Files (*.*)|*.*";
                choofdlog.FilterIndex = 1;
                choofdlog.Multiselect = true;

                if (choofdlog.ShowDialog() == DialogResult.OK)
                {
                    sFileName = choofdlog.FileName;
                    string[] arrAllFiles = choofdlog.FileNames; //used when Multiselect = true           
                }

                File.Copy(sFileName, AppDomain.CurrentDomain.BaseDirectory + "db\\" + System.IO.Path.GetFileName(sFileName));
            }
            catch (Exception k) { }
        }
    }
}
