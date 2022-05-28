using Antivirus.Pages;
using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

namespace home.Pages
{
    /// <summary>
    /// Interaction logic for registry.xaml
    /// </summary>
    public partial class registry : UserControl
    {
        ObservableCollection<RegEntry> entries = null;
        public registry()
        {
            InitializeComponent();
            // MessageBox.Show("/////init");
        }
        static string softwareRegLoc = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";
        string[] regpaths = { "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\RunMRU", "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\App Paths", "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts" };
        int j = 0;
        int k = 0;
        int l = 0;
        int m = 0;

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            if (lstall.Items.Count == 0)
            {
                //var dlg = new ModernDialog
                //{

                //    Content = "No Issues Detected",
                //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                //};
                //dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton};
                //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                //// dlg.OkButton.FontWeight = 
                //dlg.OkButton.FontWeight = FontWeights.Bold;
                //dlg.ShowDialog();

                MyPopup nm = new MyPopup();
                nm.txt.Text = "No Issues Detected";
                nm.ShowDialog();
            }
            else
            {
                var dlg = new ModernDialog
                {

                    Content = "Would You like to take backup of Registry",
                    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                };
                dlg.Buttons = new System.Windows.Controls.Button[] { dlg.YesButton, dlg.NoButton };
              
                dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                // dlg.OkButton.FontWeight = 
                dlg.OkButton.FontWeight = FontWeights.Bold;
                dlg.ShowDialog();
                if (dlg.DialogResult.HasValue && dlg.DialogResult.Value)
                {
                   // MyPopup pop = new MyPopup();
                   // pop.txt.Text = "This will take some time. Please don't click back button";
                   //pop.Show();

                    string path = "\"" + "C:\\Users\\" + Environment.UserName + "\\Desktop\\MyRegistry.reg" + "\"";
                    // string key = "\"" + "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Idconfigdb" + "\"";
                    Process proc = new Process();
                    pbscan.IsIndeterminate = true;

                    try
                    {
                        proc.StartInfo.FileName = "regedit.exe";
                        proc.StartInfo.UseShellExecute = false;

                        proc = Process.Start("regedit.exe", "/e " + path);
                        proc.WaitForExit();
                        //var a = new ModernDialog
                        //{

                        //    Content = "The backup is stored on your Desktop",
                        //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                        //};
                        //a.Buttons = new System.Windows.Controls.Button[] { a.OkButton };
                        //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                        //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                        //// dlg.OkButton.FontWeight = 
                        //dlg.OkButton.FontWeight = FontWeights.Bold;
                        //a.ShowDialog();

                        MyPopup nm = new MyPopup();
                        nm.txt.Text = "The backup is stored on your Desktop";
                        nm.ShowDialog();
                        await Task.Run(() => clearList());
                        pbscan.IsIndeterminate = false;
                    }
                    catch (Exception)
                    {
                        proc.Dispose();
                    }
                }
                else
                {
                    pbscan.IsIndeterminate = false;
                    pbscan.Minimum = 0;
                    pbscan.Maximum = entries.Count;
                    await Task.Run(() => clearList());
                }
            }
         
        }


        public void clearList()
        {

            for (int i = entries.Count - 1; i >= 0; i--)
            {

                switch (entries[i].Type)
                {
                    case "key": deleteKeys(entries[i]);
                       // MessageBox.Show("CAlliing key");
                        break;
                    case "value": deleteValues(entries[i]);
                        break;
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    entries.RemoveAt(i);
                    pbscan.Value = pbscan.Maximum - i;
                    //MessageBox.Show(pbar.Value+"");
                }));

            }

        }

        public void deleteKeys(RegEntry entry)
        {
            if (IntPtr.Size == 4)
            {
                try
                {
                    using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    {
                        // MessageBox.Show("DELeting>>>" + entry.Name);
                        if (hklm.OpenSubKey(entry.Name).SubKeyCount == 0)
                            hklm.DeleteSubKey(entry.Name);
                        else
                            hklm.DeleteSubKeyTree(entry.Name);
                    }
                }
                catch (Exception e)
                {

                     //MessageBox.Show(e.Message+">>>>"+entry.Name);
                    try
                    {
                        using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                        {
                            if (hklm.OpenSubKey(entry.Name).SubKeyCount == 0)
                                hklm.DeleteSubKey(entry.Name);
                            else
                                hklm.DeleteSubKeyTree(entry.Name);
                        }
                    }
                    catch (Exception ex)
                    {

                        //MessageBox.Show(entry.Name+">>>" + ex);
                    }


                }
            }
            else
            {
                try
                {
                    using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
                    {
                        hklm.DeleteSubKey(entry.Name);
                    }
                }
                catch (Exception e)
                {
                   // MessageBox.Show(e.Message);
                    try
                    {
                        using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
                        {
                            hklm.DeleteSubKey(entry.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                      //  MessageBox.Show("" + ex);

                    }
                }

            }
        }

        public void deleteValues(RegEntry entry)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                // MessageBox.Show(">>>"+entry.Name+">>"+entry.Path);
                try
                {
                    using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    {
                        var a = hklm.OpenSubKey(entry.Path, true);
                        a.DeleteValue(entry.Name);
                    }
                }
                catch (Exception e)
                {

                   //  MessageBox.Show(e.ToString());
                    try
                    {
                        using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                        {
                            var a = hklm.OpenSubKey(entry.Path, true);
                            
                            a.DeleteValue(entry.Name);
                        }
                    }
                    catch (Exception ex)
                    {

                        //MessageBox.Show("" + ex);
                    }


                }
            }
            else
            {

                try
                {
                    using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
                    {
                        var a = hklm.OpenSubKey(entry.Path, true);
                        a.DeleteValue(entry.Name);
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
                        {
                            var a = hklm.OpenSubKey(entry.Path, true);
                            a.DeleteValue(entry.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show("" + ex);

                    }

                }
            }
        }




        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
           // fixbtn.IsEnabled = false;
            lstall.ItemsSource = entries;
            pbscan.IsIndeterminate = true;
            entries = new ObservableCollection<RegEntry>();
            lstall.ItemsSource = entries;
            
            await Task.Run(() => getList());

            pbscan.IsIndeterminate = false;
            
if (entries.Count == 0)
            {
                //var dlg = new ModernDialog
                //{

                //    Content = "No Issues Detected",
                //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                //};
                //dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton};
                //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                //// dlg.OkButton.FontWeight = 
                //dlg.OkButton.FontWeight = FontWeights.Bold;
                //dlg.ShowDialog();

                MyPopup jk = new MyPopup();
                jk.txt.Text = "No Issues Detected";
                jk.ShowDialog();
            }

            
        }



        //file count thread


        public void getList()
        {


            bool? isSleepChecked = chkempty.Dispatcher.Invoke(new Func<bool?>(
                             () => chkempty.IsChecked));


            if ((bool)isSleepChecked)
            {
                string lstallsoft = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";
                if (IntPtr.Size == 4)
                {
                    using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    using (var key = hklm.OpenSubKey(lstallsoft))
                    {
                        foreach (string ac in key.GetSubKeyNames())
                        {
                            RegistryKey empkey = key.OpenSubKey(ac);
                            //j++;
                            //string df = (string)empkey.GetValue("InstallSource");
                            string fjg = (string)empkey.GetValue("DisplayName");
                            if (fjg == null)
                            {
                                Dispatcher.Invoke(new Action(() =>
                                {
                                    entries.Add(new RegEntry() { Name = lstallsoft + "\\" + ac, Type = "key" });
                                }));
                            }
                        }
                    }

                }
                else
                {
                    using (RegistryKey emp = Registry.LocalMachine.OpenSubKey(lstallsoft, false))
                    {
                        foreach (string ac in emp.GetSubKeyNames())
                        {
                            RegistryKey empkey = emp.OpenSubKey(ac);
                            //j++;
                            //string df = (string)empkey.GetValue("InstallSource");
                            string fjg = (string)empkey.GetValue("DisplayName");
                            if (fjg == null)
                            {
                                Dispatcher.Invoke(new Action(() =>
                                {
                                    entries.Add(new RegEntry() { Name = lstallsoft + "\\" + ac, Type = "key" });
                                }));
                            }
                        }
                    }



                }
            }

            //MRU
            isSleepChecked = chkMRU.Dispatcher.Invoke(new Func<bool?>(
                            () => chkMRU.IsChecked));
            if ((bool)isSleepChecked)
            {
                string regPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU";
                using (RegistryKey flkeydf = Registry.CurrentUser.OpenSubKey(regPath))
                {

                    if (flkeydf != null)
                    {
                        try
                        {
                            Dispatcher.Invoke(new Action(() =>
                            {
                                entries.Add(new RegEntry() { Name = regPath, Path = "key", Type = "key" });
                            }));

                        }
                        catch (Exception df)
                        {
                            //  MessageBox.Show("" + df);
                        }
                    }

                }
            }
            //App paths
           
           // }
            //file extensions
            isSleepChecked = chkappths.Dispatcher.Invoke(new Func<bool?>(
                          () => chkappths.IsChecked));
            if ((bool)isSleepChecked)
            {
                string regPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts";

                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    RegistryKey kdfjl = hklm.OpenSubKey(regPath);
                    try
                    {

                        foreach (var a in kdfjl.GetSubKeyNames())
                        {
                            RegistryKey Key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts" + "\\" + a);

                            //var last = Key.SubKeyCount;
                            int c = 0;
                            if (Key.GetSubKeyNames().Contains("OpenWithList"))
                            {
                                c += Key.OpenSubKey("OpenWithList").GetValueNames().Count();

                            }
                            if (Key.GetSubKeyNames().Contains("OpenWithProgids"))
                            {
                                c += Key.OpenSubKey("OpenWithProgids").GetValueNames().Count();

                            }
                            if (Key.GetSubKeyNames().Contains("UserChoice"))
                            {
                                c += Key.OpenSubKey("UserChoice").GetValueNames().Count();
                            }

                            if (c == 0)
                                Dispatcher.Invoke(new Action(() =>
                                {
                                    entries.Add(new RegEntry() { Name = regPath+"\\"+ a, Path = regPath+"\\"+a, Type = "key" });
                                }));

                        }
                    }
                    catch (Exception t)
                    {
                        Console.WriteLine(t);

                    }
                }
            }

            //shared dll
            isSleepChecked = chkapp.Dispatcher.Invoke(new Func<bool?>(
                          () => chkapp.IsChecked));
            if ((bool)isSleepChecked)
            {
                string sharedpath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\SharedDlls";
                if (Environment.Is64BitOperatingSystem)
                {
                    try
                    {
                        using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                        {
                            var subkey = hklm.OpenSubKey(sharedpath);
                            foreach (string ac in subkey.GetValueNames())
                            {
                                try
                                {
                                    File.OpenRead(ac);

                                }
                                catch (Exception e)
                                {
                                    Dispatcher.Invoke(new Action(() =>
                                    {
                                        entries.Add(new RegEntry() { Name = ac, Path = sharedpath, Type = "value" });
                                    }));

                                }

                            }
                        }

                        //Console.WriteLine
                    }
                    catch (Exception e)
                    { }
                }
                else
                {
                    using (RegistryKey kdfjl = Registry.LocalMachine.OpenSubKey(sharedpath))
                    {
                        try
                        {
                            foreach (string ac in kdfjl.GetValueNames())
                            {
                                try
                                {
                                    File.OpenRead(ac);

                                }
                                catch (Exception e)
                                {
                                    Dispatcher.Invoke(new Action(() =>
                                    {
                                        entries.Add(new RegEntry() { Name = ac, Path = sharedpath, Type = "value" });
                                    }));

                                }


                            }
                        }
                        catch (Exception t)
                        {
                            // MessageBox.Show(""+t);

                        }
                    }
                }
            }

            //installer
            isSleepChecked = chkinstaller.Dispatcher.Invoke(new Func<bool?>(
                    () => chkinstaller.IsChecked));
            if ((bool)isSleepChecked)
            {
                string instpath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\Folders";
                if (Environment.Is64BitOperatingSystem)
                {
                    try
                    {
                        using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                        {
                            var a = hklm.OpenSubKey(instpath);

                            foreach (string ac in a.GetValueNames())
                            {

                                try
                                {
                                    if (!Directory.Exists(ac))
                                        Dispatcher.Invoke(new Action(() =>
                                        {
                                            entries.Add(new RegEntry() { Name = ac, Path = instpath, Type = "value" });
                                        }));

                                }
                                catch (Exception e)
                                {
                                    //Console.WriteLine(e.Message);
                                }

                            }
                        }

                        //Console.WriteLine
                    }
                    catch (Exception e)
                    { }
                }
                else
                {
                    using (RegistryKey kdfjl = Registry.LocalMachine.OpenSubKey(instpath))
                    {
                        try
                        {
                            foreach (string ac in kdfjl.GetValueNames())
                            {
                                if (!Directory.Exists(ac))
                                    Dispatcher.Invoke(new Action(() =>
                                    {
                                        entries.Add(new RegEntry() { Name = ac, Path = instpath, Type = "value" });
                                    }));
                            }
                        }
                        catch (Exception t)
                        {
                            // MessageBox.Show(""+t);

                        }
                    }
                }

            }
            //applications
            isSleepChecked = chkp.Dispatcher.Invoke(new Func<bool?>(
                          () => chkp.IsChecked));
            if ((bool)isSleepChecked)
            {
                string regPath = @"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store";
                if (Environment.Is64BitOperatingSystem)
                {
                    try
                    {
                        using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                        {
                            var a = hklm.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store");

                            foreach (string ac in a.GetValueNames())
                            {

                                try
                                {
                                    File.OpenRead(ac);
                                    ////Console.WriteLine(ac);
                                }
                                catch (Exception e)
                                {
                                    Dispatcher.Invoke(new Action(() =>
                                    {
                                        entries.Add(new RegEntry() { Name = ac, Path = regPath, Type = "value" });
                                    }));
                                }

                            }
                        }

                        //Console.WriteLine
                    }
                    catch (Exception e)
                    { }

                }
                else
                {
                    using (RegistryKey kdfjl = Registry.CurrentUser.OpenSubKey(regPath))
                    {
                        try
                        {
                            foreach (string ac in kdfjl.GetValueNames())
                            {

                                try
                                {
                                    File.OpenRead(ac);
                                    ////Console.WriteLine(ac);
                                }
                                catch (Exception e)
                                {
                                    Dispatcher.Invoke(new Action(() =>
                                    {
                                        entries.Add(new RegEntry() { Name = ac, Path = regPath, Type = "value" });
                                    }));
                                }


                            }
                        }
                        catch (Exception t)
                        {
                            // MessageBox.Show(""+t);

                        }
                    }
                }
            }
        }

        public class RegEntry : INotifyPropertyChanged
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
                        this.NotifyPropertyChanged("Name");
                    }
                }
            }

            private string path;
            public string Path
            {
                get { return this.path; }
                set
                {

                    if (this.path != value)
                    {
                        this.path = value;
                        this.NotifyPropertyChanged("Path");
                    }
                }
            }

            private string type;
            public string Type
            {
                get { return this.type; }
                set
                {

                    if (this.type != value)
                    {
                        this.type = value;
                        this.NotifyPropertyChanged("Tyoe");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string propName)
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void pbscan_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void lstall_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }

}
