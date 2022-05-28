using Antivirus.Pages;
using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

namespace home.Pages
{
    /// <summary>
    /// Interaction logic for tools.xaml
    /// </summary>
    public partial class tools : UserControl
    {

        ObservableCollection<ToolEntry> entries;// = null;

        public tools()
        {
            InitializeComponent();

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private async void Tile_Click_1(object sender, RoutedEventArgs e)
        {

            try
            {

                lstfilename.ItemsSource = entries;
                pb.IsIndeterminate = true;
                entries = new ObservableCollection<ToolEntry>();

                lstfilename.ItemsSource = entries;
                await Task.Run(() => getList());

                pb.IsIndeterminate = false;
            }
            catch (Exception hj)
            { }

        }

        private void Tile_Click_2(object sender, RoutedEventArgs e)
        {
            if (lstfilename.SelectedItem == null)
            {
                //var dlg = new ModernDialog
                //{

                //    Content = "Select an  Option to Uninstall",
                //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                //};
                //dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton };
                //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                //// dlg.OkButton.FontWeight = 
                //dlg.OkButton.FontWeight = FontWeights.Bold;
                //dlg.ShowDialog();
                MyPopup kjk = new MyPopup();
                kjk.txt.Text = "Select an  Option to Uninstall";
                kjk.Show();
            }
            else
            {
                try
                {
                    ToolEntry jf = (ToolEntry)lstfilename.SelectedItem;

                    Process process = new Process();
                    process.StartInfo.FileName = "CMD.exe";
                    process.StartInfo.Arguments = "/K" + jf.Path;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.Start();

                }
                catch (Exception khf)
                {
                    // MessageBox.Show(khf.ToString());
                }
            }
        }

        public void getList()
        {
            Dictionary<string, string> programs = new Dictionary<string, string>();

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

                                var displayName = sk.GetValue("DisplayName");
                                var path = sk.GetValue("UninstallString");
                                var size = sk.GetValue("EstimatedSize");


                                if (displayName != null && path != null)
                                {
                                    //MessageBox.Show(displayName + "======>>>" + path);
                                    try
                                    {

                                        Dispatcher.Invoke(new Action(() =>
                                        {
                                            entries.Add(new ToolEntry() { Name = displayName.ToString(), Path = path.ToString() });
                                        }));
                                    }
                                    catch (Exception i)
                                    {
                                        // MessageBox.Show(i.Message);
                                    }



                                    Console.WriteLine(displayName + "====>" + path);
                                }
                            }
                            catch (Exception ex)
                            {
                                // MessageBox.Show(ex.Message);
                            }

                        }

                    }

                }


            }

            // for current user 
            using (var baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
            {
                using (var key = baseKey.OpenSubKey(uninstallKey))
                {
                    foreach (string skName in key.GetSubKeyNames())
                    {
                        using (RegistryKey sk = key.OpenSubKey(skName))
                        {
                            try
                            {

                                var displayName = sk.GetValue("DisplayName");
                                var path = sk.GetValue("UninstallString");
                                var size = sk.GetValue("EstimatedSize");


                                if (displayName != null && path != null)
                                {
                                    //MessageBox.Show(displayName + "======>>>" + path);
                                    try
                                    {

                                        Dispatcher.Invoke(new Action(() =>
                                        {
                                            entries.Add(new ToolEntry() { Name = displayName.ToString(), Path = path.ToString() });
                                        }));
                                    }
                                    catch (Exception i)
                                    {
                                        // MessageBox.Show(i.Message);
                                    }



                                    Console.WriteLine(displayName + "====>" + path);
                                }
                            }
                            catch (Exception ex)
                            {
                                // MessageBox.Show(ex.Message);
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

                                var displayName = sk.GetValue("DisplayName");
                                var path = sk.GetValue("UninstallString");
                                var size = sk.GetValue("EstimatedSize");


                                if (displayName != null && path != null)
                                {
                                    //MessageBox.Show(displayName + "======>>>" + path);
                                    try
                                    {

                                        Dispatcher.Invoke(new Action(() =>
                                        {
                                            entries.Add(new ToolEntry() { Name = displayName.ToString(), Path = path.ToString() });
                                        }));
                                    }
                                    catch (Exception i)
                                    {
                                        // MessageBox.Show(i.Message);
                                    }



                                    Console.WriteLine(displayName + "====>" + path);
                                }
                            }
                            catch (Exception ex)
                            {
                                // MessageBox.Show(ex.Message);
                            }

                        }

                    }

                }


            }
            // MessageBox.Show(entries.Count.ToString());
        }

        public class ToolEntry : INotifyPropertyChanged
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

            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string propName)
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
