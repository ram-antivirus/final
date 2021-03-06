using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for PUA.xaml
    /// </summary>
    public partial class PUA : UserControl
    {

        ObservableCollection<Potential> kl = new ObservableCollection<Potential>();
        ObservableCollection<Potential> myapp = new ObservableCollection<Potential>();
        ObservableCollection<string> b;
        ObservableCollection<string> black;
        ObservableCollection<string> final;
       
        public PUA()
        {
            InitializeComponent();
        }

        private void lst2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                //lst2.ItemsSource = kl;

                lst2.Visibility = Visibility.Hidden;
                kl = new ObservableCollection<Potential>();
              //  MessageBox.Show(System.AppDomain.CurrentDomain.BaseDirectory + "publisher_ShouldIRemoveit.bnm");
                DecryptFile(System.AppDomain.CurrentDomain.BaseDirectory + "publisher_ShouldIRemoveit.bnm", System.AppDomain.CurrentDomain.BaseDirectory + "publisher_ShouldIRemoveit.txt");
                Dispatcher.Invoke(new Action(() =>
                {
                    RunApps();
                }));
                Dispatcher.Invoke(new Action(() => { compareWhitelist(); }));
                lst2.ItemsSource = kl;
            }
            catch (Exception l)
            {
             //   MessageBox.Show("App data Exception -->>"+l);
            }


        }

      

        // compare with whitelist

        

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Potential p1 = (Potential)lst2.SelectedItem;
            var dlg = new ModernDialog
            {
                Title = "Warinig",
                Content = "Would you like to uninstall this software",
                Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
            };
            dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton, dlg.CancelButton };
            dlg.ShowDialog();


            if (dlg.DialogResult.HasValue && dlg.DialogResult.Value)
            {

                
                Process process = new Process();
                process.StartInfo.FileName = "CMD.exe";
                process.StartInfo.Arguments = "/K" + p1.Path;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();

            }
            else
            {
                try
                {

                    using (StreamWriter wr = new StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + "publisher_ShouldIRemoveit.txt", false))
                    {
                       // MessageBox.Show(p1.Publisher);
                        wr.WriteLine(p1.Publisher);
                    }

                    EncryptFile(System.AppDomain.CurrentDomain.BaseDirectory + "publisher_ShouldIRemoveit.txt", System.AppDomain.CurrentDomain.BaseDirectory + "publisher_ShouldIRemoveit.bnm");
                    Thread.Sleep(1000);
                    File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "publisher_ShouldIRemoveit.txt");
                    kl.Remove(p1); 

                    //File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + "publisher_ShouldIRemoveit.txt", p1.Publisher + Environment.NewLine);
                    //File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "publisher_ShouldIRemoveit.txt");
                    //EncryptFile(System.AppDomain.CurrentDomain.BaseDirectory + "publisher_ShouldIRemoveit.txt", System.AppDomain.CurrentDomain.BaseDirectory + "publisher_ShouldIRemoveit.bnm");
                    //File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "publisher_ShouldIRemoveit.txt");
                    //kl.Remove(p1);                                      
                }
                catch (Exception kl) { //MessageBox.Show("Appl Exception-->>" + kl); 
                }
            }
        }

        public async void RunApps()
        {
            try
            {
                string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                using (var baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (var key = baseKey.OpenSubKey(uninstallKey))
                    {
                        foreach (string skName in key.GetSubKeyNames())
                        {
                            using (RegistryKey sk = key.OpenSubKey(skName))
                            {
                                try
                                {

                                    string displayName = sk.GetValue("DisplayName").ToString();
                                    string publisher = sk.GetValue("Publisher").ToString();
                                    string path = sk.GetValue("UninstallString").ToString();
                                    Console.WriteLine(displayName);
                                    myapp.Add(new Potential() { Name = displayName, Publisher = publisher, Path = path });

                                    //  lst1.ItemsSource = entries;

                                    //switch (displayName)
                                    //{
                                    //    case "PC Optimizer Pro":
                                    //        Console.WriteLine(displayName);
                                    //        break;

                                    //    default:
                                    //        // Console.WriteLine("Default case");
                                    //        break;
                                    //}

                                }
                                catch (Exception vdf)
                                {

                                }


                            }
                        }

                    }
                }

                // current user 

                using (var baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    using (var key = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))
                    {
                        foreach (string skName in key.GetSubKeyNames())
                        {
                            using (RegistryKey sk = key.OpenSubKey(skName))
                            {
                                try
                                {

                                    string displayName = sk.GetValue("DisplayName").ToString();
                                    string publisher = sk.GetValue("Publisher").ToString();
                                    string path = sk.GetValue("UninstallString").ToString();
                                    Console.WriteLine(displayName);
                                    myapp.Add(new Potential() { Name = displayName, Publisher = publisher, Path = path });

                                    //lst.ItemsSource = entries;

                                    //switch (displayName)
                                    //{
                                    //    case "PC Optimizer Pro":
                                    //        Console.WriteLine(displayName);
                                    //        break;

                                    //    default:
                                    //        // Console.WriteLine("Default case");
                                    //        break;
                                    //}
                                }
                                catch (Exception vdf)
                                {

                                }


                            }
                        }

                    }
                }

                // wow path 

                using (var baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (var key = baseKey.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"))
                    {
                        foreach (string skName in key.GetSubKeyNames())
                        {
                            using (RegistryKey sk = key.OpenSubKey(skName))
                            {
                                try
                                {

                                    string displayName = sk.GetValue("DisplayName").ToString();
                                    string publisher = sk.GetValue("Publisher").ToString();
                                    string path = sk.GetValue("UninstallString").ToString();
                                    Console.WriteLine(displayName);
                                    myapp.Add(new Potential() { Name = displayName, Publisher = publisher, Path = path });

                                    // lst.ItemsSource = entries;
                                    //switch (displayName)
                                    //{
                                    //    case "PC Optimizer Pro":
                                    //        Console.WriteLine(displayName);
                                    //        break;

                                    //    default:
                                    //        //  Console.WriteLine("Default case");
                                    //        break;
                                    //}
                                }
                                catch (Exception vdf)
                                {

                                }


                            }
                        }

                    }
                }

                // File.WriteAllLines(@"E:\a.txt", entries);

            }
            catch (Exception h) {
              //  MessageBox.Show("Exception--->>" + h);
            }
        }
        
        // compare with whitelist

        public async void compareWhitelist()
        {
            string[] simpleArray = { };
            // StreamWriter wr = new StreamWriter(@"E:\b.txt",true);
            b = new ObservableCollection<string>();
            black = new ObservableCollection<string>();


            var fileRead = File.ReadAllLines(Environment.CurrentDirectory + "\\publisher_ShouldIRemoveit.txt");
            foreach (var h in fileRead)
            {
                b.Add(h);

            }

            try
            {
                //   MessageBox.Show(b.Count.ToString());


                foreach (var g in myapp)
                {
                    foreach (var m in b)
                    {
                        if (!g.Publisher.Equals(m))
                        {
                            black.Add(g.Publisher);
                            break;
                        }



                    }
                }

                var bnotContains = black.Except(b);


                foreach (var t in bnotContains)
                {
                    foreach (var x in myapp)
                    {
                        if (x.Publisher == t)
                        {
                            if (kl.Any(p => p.Name == x.Name) == false) kl.Add(new Potential() { Name = x.Name, Path = x.Path, Publisher = x.Publisher });
                            //Dispatcher.Invoke(new Action(() =>
                            //kl.Add(new Person() { Name = x.Name, Path = x.Path,Publisher =x.Publisher })));
                            //MessageBox.Show(Name);

                        }
                        //else
                        //{
                        //    lst2.Items.Add("not matched");

                        //}
                    }
                }

                //foreach (var d in person)
                //{
                //    if (d.Publisher== bnotContains.ToString())
                //    {
                //        MessageBox.Show(d.Name);
                //    }
                //    else {

                //        MessageBox.Show("not matched");
                //    }
                //}
                //foreach (var bm in bnotContains)
                //{
                //    final.Add(bm);
                //    break;

                //}


                // 
                //MessageBox.Show("Count -- >>" + lst2.Items.Count);
                var dlg = new ModernDialog
                {

                    Content = kl.Count + " " + "Unknown Publishers Detected",
                    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                };
                dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton };
                dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                // dlg.OkButton.FontWeight = 
                dlg.OkButton.FontWeight = FontWeights.Bold;
                dlg.ShowDialog();
              
                lst2.Visibility =Visibility.Visible;

            }
            catch (Exception g)
            {
                  MessageBox.Show("Exception of loop-->>" + g);
            }

            //  MessageBox.Show(black.Count.ToString());
        }


        // encrypt file
        private static void EncryptFile(string inputFile, string outputFile)
        {

            try
            {
                string password = @"myKey123"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch(Exception d)
            {
              //  MessageBox.Show("Encryption failed!", "Error-->>"+d);
            }
        }
        //decrypt function

        private static void DecryptFile(string inputFile, string outputFile)
        {
            try
            {
                string password = @"myKey123"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            }
            catch (Exception f) {// MessageBox.Show("Exception decrypt -->>"+f); 
            }
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void NotifyCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
            }
        }

        

    }
}
