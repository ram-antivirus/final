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
    /// Interaction logic for ApplicationScan.xaml
    /// </summary>
    public partial class ApplicationScan : UserControl
    {
        ObservableCollection<Person2> myapp = new ObservableCollection<Person2>();
        ObservableCollection<string> b;
        ObservableCollection<string> black;
        ObservableCollection<string> final;
        ObservableCollection<Person2> kl;
        string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
        string c;
        string input;
        string output;

        public ApplicationScan()
        {
            InitializeComponent();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string s = Directory.GetCurrentDirectory();
                string a = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

                c = location.Replace(@"\Antivirus.exe", "");
                string input = c + @"\publisher_RAM.bnm";
                string output = c + @"\publisher_RAM.txt";

                //    MessageBox.Show(s);
                //  MessageBox.Show(a);
                //   MessageBox.Show(c);
                lst2.ItemsSource = kl;

                // MessageBox.Show(input);
                //  MessageBox.Show(output);

                kl = new ObservableCollection<Person2>();
                DecryptFile(input, output);
                Dispatcher.Invoke(new Action(() =>
                {
                    RunApps();
                }));
                Dispatcher.Invoke(new Action(() => { compareWhitelist(); }));
                lst2.ItemsSource = kl;
            }
            catch (Exception lkl)
            {
                //MessageBox.Show("Exception App-->>"+lkl);
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
                                    myapp.Add(new Person2() { Name = displayName, Publisher = publisher, Path = path });

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
                                    myapp.Add(new Person2() { Name = displayName, Publisher = publisher, Path = path });

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
                                    myapp.Add(new Person2() { Name = displayName, Publisher = publisher, Path = path });

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
            catch (Exception j)
            {
                // MessageBox.Show("Exception In RunApps " + j);
            }
        }
        public async void compareWhitelist()
        {
            c = location.Replace(@"\Antivirus.exe", "");
            string input = c + @"\publisher_RAM.bnm";
            string output = c + @"\publisher_RAM.txt";


            //  MessageBox.Show("From CompareList");
            //   MessageBox.Show(input);
            //   MessageBox.Show(output);

            try
            {
                string[] simpleArray = { };
                // StreamWriter wr = new StreamWriter(@"E:\b.txt",true);
                b = new ObservableCollection<string>();
                black = new ObservableCollection<string>();

                var fileRead = File.ReadAllLines(output);
                foreach (var h in fileRead)
                {
                    b.Add(h);

                }
            }
            catch (Exception l)
            {// MessageBox.Show("Exception -->>"+l); 
            }

            try
            {
                //   MessageBox.Show(b.Count.ToString());

                //myapp is installes application list
                foreach (var g in myapp) //McAfee, limited
                {
                    //b is publishers list
                    foreach (var m in b)  //McAfee. 
                    {
                        
                        if (g.Publisher.Contains(m))
                        {
                            break;
                        }

                        else {
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
                            if (kl.Any(p => p.Name == x.Name) == false) kl.Add(new Person2() { Name = x.Name, Path = x.Path, Publisher = x.Publisher });
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
                //var dlg = new ModernDialog
                //{

                //    Content = kl.Count + " " + "Unknown Publishers Detected",
                //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                //};
                //dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton };
                //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                //// dlg.OkButton.FontWeight = 
                //dlg.OkButton.FontWeight = FontWeights.Bold;
                //dlg.ShowDialog();
                if (kl.Count > 0)
                {
                    MyPopup jkhk = new MyPopup();
                    jkhk.txt.Text = kl.Count + " " + "Unknown Applications Detected";

                    jkhk.ShowDialog();
                    File.Delete(output);
                    btnwhtlist.IsEnabled = true;
                }
                else if (kl.Count == 0)
                {
                    MyPopup jkhk = new MyPopup();
                    jkhk.txt.Text = "Your system has no Unkown Application";

                    jkhk.ShowDialog();
                    File.Delete(output);

                }


            }
            catch (Exception g)
            {
                //  MessageBox.Show("Exception of loop-->>" + g);
            }

            //  MessageBox.Show(black.Count.ToString());
        }
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
            catch (Exception p)
            {
                //   MessageBox.Show("Exception -->"+p);
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                Person2 p1 = (Person2)lst2.SelectedItem;
                if (lst2.SelectedItem == null)
                {
                    //var dlg1 = new ModernDialog
                    //{

                    //    Content = "Select an  Option to Uninstall",
                    //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                    //};
                    //dlg1.Buttons = new System.Windows.Controls.Button[] { dlg1.OkButton };
                    //dlg1.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                    //dlg1.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                    //// dlg.OkButton.FontWeight = 
                    //dlg1.OkButton.FontWeight = FontWeights.Bold;
                    //dlg1.ShowDialog();

                    MyPopup nm = new MyPopup();
                    nm.txt.Text = "Select an  Option to Uninstall";
                    nm.ShowDialog();
                }
                else
                {

                    Process process = new Process();
                    process.StartInfo.FileName = "CMD.exe";
                    process.StartInfo.Arguments = "/K" + p1.Path;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.Start();
                }
                //else
                //{
                //    Person2 p1 = (Person2)lst2.SelectedItem;
                //    var dlg = new ModernDialog
                //    {
                //        Title = "Warinig",
                //        Content = "Would you like to uninstall this software",
                //        Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                //    };
                //    dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton, dlg.CancelButton };
                //    dlg.ShowDialog();


                //    if (dlg.DialogResult.HasValue && dlg.DialogResult.Value)
                //    {


                //        Process process = new Process();
                //        process.StartInfo.FileName = "CMD.exe";
                //        process.StartInfo.Arguments = "/K" + p1.Path;
                //        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //        process.Start();

                //    }
                //    else
                //    {
                //        File.AppendAllText(Directory.GetCurrentDirectory() + "\\publisher_ShouldIRemoveit.txt", p1.Publisher + Environment.NewLine);
                //        EncryptFile(Directory.GetCurrentDirectory() + "\\publisher_ShouldIRemoveit.txt", Directory.GetCurrentDirectory() + "\\publisher_ShouldIRemoveit.bnm");
                //        File.Delete(Directory.GetCurrentDirectory() + "\\publisher_ShouldIRemoveit.txt");
                //        kl.Remove(p1);

                //    }
            }
            catch (Exception j)
            {

            }
        }

        private void lst2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btnwhtlist.IsEnabled = true;
            c = location.Replace(@"\Antivirus.exe", "");
            string input = c + @"\publisher_RAM.bnm";
            string output = c + @"\publisher_RAM.txt";
            //    MessageBox.Show("From ButtonClick");
            //     MessageBox.Show(input);
            //   MessageBox.Show(output);

            try
            {

                Person2 p1 = (Person2)lst2.SelectedItem;
                if (lst2.SelectedItem == null)
                {

                    MyPopup kjk = new MyPopup();
                    kjk.txt.Text = "Select an Application which you want to add in whitelist";
                    kjk.ShowDialog();
                }
                DecryptFile(input, output);
                //Person2 p1 = (Person2)lst2.SelectedItem;
                File.AppendAllText(output, p1.Publisher + Environment.NewLine);
                EncryptFile(output, input);
                File.Delete(output);
                kl.Remove(p1);
                MyPopup kj = new MyPopup();
                kj.txt.Text = "Your selected application Whitelisted";
                kj.ShowDialog();


            }
            catch (Exception kfgu) { }
            finally
            {
                kl.Clear();
                btnwhtlist.IsEnabled = false;
            }
        }
    }


}
