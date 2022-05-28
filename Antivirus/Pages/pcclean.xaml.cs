using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for pcclean.xaml
    /// </summary>
    public partial class pcclean : UserControl
    {
        public pcclean()
        {
            InitializeComponent();
        }
        static string unametemp = "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Temp";
        string gettem = System.IO.Path.GetTempPath();
        static string unamethumb = "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Microsoft\\Windows\\Explorer";
        static string unametask = "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\Recent\\AutomaticDestinations";
        static string unameWER = @"C:\Users\\" + Environment.UserName + "\\AppData\\Local\\Microsoft\\Windows\\WER";
        static string wintemp = @"C:\WINDOWS\Temp";
        ObservableCollection<CleanEntry> entries = null;
        int j = 0;
        int k = 0;
        int l = 0;
        int n = 0;
        int m = 0;
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow); const int SW_HIDE = 0; const int SW_SHOW = 5;


        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }


        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            btnscan.IsEnabled = false;
            // MessageBox.Show(unametemp);
            entries = new ObservableCollection<CleanEntry>();
            pbar.Value = 0;
            pbar.IsIndeterminate = true;
            lstfilename.ItemsSource = entries;
            pbar.Background = Brushes.Transparent;
            await Task.Run(() => getlist());

            if (entries.Count == 0)
            {
                //var dlg = new ModernDialog
                //{

                //    Content = "No issues found",
                //   // Foreground = Brushes.Brown
                //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                //};
                //dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton };
                //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                //// dlg.OkButton.FontWeight = 
                //dlg.OkButton.FontWeight = FontWeights.Bold;
                //dlg.ShowDialog();
                MyPopup bn = new MyPopup();
                bn.txt.Text = "No Issues Found";
                bn.ShowDialog();

            }
            pbar.IsIndeterminate = false;
            btnfix.IsEnabled = true;
            btnscan.IsEnabled = true;

        }

        public void getlist()
        {
            try
            {
                bool? isSleepCheckedd = chkstartshortcut.Dispatcher.Invoke(new Func<bool?>(
                           () => chkstartshortcut.IsChecked));
                if ((bool)isSleepCheckedd)
                {
                    string[] j = Directory.GetFiles(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs");
                    foreach (string jh in j)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = jh.ToString() });
                        }));
                    }
                }




                bool? isSleepChecked = chktemp.Dispatcher.Invoke(new Func<bool?>(
                             () => chktemp.IsChecked));


                if ((bool)isSleepChecked)
                {
                    string[] files = Directory.GetFiles(unametemp);
                    string[] dir = Directory.GetDirectories(unametemp);
                    string[] winfiles = Directory.GetFiles(wintemp);
                    string[] windir = Directory.GetDirectories(wintemp);
                    foreach (string hds in files)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = hds.ToString() });
                        }));
                    }
                    foreach (string c in dir)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = c.ToString() });
                        }));
                    }

                    // temp files in windows folder 

                    foreach (string hds in winfiles)
                    {
                       // MessageBox.Show(hds);
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = hds.ToString() });
                        }));
                    }
                    foreach (string c in windir)
                    {
                       // MessageBox.Show(c);
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = c.ToString() });
                        }));
                    }
                }

                bool? isSleepChecked1 = chkWER.Dispatcher.Invoke(new Func<bool?>(
                             () => chkWER.IsChecked));
                if ((bool)isSleepChecked1)
                {

                    string[] filepath = Directory.GetFiles(unameWER);
                    string[] dirpref = Directory.GetDirectories(unameWER);
                    foreach (string fg in filepath)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = fg.ToString() });
                        }));

                    }

                    foreach (string c in dirpref)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = c.ToString() });
                        }));
                    }
                }
                isSleepChecked = chkpref.Dispatcher.Invoke(new Func<bool?>(
                             () => chkpref.IsChecked));
                if ((bool)isSleepChecked)
                {

                    string[] filepath = Directory.GetFiles(@"C:\Windows\Prefetch");
                    string[] dirpref = Directory.GetDirectories(@"C:\Windows\Prefetch");
                    foreach (string fg in filepath)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = fg.ToString() });
                        }));

                    }

                    foreach (string c in dirpref)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = c.ToString() });
                        }));
                    }
                }
                isSleepChecked = chkmem.Dispatcher.Invoke(new Func<bool?>(
                             () => chkmem.IsChecked));
                if ((bool)isSleepChecked)
                {
                    if (Directory.Exists(@"C:\Windows\Minidump"))
                    {
                        string[] fpath = Directory.GetFiles(@"C:\Windows\Minidump");

                        foreach (string fdj in fpath)
                        {
                            Dispatcher.Invoke(new Action(() =>
                            {
                                entries.Add(new CleanEntry() { Name = fdj.ToString() });
                            }));

                        }
                    }
                }
                isSleepChecked = chkthumb.Dispatcher.Invoke(new Func<bool?>(
                            () => chkthumb.IsChecked));
                if ((bool)isSleepChecked)
                {
                    string[] thumbpath = Directory.GetFiles(unamethumb);
                    string[] dirthumb = Directory.GetDirectories(unamethumb);
                    foreach (string fdj in thumbpath)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = fdj.ToString() });
                        }));

                    }
                    foreach (string c in dirthumb)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = c.ToString() });
                        }));
                    }
                }

                isSleepChecked = chktask.Dispatcher.Invoke(new Func<bool?>(
                           () => chktask.IsChecked));
                if ((bool)isSleepChecked)
                {
                    string[] pathtask = Directory.GetFiles(unametask);
                    string[] dirtask = Directory.GetDirectories(unametask);
                    foreach (string fdj in pathtask)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = fdj.ToString() });
                        }));

                    }
                    foreach (string fdj in dirtask)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = fdj.ToString() });
                        }));

                    }
                }

                isSleepChecked = chkWER.Dispatcher.Invoke(new Func<bool?>(
                          () => chkWER.IsChecked));
                if ((bool)isSleepChecked)
                {
                    string[] files = Directory.GetFiles(unameWER);
                    string[] dir = Directory.GetDirectories(unameWER);

                    foreach (string hds in files)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = hds.ToString() });
                        }));
                        //MessageBox.Show(""+hds);
                    }
                    foreach (string c in dir)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.Add(new CleanEntry() { Name = c.ToString() });
                        }));
                        //MessageBox.Show(c);
                    }
                }

                //MessageBox.Show(entries.Count.ToString());
                for (int i = 0; i < entries.Count; i++)
                {
                    try
                    {
                        using (Stream stream = new FileStream(entries[i].Name, FileMode.Open))
                        {
                            // File/Stream manipulating code here
                        }
                    }
                    catch
                    {
                        //check here why it failed and ask user to retry if the file is in use.
                        Dispatcher.Invoke(new Action(() =>
                        {
                            entries.RemoveAt(i);
                        }));

                    }
                }
                //MessageBox.Show(entries.Count.ToString());
            }
            catch (Exception exxc)
            {
                //MessageBox.Show(exxc.ToString()); 
            }
        }





        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {

            pbar.Visibility = Visibility.Visible;
            pbar.IsIndeterminate = false;
            pbar.Minimum = 0;
            pbar.Maximum = entries.Count;
            btnfix.IsEnabled = false;
            await Task.Run(() => clearList());
            //var dlgwarn = new ModernDialog
            //{

            //    Content = "Some files could not be deleted as they are in use.",
            //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF175DB2"))
            //};
            //dlgwarn.Buttons = new System.Windows.Controls.Button[] { dlgwarn.OkButton };
            //dlgwarn.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
            //dlgwarn.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
            //// dlg.OkButton.FontWeight = 
            //dlgwarn.OkButton.FontWeight = FontWeights.Bold;
            //dlgwarn.ShowDialog();

            MyPopup kk = new MyPopup();
            kk.txt.Text = "Process error:"+Environment.NewLine+"The operation cannot be completed as some files are in use and cannot be deleted.Please make sure to close all programs and files, then try again.";
            kk.ShowDialog();
            btnfix.IsEnabled = true;

        }

        public void clearList()
        {
            for (int i = entries.Count - 1; i >= 0; i--)
            {
                try
                {

                    FileAttributes attr = File.GetAttributes(entries[i].Name);
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        Directory.Delete(entries[i].Name, true);
                    else
                    {
                        File.SetAttributes(entries[i].Name, FileAttributes.Normal);
                        File.Delete(entries[i].Name);
                    }

                }

                catch (Exception er)
                {
                    // MessageBox.Show("" + er);
                }

                Dispatcher.Invoke(new Action(() =>
                {
                    entries.RemoveAt(i);
                    pbar.Value = pbar.Maximum - i;
                    //MessageBox.Show(pbar.Value+"");
                }));

            }

        }




        public class CleanEntry : INotifyPropertyChanged
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

            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string propName)
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void lstfilename_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void chkWER_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
