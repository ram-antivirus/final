using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

namespace Antivirus
{
    /// <summary>
    /// Interaction logic for PotenTially.xaml
    /// </summary>
    public partial class PotenTially : ModernWindow
    {
        ObservableCollection<Potential> myapp = new ObservableCollection<Potential>();
        ObservableCollection<string> b;
        ObservableCollection<string> black;
        ObservableCollection<string> final;
        ObservableCollection<Potential> kl;
        public PotenTially()
        {
            InitializeComponent();
            lst2.ItemsSource = kl;

            kl = new ObservableCollection<Potential>();
            DecryptFile(Environment.CurrentDirectory + "\\publisher_ShouldIRemoveit.bnm", Environment.CurrentDirectory + "\\publisher_ShouldIRemoveit.txt");
            Dispatcher.Invoke(new Action(() =>
            {
                RunApps();
            }));
            Dispatcher.Invoke(new Action(() => { compareWhitelist(); }));
            lst2.ItemsSource = kl;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        public async void RunApps()
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
                            //kl.Add(new Potential() { Name = x.Name, Path = x.Path,Publisher =x.Publisher })));
                            //MessageBox.Show(Name);

                        }
                        //else
                        //{
                        //    lst2.Items.Add("not matched");

                        //}
                    }
                }

                //foreach (var d in Potential)
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

            }
            catch (Exception g)
            {
                //  MessageBox.Show("Exception of loop-->>" + g);
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
            catch
            {
                Console.WriteLine("Encryption failed!", "Error");
            }
        }
        //decrypt function

        private static void DecryptFile(string inputFile, string outputFile)
        {

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
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void NotifyCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
            }
        }

        private void lst2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
