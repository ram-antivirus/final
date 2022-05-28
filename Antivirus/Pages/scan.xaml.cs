using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
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
//using System.Windows.Shapes;
using MahApps.Metro;
using System.Collections.Concurrent;
using System.Threading;
using Counter;
using MSNetwork4Demo2;
using nClam;
using System.IO;
using FullScanCounter;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Security.Cryptography;
using AccessFile;
using FirstFloor.ModernUI.Windows.Controls;
using FilePathNameSpace;
using System.Windows.Threading;
using System.Diagnostics;
using System.Collections;
using System.Timers;
using System.ServiceProcess;

namespace Antivirus.Pages
{
    /// <summary>
    /// Interaction logic for scan.xaml
    /// </summary>
    public partial class scan : UserControl
    {
        DispatcherTimer dt = new DispatcherTimer();
        Stopwatch sw = new Stopwatch();
        string currentTime = string.Empty;
        String TargetPath;
        PauseTokenSource m_pauseTokeSource;
        CancellationTokenSource cancellationTokenSource;
        CancellationToken token;
        private Action _cancelwork;
        private BlockingCollection<Info> bc;
        public ObservableCollection<FilePath> DefectedItems;
        ArrayList Array = new ArrayList();
        string[] s;
        public static int fullscangoto;
        int count = 0;
        System.Collections.Specialized.StringCollection Array1 = new System.Collections.Specialized.StringCollection();

        private System.Timers.Timer timer1 = null;

        int c;
        int fullcount;
        int customCount ;
        string[] QuickScanPaths;
     

        public scan()
        {
            InitializeComponent();
            CustomScanProgressBar.Visibility = Visibility.Hidden;
            FullScanProgressBar.Visibility = Visibility.Hidden;
            QuickScanProgressBar.Visibility = Visibility.Hidden;
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);

           

            if (App.ashu.Contains(@"\"))
            {
                CustomScanTab.IsSelected = true;
                TargetPath = App.ashu;
                // tot.Text=Antivirus.Properties.Settings.Default.TotalScanned.ToString();



            }
            else
            {

                QuickScanTab.IsSelected = true;
            }

        }


     
        void dt_Tick(object sender, EventArgs e)
        {
            if (sw.IsRunning)
            {
                TimeSpan ts = sw.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Hours, ts.Minutes, ts.Seconds);
                timeTot.Text = currentTime;
            }
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            Process myProcess = Process.GetCurrentProcess();
            if (myProcess.Responding)
            {
                var counter = new PerformanceCounter("Process", "% Processor Time", myProcess.ProcessName);
            //    MessageBox.Show("Status = Running");
            //    MessageBox.Show(myProcess.ProcessName);
            //    MessageBox.Show(myProcess.ProcessName + "       | CPU% " + (Math.Round(counter.NextValue(), 1)));
                int f = (Int32)(Math.Round(counter.NextValue(), 1));
                if (f == 0)
                {
             //       MessageBox.Show("F == 0");
                  //  myProcess.Kill();
 
                }
            }
            else
            {
                MessageBox.Show("Status = Not Responding Shutting Down");
                myProcess.Kill();
            }

        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.browseButton.IsEnabled = true;
            this.customcancel.IsEnabled = false;
            this.custompause.IsEnabled = false;
            this.CustomCurrentPath.Content = App.ashu;
            this.QuickCurrentPath.Content = "";
            this.fullscancurrentpath.Content = "";
        }




        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();

            // Display OpenFileDialog by calling ShowDialog method 
            var result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result.ToString().Equals("OK"))
            {
                TargetPath = dlg.SelectedPath;
                String test;
                if (TargetPath.Length > 64)
                    test = TargetPath.Replace(TargetPath.Substring((TargetPath.Length / 2), TargetPath.Length - 64), "...");
                else
                    test = TargetPath;
                this.CustomCurrentPath.Content = test;
                //Console.WriteLine(test);

            }
        }

        private async void Custom_Scan(object sender, RoutedEventArgs e)
        {
            customCount = 0;
            CustomCurrentPath.Content = "Loading...";
            Antivirus.Properties.Settings.Default.ScanRun = true;
            txtissues.Text = "000";
            try{
                
             ServiceController sc = new ServiceController("ClamD");
                

                switch (sc.Status)
                {
                    case ServiceControllerStatus.Running:
            Antivirus.Properties.Settings.Default.ScanDate = DateTime.Now.ToShortDateString();
            Antivirus.Properties.Settings.Default.ScanTime = DateTime.Now.ToShortTimeString();
            viewReport.Visibility = Visibility.Visible;
            CustomScanProgressBar.Visibility = Visibility.Visible;
            QuickScanTab.IsEnabled = false;
            FullScanTab.IsEnabled = false;
            customscan.IsEnabled = false;
            if (TargetPath != null)
            {
                if (!Directory.Exists(TargetPath))
                {
                    //var dlg = new ModernDialog
                    //{
                    //    Content = "Please choose a valid path!",
                    //    Background = Brushes.Blue,
                    //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                    //};
                    //dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton };
                    //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                    //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                    //// dlg.OkButton.FontWeight = 
                    //dlg.OkButton.FontWeight = FontWeights.Bold;
                    //dlg.ShowDialog();
                    //MyPopup kk = new MyPopup();
                    //kk.txt.Text = "Please choose a valid path!";
                    //kk.Show();



                }
            }
            else
            {
                // var dlg = new ModernDialog
                // {
                //     Content = "Choose a folder!",
                //     Background = Brushes.Blue,
                //     Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                // };
                // dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton };
                // dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                // dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                //// dlg.OkButton.FontWeight = 
                // dlg.OkButton.FontWeight = FontWeights.Bold;
                // dlg.ShowDialog();
                MyPopup kk = new MyPopup();
                kk.txt.Text = "Choose a folder!";
                kk.Show();
                customscan.IsEnabled = true;
                CustomScanProgressBar.Visibility = Visibility.Hidden;
                CustomCurrentPath.Content = "";
                return;

            }
            sw.Reset();
            TimeSpan ts = sw.Elapsed;
            currentTime = String.Format("{0:00}:{1:00}:{2:00}",
            ts.Hours, ts.Minutes, ts.Seconds);
            timeTot.Text = currentTime;

            sw.Start();
            dt.Start();
            Task task;
            Progress<string> scanReport;

            bc = new BlockingCollection<Info>();
            DefectedItems = new ObservableCollection<FilePath>();
            DefectedItems.Clear();
            this.CustomDefectedItemsList.ItemsSource = DefectedItems;
            this.m_pauseTokeSource = new PauseTokenSource();
            this.customscan.IsEnabled = false;
            //this.FullScanTab.IsEnabled = false;
            //this.QuickScanTab.IsEnabled = false;
            this.CustomScanProgressBar.Minimum = 0;
            this.CustomScanProgressBar.Maximum = 100;
            this.CustomScanProgressBar.Value = 0;
            this.custompause.IsEnabled = true;
            this.customcancel.IsEnabled = true;
            this.custompause.Content = "||";
            this.customdelete.IsEnabled = false;
            this.CustomCurrentPath.Content = "";
            c = 0;

            try
            {
                this._cancelwork = (Action)(() =>
                {
                    this.customcancel.IsEnabled = false;
                    this.customscan.IsEnabled = true;
                    this.custompause.IsEnabled = false;
                    this.m_pauseTokeSource.IsPaused = false;

                    this.TargetPath = null;

                    cancellationTokenSource.Cancel();

                });

                cancellationTokenSource = new CancellationTokenSource();
                token = cancellationTokenSource.Token;

                Counter.Class1 m = new Counter.Class1();

                task = Task.Run(() => m.RecursiveScan2(TargetPath, bc, token), token).ContinueWith((antecedent) =>
                {
                    this.CustomScanProgressBar.Maximum = antecedent.Result;

                    bc.CompleteAdding();
                }, TaskScheduler.FromCurrentSynchronizationContext());

                scanReport = new Progress<string>((Action<string>)(i =>
                {
                    if (task.IsCompleted)
                        this.CustomScanProgressBar.Value = (double)++c;

                    String test = i;
                    try
                    {
                        if (i.Length > 64)
                        {
                            var l = test.Length - 64;
                            //Console.WriteLine(i + ">>>" + l);
                            test = test.Replace(test.Substring(30, test.Length - 60), "...");
                        }
                    }
                    catch (Exception er)
                    {

                    }
                    this.CustomCurrentPath.Content = test;
                    customCount++;

                   // this.totFiles.Text = c.ToString();
                    this.totFiles.Text = customCount.ToString();
                    Antivirus.Properties.Settings.Default.TotScanHome = customCount;
                }));

                var scanTask = Task.Run(() => this.Scan(token, scanReport, this.m_pauseTokeSource.Token), token).ContinueWith((antecedent) =>
                {
                    this.customscan.IsEnabled = false;
                    this.customcancel.IsEnabled = false;
                    this.customscan.IsEnabled = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());

                await Task.WhenAll(task, scanTask);


                //Results r = new Results() {TotalScanned = c,TotalIssues = DefectedItems.Count};
                //var json = new JavaScriptSerializer().Serialize(r);
                //FileReadWrite.WriteFile(@"E:\\j.abc",json.ToString());
                //var s = FileReadWrite.ReadFile(@"E:\\j.abc");

                //r = JsonConvert.DeserializeObject<Results>(s);
                //MessageBox.Show(r.TotalScanned+" "+r.TotalIssues);

                Antivirus.Properties.Settings.Default.TotScanHome = c;
                Antivirus.Properties.Settings.Default.Totalssues = DefectedItems.Count;

               
                this.quickdelete.IsEnabled = true;

                //var dlg = new ModernDialog
                //{
                //    Content = "Scan Completed!",
                //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))

                //};
                //dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton };
                //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                //// dlg.OkButton.FontWeight = 
                //dlg.OkButton.FontWeight = FontWeights.Bold;
                //dlg.ShowDialog();

                MyPopup kjk = new MyPopup();
                kjk.txt.Text = "Scan has Completed Successfully. Click on View Report to get the Summary of detected threats!";
                kjk.ShowDialog();
                viewReport.Visibility = Visibility.Visible;
                sw.Stop();
                CustomScanProgressBar.Visibility = Visibility.Hidden;

                Cqurantile.IsEnabled = true;
                customdelete.IsEnabled = true;
                viewReport.IsEnabled = true;

                //txtScanned.Text = Antivirus.Properties.Settings.Default.TotalScanned.ToString();
                txtissues.Text = Antivirus.Properties.Settings.Default.Totalssues.ToString();
                Antivirus.Properties.Settings.Default.ScanRun = false;
                this.FullScanTab.IsEnabled = true;
                this.QuickScanTab.IsEnabled = true;
                CustomCurrentPath.Content = "";
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
             //   MessageBox.Show(ex.Message);
            }

            this.custompause.IsEnabled = false;
            this.customcancel.IsEnabled = false;
            this.customdelete.IsEnabled = true;
            this._cancelwork = (Action)null;
            sw.Stop();
                                       break;
                    case ServiceControllerStatus.Stopped:
                                       MessageBox.Show("Something Goes Wrong");
                                       break;
                   
                    case ServiceControllerStatus.StartPending:
                                       MessageBox.Show("Initializing........");
                                       break;
                    default:
                                    //   fullScanCustom();
                                       break;
                }


            }
            catch (Exception h)
            { }

         
        }

        private void Custom_Cancel(object sender, RoutedEventArgs e)
        {
            if (this._cancelwork == null)
                return;
            this._cancelwork();
            sw.Stop();
        }

        private void Custom_Pause(object sender, RoutedEventArgs e)
        {

            this.m_pauseTokeSource.IsPaused = !this.m_pauseTokeSource.IsPaused;
            if (this.m_pauseTokeSource.IsPaused)
            {
                this.custompause.Content = (object)">";
                sw.Stop();
            }

            else
            {
                this.custompause.Content = (object)"||";
                sw.Start();
            }
            return;

        }

        private void Custom_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                sw.Stop();
             
                if (CustomDefectedItemsList.SelectedItems.Count > 0)
                {
                    for (int i = CustomDefectedItemsList.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        var file = ((FilePath)CustomDefectedItemsList.SelectedItems[i]).File_Path;

                        DefectedItems.Remove(DefectedItems.Where(item => item.File_Path == file).Single());

                        System.IO.File.Delete(file);
                        txtissues.Text = DefectedItems.Count.ToString();

                    }



                    MyPopup hjh = new MyPopup();
                    hjh.txt.Text = "Virus successfully deleted..";
                    hjh.ShowDialog();
                }

                else {
                    MyPopup hjh = new MyPopup();
                    hjh.txt.Text = "Please select a file..";
                    hjh.ShowDialog();
                
                }
            }
            catch (Exception k)
            {

            }
            //txtissues.Text = "000";
        }

//        public void Clear<T>( BlockingCollection<T> blockingCollection)
//        {
//            if (blockingCollection == null)
//            {
//                throw new ArgumentNullException("blockingCollection");
//            }
//            int i = 0;
//            int cou = blockingCollection.Count;
//            MessageBox.Show("count = " + cou.ToString());
//            while (blockingCollection.Count >= 0)
//            {
//                T item;
//                blockingCollection.TryTake(out item
//);
              
//                MessageBox.Show(item.ToString());
//            }
//        }

        private async void Full_Scan(object sender, RoutedEventArgs e)
        {
            
            try
            {
                
                fullcount = 0;
                txtissues.Text = "000";
                fullscancurrentpath.Content = "Loading....";
                Antivirus.Properties.Settings.Default.ScanRun = true;
                QuickScanTab.IsEnabled = false;
                CustomScanTab.IsEnabled = false;
                 ServiceController sc = new ServiceController("ClamD");

                 switch (sc.Status)
                 {
                     case ServiceControllerStatus.Running:
                         fullScanCustom();
                         break;
                     case ServiceControllerStatus.StartPending:
                         MessageBox.Show("Initializing RAM Antivirus....... Please wait");
                         break;
                     case ServiceControllerStatus.Stopped :
                         MessageBox.Show("Something goes wrong!!!!");
                         break;
                     default :
                         fullScanCustom();
                         break;
                 }

                

             


            }
            catch (Exception h)
            {
                MessageBox.Show("Ex" + h);
            }
          

            //Antivirus.Properties.Settings.Default.ScanDate = DateTime.Now.ToShortDateString();
            //Antivirus.Properties.Settings.Default.ScanTime = DateTime.Now.ToShortTimeString();
            //viewReportf.Visibility = Visibility.Visible;
            //FullScanProgressBar.Visibility = Visibility.Visible;
            //sw.Reset();
            //TimeSpan ts = sw.Elapsed;
            //currentTime = String.Format("{0:00}:{1:00}:{2:00}",
            //ts.Hours, ts.Minutes, ts.Seconds);
            //timeTot.Text = currentTime;
            //sw.Start();
            //dt.Start();
            
            //this.m_pauseTokeSource = new PauseTokenSource();
            //DefectedItems = new ObservableCollection<FilePath>();
            //DefectedItems.Clear();
            //this.FullScanDefectedList.ItemsSource = DefectedItems;
            //this.fullscan.IsEnabled = false;
            //c = 0;
            //try
            //{
            //    this.FullScanProgressBar.Minimum = 0.0;
            //    this.FullScanProgressBar.Maximum = 100000;
            //    this.fullpause.IsEnabled = true;
            //    this.fullcancel.IsEnabled = true;
            //    this.fullpause.Content = "||";
            //    this.fullscandelete.IsEnabled = false;

            //    this._cancelwork = (Action)(() =>
            //    {
            //        this.fullpause.IsEnabled = false;
            //        this.fullscan.IsEnabled = true;
            //        this.fullcancel.IsEnabled = false;
            //        this.m_pauseTokeSource.IsPaused = false;
            //        this.FullScanProgressBar.Value = 0;
            //        this.TargetPath = null;

            //        cancellationTokenSource.Cancel();
            //    });

            //    cancellationTokenSource = new CancellationTokenSource();
            //    token = cancellationTokenSource.Token;

            //    DriveInfo[] drives = DriveInfo.GetDrives();

            //    BlockingCollection<string> driveCollection = new BlockingCollection<string>();


            //    List<Task<int>> taskList = new List<Task<int>>();

            //    for (int i = 0; i < drives.Length; i++)
            //    {
            //        var a = drives[i].Name.TrimEnd('\\');

            //        if (File.Exists(a.TrimEnd(':') + ".bin"))
            //        {
            //            // MessageBox.Show("Deleting " + a.Trim(':') + ".bin");
            //            System.IO.File.Delete(a.TrimEnd(':') + ".bin");
            //        }
            //        try
            //        {
            //            taskList.Add(Task<int>.Run(() => FullScanCounter.Class1.Count(a, driveCollection), token));
            //        }
            //        catch (Exception ep)
            //        {
            //        }
            //    }

            //    await Task.WhenAny(taskList);

            //    var fullscanreport = new Progress<string>((Action<string>)(i =>
            //    {
            //        String test = i;
            //        try
            //        {
            //            if (i.Length > 64)
            //            {
            //                var l = test.Length - 64;

            //                test = test.Replace(test.Substring(30, test.Length - 60), "...");
            //            }
            //        }
            //        catch (Exception er)
            //        {

            //        }
            //        this.fullscancurrentpath.Content = test;

            //        this.FullScanProgressBar.Value = ++c;
            //        this.totFiles.Text = c.ToString();
            //        Antivirus.Properties.Settings.Default.TotScanHome = c;
            //    }));

            //    int countofloop = 0;
            //    int bsc = 0;
            //    int k = 0;
            //    List<string>[] pathList = new List<string>[50];
            //    foreach (var name in driveCollection.GetConsumingEnumerable())
            //    {
            //        countofloop++;
            //        // MessageBox.Show("Starting " + name + " now");
            //        using (FileStream stream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + name.Substring(0, 1) + ".bin", FileMode.Open))
            //        {
            //            while (stream.Position != stream.Length)
            //            {
            //                BinaryFormatter bin = new BinaryFormatter();
            //                pathList[k] = (List<string>)bin.Deserialize(stream);
            //                //MessageBox.Show(pathList.Count.ToString());
            //              var fullScanTask = Task.Run(() => this.FullScan(pathList[k], token, fullscanreport, this.m_pauseTokeSource.Token), token);
            //              await Task.WhenAll(fullScanTask);


            //              //  MessageBoxResult r = MessageBox.Show("List done", "Message", MessageBoxButton.OK);
            //                //if (r == MessageBoxResult.OK)
            //                //{
            //                //    try
            //                //    {
            //                //        Process myProcess = Process.GetCurrentProcess();
            //                //        if (myProcess.Responding)
            //                //        {
            //                //            //MessageBox.Show("Status = Running");
            //                //        }
            //                //        else
            //                //        {
            //                //            MessageBox.Show("Status = Not Responding");
            //                //        }

            //                //    }
            //                //    catch (Exception j)
            //                //    {
            //                //        MessageBox.Show("j" + j);
            //                //    }
            //                //}

            //                //if (bc == null)
            //                //    break;
            //            }
            //        }
            //        if (countofloop == drives.Length)
            //           break;
            //    }
            //    try
            //    {
            //        await Task.WhenAll(taskList);
            //        var total = 0;
            //        foreach (var task in taskList)
            //        {
            //            total += task.Result;
            //        }
            //        this.FullScanProgressBar.Maximum = total;
            //        // MessageBox.Show(total.ToString());
            //    }
            //    catch (AggregateException ae)
            //    {
            //    }

            //    //var dlg = new ModernDialog
            //    //{
            //    //    Content = "Scan Completed!",
            //    //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
            //    //};
            //    //dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton };
            //    //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
            //    //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
            //    //// dlg.OkButton.FontWeight = 
            //    //dlg.OkButton.FontWeight = FontWeights.Bold;
            //    //dlg.ShowDialog();

            //    MyPopup jt = new MyPopup();
            //    jt.txt.Text = "Scan Complete";
            //    jt.Show();
            //    viewReport.Visibility = Visibility.Visible;
            //    sw.Stop();
            //    FullScanProgressBar.Visibility = Visibility.Hidden;
            //    Antivirus.Properties.Settings.Default.ScanRun = false;
            //    QuickScanTab.IsEnabled = true;
            //    CustomScanTab.IsEnabled = true;

               

            //    //txtScanned.Text = Antivirus.Properties.Settings.Default.TotalScanned.ToString();
            //    // txtissues.Text = Antivirus.Properties.Settings.Default.Totalssues.ToString();
            //}
            //catch (Exception ev)
            //{
            //}
        }

        private async void fullScanCustom()
        {
            Antivirus.Properties.Settings.Default.ScanDate = DateTime.Now.ToShortDateString();
            Antivirus.Properties.Settings.Default.ScanTime = DateTime.Now.ToShortTimeString();
            viewReport.Visibility = Visibility.Visible;
            FullScanProgressBar.Visibility = Visibility.Visible;
            sw.Reset();
            TimeSpan ts = sw.Elapsed;
            currentTime = String.Format("{0:00}:{1:00}:{2:00}",
            ts.Hours, ts.Minutes, ts.Seconds);
            timeTot.Text = currentTime;
            sw.Start();
            dt.Start();
            DefectedItems = new ObservableCollection<FilePath>();
            DefectedItems.Clear();
            this.FullScanDefectedList.ItemsSource = DefectedItems;
            this.FullScanProgressBar.Minimum = 0.0;
            this.FullScanProgressBar.Maximum = 100000;
            this.fullpause.IsEnabled = true;
            this.fullcancel.IsEnabled = true;
            this.fullpause.Content = "||";
            // this.fullscandelete.IsEnabled = false;
            this.fullscancurrentpath.Content = "";
            c = 0;

            string[] drives = System.IO.Directory.GetLogicalDrives();
            foreach (string str in drives)
            {
                // MessageBox.Show(str);
            }
            fullscangoto = 0;

        cleanup:



            Task task;
            Progress<string> scanReport;

            bc = new BlockingCollection<Info>();

            this.m_pauseTokeSource = new PauseTokenSource();
            //    MessageBox.Show(fullscangoto.ToString());
            TargetPath = drives[fullscangoto];
            //     MessageBox.Show("Now Scanning " + TargetPath);
            try
            {
                this._cancelwork = (Action)(() =>
                {
                    this.fullpause.IsEnabled = false;
                    this.fullscan.IsEnabled = true;
                    //   this.fullcancel.IsEnabled = false;
                    this.m_pauseTokeSource.IsPaused = false;
                    this.FullScanProgressBar.Value = 0;
                    this.TargetPath = null;

                    cancellationTokenSource.Cancel();
                });

                cancellationTokenSource = new CancellationTokenSource();
                token = cancellationTokenSource.Token;
                Counter.Class1 m = new Counter.Class1();
                task = Task.Run(() => m.RecursiveScan2(TargetPath, bc, token), token).ContinueWith((antecedent) =>
                {
                    this.CustomScanProgressBar.Maximum = antecedent.Result;

                    bc.CompleteAdding();
                }, TaskScheduler.FromCurrentSynchronizationContext());






                scanReport = new Progress<string>((Action<string>)(i =>
                {
                    if (task.IsCompleted)
                        this.CustomScanProgressBar.Value = (double)++c;

                    String test = i;
                    try
                    {
                        if (i.Length > 64)
                        {
                            var l = test.Length - 64;
                            //Console.WriteLine(i + ">>>" + l);
                            test = test.Replace(test.Substring(30, test.Length - 60), "...");
                        }
                    }
                    catch (Exception er)
                    {

                    }
                    this.fullscancurrentpath.Content = test;
                    fullcount++;
                    //MessageBox.Show(fullcount.ToString());
                    this.totFiles.Text = fullcount.ToString();
                   // this.totFiles.Text = c.ToString();
                    Antivirus.Properties.Settings.Default.TotScanHome = fullcount;
                }));




                var scanTask = Task.Run(() => this.Scan(token, scanReport, this.m_pauseTokeSource.Token), token).ContinueWith((antecedent) =>
                {
                    this.customscan.IsEnabled = false;
                    this.customcancel.IsEnabled = false;
                    this.customscan.IsEnabled = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());



                await Task.WhenAll(task, scanTask);

                fullscangoto++;

                if (fullscangoto < drives.Length)
                {
                    goto cleanup;
                }
                //Results r = new Results() {TotalScanned = c,TotalIssues = DefectedItems.Count};
                //var json = new JavaScriptSerializer().Serialize(r);
                //FileReadWrite.WriteFile(@"E:\\j.abc",json.ToString());
                //var s = FileReadWrite.ReadFile(@"E:\\j.abc");

                //r = JsonConvert.DeserializeObject<Results>(s);
                //MessageBox.Show(r.TotalScanned+" "+r.TotalIssues);

                Antivirus.Properties.Settings.Default.TotalScanned = c;
                Antivirus.Properties.Settings.Default.Totalssues = DefectedItems.Count;

                this.FullScanTab.IsEnabled = true;
                this.QuickScanTab.IsEnabled = true;
                this.quickdelete.IsEnabled = true;

                //var dlg = new ModernDialog
                //{
                //    Content = "Scan Completed!",
                //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))

                //};
                //dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton };
                //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                //// dlg.OkButton.FontWeight = 
                //dlg.OkButton.FontWeight = FontWeights.Bold;
                //dlg.ShowDialog();


                MyPopup kjk = new MyPopup();
                kjk.txt.Text = "Scan has Completed Successfully. Click on View Report to get the Summary of detected threats!";
                kjk.ShowDialog();
                viewReport.Visibility = Visibility.Visible;
                sw.Stop();
                FullScanProgressBar.Visibility = Visibility.Hidden;
                Antivirus.Properties.Settings.Default.ScanRun = false;
                //txtScanned.Text = Antivirus.Properties.Settings.Default.TotalScanned.ToString();
                txtissues.Text = Antivirus.Properties.Settings.Default.Totalssues.ToString();
                QuickScanTab.IsEnabled = true;
                CustomScanTab.IsEnabled = true;
                fullscancurrentpath.Content = "";

            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
                //  MessageBox.Show("Exception of Drive complition" + ex);
            }

            this.custompause.IsEnabled = false;
            this.customcancel.IsEnabled = false;
            this.customdelete.IsEnabled = true;
            this._cancelwork = (Action)null;
            sw.Stop();


        }

        private void Full_Pause(object sender, RoutedEventArgs e)
        {

            this.m_pauseTokeSource.IsPaused = !this.m_pauseTokeSource.IsPaused;
            if (this.m_pauseTokeSource.IsPaused)
            {
                this.fullpause.Content = (object)">";
                sw.Stop();
            }
            else
            {
                this.fullpause.Content = (object)"||";
                sw.Start();
            }
            return;
        }

        private void fullcancel_Click(object sender, RoutedEventArgs e)
        {
            sw.Stop();
            if (this._cancelwork == null)
                return;
            this._cancelwork();
            FullScanProgressBar.Visibility = Visibility.Hidden;
        }

        private void Full_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                sw.Stop();
                if (FullScanDefectedList.SelectedItems.Count>0)
                {
                for (int i = FullScanDefectedList.SelectedItems.Count - 1; i >= 0; i--)
                {
                    var file = ((FilePath)FullScanDefectedList.SelectedItems[i]).File_Path;

                    
                    DefectedItems.Remove(DefectedItems.Where(item => item.File_Path == file).Single());
                    System.IO.File.Delete(file);
                    txtissues.Text = DefectedItems.Count.ToString();
                }

                MyPopup hjh = new MyPopup();
                hjh.txt.Text = "Virus successfully deleted..";
                hjh.ShowDialog();
                }
                else{
                MyPopup hjh = new MyPopup();
                    hjh.txt.Text = "Please select a file..";
                    hjh.ShowDialog();
                
                }
            }
            catch (Exception l)
            {

            }
            //txtissues.Text = "000";
        }

        private void Quick_Pause(object sender, RoutedEventArgs e)
        {

            try
            {
                this.m_pauseTokeSource.IsPaused = !this.m_pauseTokeSource.IsPaused;
                if (this.m_pauseTokeSource.IsPaused)
                {
                    this.quickpause.Content = (object)">";
                    sw.Stop();
                }
                else
                {
                    this.quickpause.Content = (object)"||";
                    sw.Start();
                }
            }
            catch (Exception kj)
            {

            }
            return;
        }

        private async void Quick_Scan(object sender, RoutedEventArgs e)
        {
            QuickCurrentPath.Content = "Loading.....";
            txtissues.Text = "000";
            Antivirus.Properties.Settings.Default.ScanRun = true;

           
            ServiceController sc = new ServiceController("ClamD");

            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    try
                    {

                        Antivirus.Properties.Settings.Default.ScanDate = DateTime.Now.ToShortDateString();
                        Antivirus.Properties.Settings.Default.ScanTime = DateTime.Now.ToShortTimeString();
                        viewReportq.Visibility = Visibility.Visible;
                        QuickScanProgressBar.Visibility = Visibility.Visible;
                        sw.Reset();
                        TimeSpan ts = sw.Elapsed;
                        currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                        ts.Hours, ts.Minutes, ts.Seconds);
                        timeTot.Text = currentTime;
                        sw.Start();
                        dt.Start();
                        Progress<string> scanReport;

                        Task task;
                        bc = new BlockingCollection<Info>();

                        QuickScanPaths = new string[] { Environment.GetEnvironmentVariable("SystemRoot") + "\\System32", "C:\\Boot", System.IO.Path.GetTempPath() };
                        DefectedItems = new ObservableCollection<FilePath>();
                        DefectedItems.Clear();
                        this.QuickDefectedList.ItemsSource = DefectedItems;
                        this.m_pauseTokeSource = new PauseTokenSource();
                        this.quickscan.IsEnabled = false;
                        try
                        {
                            quickscan.IsEnabled = false;
                            FullScanTab.IsEnabled = false;
                            CustomScanTab.IsEnabled = false;
                            this.QuickScanProgressBar.Minimum = 0.0;
                            this.QuickScanProgressBar.Maximum = 50000;
                            this.quickpause.IsEnabled = true;
                            this.quickcancel.IsEnabled = true;
                            // this.quickdelete.IsEnabled = false;
                            this.quickpause.Content = "||";
                            this.QuickCurrentPath.Content = "";

                            this._cancelwork = (Action)(() =>
                            {
                                this.quickpause.IsEnabled = false;
                                this.quickscan.IsEnabled = true;
                                this.quickcancel.IsEnabled = false;
                                this.m_pauseTokeSource.IsPaused = false;
                                this.QuickScanProgressBar.Value = 0;
                                this.TargetPath = null;
                                cancellationTokenSource.Cancel();
                            });

                            cancellationTokenSource = new CancellationTokenSource();
                            token = cancellationTokenSource.Token;

                            Counter.Class1 m = new Counter.Class1();

                            task = Task.Run(() =>
                                   Parallel.ForEach(QuickScanPaths, path =>
                                   {
                                       m.RecursiveScan2(path, bc, token);
                                   })
                           );

                            Task<int>[] QuickCountTaskArray = new Task<int>[QuickScanPaths.Length];

                            c = 0;

                            scanReport = new Progress<string>((Action<string>)(i =>
                            {

                                this.QuickScanProgressBar.Value = (double)++c;
                                String test = i;
                                try
                                {
                                    if (i.Length > 64)
                                    {
                                        var l = test.Length - 64;

                                        test = test.Replace(test.Substring(30, test.Length - 60), "...");
                                    }
                                }
                                catch (Exception er)
                                {

                                }
                                this.QuickCurrentPath.Content = test;
                                this.totFiles.Text = c.ToString();
                                Antivirus.Properties.Settings.Default.TotScanHome = c;

                            }));

                            var scanTask = Task.Run(() => this.Scan(token, scanReport, this.m_pauseTokeSource.Token), token).ContinueWith((antecedent) =>
                            {
                                this.quickscan.IsEnabled = true;
                                this.quickpause.IsEnabled = false;
                                this.quickcancel.IsEnabled = false;

                                //var dlg = new ModernDialog
                                //{
                                //    Content = "Scan Complete!",
                                //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                                //};
                                //dlg.Buttons = new System.Windows.Controls.Button[] { dlg.OkButton };
                                //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                                //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                                //// dlg.OkButton.FontWeight = 
                                //dlg.OkButton.FontWeight = FontWeights.Bold;
                                //dlg.ShowDialog();

                                MyPopup kjj = new MyPopup();
                                kjj.txt.Text = "Scan has Completed Successfully. Click on View Report to get the Summary of detected threats!";
                                kjj.ShowDialog();

                                QbtnSelall.IsEnabled = true;
                                quickdelete.IsEnabled = true;
                                viewReportq.IsEnabled = true;

                                viewReport.Visibility = Visibility.Visible;
                                sw.Stop();
                                QuickScanProgressBar.Visibility = Visibility.Hidden;
                                Antivirus.Properties.Settings.Default.ScanRun = false;
                                FullScanTab.IsEnabled = true;
                                CustomScanTab.IsEnabled = true;
                                QuickCurrentPath.Content = "";
                                //txtScanned.Text = Antivirus.Properties.Settings.Default.TotalScanned.ToString();
                                //txtissues.Text = Antivirus.Properties.Settings.Default.Totalssues.ToString();

                            }, TaskScheduler.FromCurrentSynchronizationContext());
                            await Task.WhenAny(task);
                            bc.CompleteAdding();
                            this.QuickScanProgressBar.Maximum = bc.Count;







                            await Task.WhenAny(scanTask);


                            Antivirus.Properties.Settings.Default.TotalScanned = c;
                            Antivirus.Properties.Settings.Default.Totalssues = DefectedItems.Count;





                            this.custompause.IsEnabled = false;
                            this.customcancel.IsEnabled = false;
                            this._cancelwork = (Action)null;
                            this.quickdelete.IsEnabled = true;

                            return;
                        }
                        catch (OperationCanceledException)
                        {

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    catch (Exception l)
                    {

                    }
                    break;
                case ServiceControllerStatus.Stopped:
                    MessageBox.Show("Something Goes Wrong");
                    break;

                case ServiceControllerStatus.StartPending:
                    MessageBox.Show("Initializing........");
                    break;
                default:
                  //  fullScanCustom();
                    break;
            }


           

        }

        private void quickcancel_Click(object sender, RoutedEventArgs e)
        {
            sw.Stop();
            if (this._cancelwork == null)
                return;
            this._cancelwork();
        }

        public async Task<int> Scan(CancellationToken token, Progress<string> progressHandler, PauseToken pauseToken)
        {
            IProgress<string> progress = (IProgress<string>)progressHandler;

            foreach (var fileToScan in bc.GetConsumingEnumerable())
            {
                await pauseToken.WaitWhilePausedAsync();

                if (token.IsCancellationRequested)
                    return c;

                var clam = new ClamClient("localhost", 3310)
                {
                    MaxStreamSize = 5242880
                };
                var scanResult = clam.ScanFileOnServer(fileToScan.Path);  //any file you would like!


                //Dispatcher.BeginInvoke(new Action(() => 
                //{
                //    this.CustomCurrentPath.Content = fileToScan.Path;

                //}));

                switch (scanResult.Result)
                {
                    case ClamScanResults.Clean:


                        break;
                    case ClamScanResults.VirusDetected:
                        Dispatcher.Invoke(new Action(() =>
                        {
                            DefectedItems.Distinct();
                            if (!fileToScan.Path.ToString().Contains(@"\TechesperData\Quarantine\"))
                            {
                                DefectedItems.Add(new FilePath() { File_Path = fileToScan.Path.ToString(), Virus_Type = scanResult.RawResult.Split(':')[2].Split(' ')[1] });
                                count++;
                                txtissues.Text = DefectedItems.Count.ToString();
                                Array1.Add(fileToScan.Path.ToString());
                            }

                           

                        }));
                        break;
                    case ClamScanResults.Error:

                        break;
                }

                if (progress != null)
                    progress.Report(fileToScan.Path);

            }


            return c;
        }

        

        public async Task<int> FullScan(List<string> list, CancellationToken token, Progress<string> progressHandler, PauseToken pauseToken)
        {

            IProgress<string> progress = (IProgress<string>)progressHandler;
            int count = 0;

            
            foreach (var path in list)
            {
                await pauseToken.WaitWhilePausedAsync();

                if (token.IsCancellationRequested)
                    return c;

                var clam = new ClamClient("localhost", 3310);
                var scanResult = clam.ScanFileOnServer(path);  //any file you would like!
                using (StreamWriter wr = new StreamWriter(@"C:\Testanti",true))
                {
                    wr.WriteLine(path);
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.fullscancurrentpath.Content = path;
                    this.totFiles.Text = c.ToString();
                }));


                using (StreamWriter wr = new StreamWriter(@"C:\Testantiresult", true))
                {
                    wr.WriteLine(scanResult.Result);
                }

                switch (scanResult.Result)
                {
                    case ClamScanResults.Clean:
                        break;
                    case ClamScanResults.VirusDetected:
                        Dispatcher.Invoke(new Action(() =>
                        {
                            DefectedItems.Distinct();
                            DefectedItems.Add(new FilePath() { File_Path = path, Virus_Type = scanResult.RawResult.Split(':')[2].Split(' ')[1] });
                            count++;
                            txtissues.Text = DefectedItems.Count.ToString();
                           
                            Antivirus.Properties.Settings.Default.Totalssues = DefectedItems.Count;
                            Array1.Add(path.ToString());
                        }));
                        break;
                    case ClamScanResults.Error:

                        break;
                }

                if (progress != null)
                    progress.Report(path);
                count++;

            }


            return count;
        }

        private void Quick_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                if (QuickDefectedList.SelectedItems.Count > 0)
                {
                    for (int i = QuickDefectedList.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        var file = ((FilePath)QuickDefectedList.SelectedItems[i]).File_Path;


                        DefectedItems.Remove(DefectedItems.Where(item => item.File_Path == file).Single());
                        System.IO.File.Delete(file);
                        txtissues.Text = DefectedItems.Count.ToString();

                    }

                    MyPopup hjh = new MyPopup();
                    hjh.txt.Text = "Virus successfully deleted..";
                    hjh.ShowDialog();
                }

                else
                {
                    MyPopup hjh = new MyPopup();
                    hjh.txt.Text = "Please select a file..";
                    hjh.ShowDialog();
                }
            }

                 
            catch (Exception k)
            {

            }
           // txtissues.Text = "000";
        }

        private void Qchkselall_Checked(object sender, RoutedEventArgs e)
        {

            QuickDefectedList.SelectAll();

        }

        private void Qchkselall_Unchecked(object sender, RoutedEventArgs e)
        {

            QuickDefectedList.UnselectAll();

        }

        private void QbtnSelall_Click(object sender, RoutedEventArgs e)
        {
          try
            {
                for (int i = QuickDefectedList.SelectedItems.Count - 1; i >= 0; i--)
                {
                    FilePath file = (FilePath)QuickDefectedList.SelectedItems[i];


                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine");

                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine\" + Path.GetFileName(file.File_Path))) ;

                    {

                        System.IO.File.Delete((Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine\" + Path.ChangeExtension(Path.GetFileName(file.File_Path), ".qua")));
                        System.IO.File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine", Path.GetFileName(file.File_Path) + ".bin"));

                       // MessageBox.Show("File Exist:-" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine", Path.GetFileName(file.File_Path) + ".bin"));
                    }

                    File.Move(file.File_Path, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine\" + Path.ChangeExtension(Path.GetFileName(file.File_Path), ".qua"));

                    string serializationFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine", Path.GetFileName(file.File_Path) + ".bin");

                    //serialize
                    using (Stream stream = File.Open(serializationFile, FileMode.CreateNew))
                    {
                        var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                        bformatter.Serialize(stream, file);
                    }

                    DefectedItems.Remove(DefectedItems.Where(item => item.File_Path == file.File_Path).Single());
                    txtissues.Text = "000";
                    MyPopup kj = new MyPopup();
                    kj.txt.Text = "Virus Quarantined";
                    kj.ShowDialog();
                }
            }
            catch (Exception lk)
            {

            }
          //  txtissues.Text = "000";
            
        }
        private void Fqurantile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = FullScanDefectedList.SelectedItems.Count - 1; i >= 0; i--)
                {
                    FilePath file = (FilePath)FullScanDefectedList.SelectedItems[i];


                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine");

                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine\" + Path.GetFileName(file.File_Path))) ;

                    {

                        System.IO.File.Delete((Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine\" + Path.ChangeExtension(Path.GetFileName(file.File_Path), ".qua")));
                        System.IO.File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine", Path.GetFileName(file.File_Path) + ".bin"));

                       // MessageBox.Show("File Exist:-" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine", Path.GetFileName(file.File_Path) + ".bin"));
                    }

                    File.Move(file.File_Path, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine\" + Path.ChangeExtension(Path.GetFileName(file.File_Path), ".qua"));

                    string serializationFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine", Path.GetFileName(file.File_Path) + ".bin");

                    //serialize
                    using (Stream stream = File.Open(serializationFile, FileMode.CreateNew))
                    {
                        var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                        bformatter.Serialize(stream, file);
                    }

                    DefectedItems.Remove(DefectedItems.Where(item => item.File_Path == file.File_Path).Single());
                    txtissues.Text = "000";
                    MyPopup kj = new MyPopup();
                    kj.txt.Text = "Virus Quarantined";
                    kj.ShowDialog();
                }
            }
            catch (Exception lk)
            {

            }
            //txtissues.Text = "000";
        }

        private void Cqurantile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = CustomDefectedItemsList.SelectedItems.Count - 1; i >= 0; i--)
                {
                    FilePath file = (FilePath)CustomDefectedItemsList.SelectedItems[i];


                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine");

                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine\" + Path.GetFileName(file.File_Path))) ;

                    {

                        System.IO.File.Delete((Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine\" + Path.ChangeExtension(Path.GetFileName(file.File_Path), ".qua")));
                        System.IO.File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine", Path.GetFileName(file.File_Path) + ".bin"));

                         MessageBox.Show("File Exist:-" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine", Path.GetFileName(file.File_Path) + ".bin"));
                    }

                    File.Move(file.File_Path, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine\" + Path.ChangeExtension(Path.GetFileName(file.File_Path), ".qua"));

                    string serializationFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine", Path.GetFileName(file.File_Path) + ".bin");

                    //serialize
                    using (Stream stream = File.Open(serializationFile, FileMode.CreateNew))
                    {
                        var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                        bformatter.Serialize(stream, file);
                    }

                    DefectedItems.Remove(DefectedItems.Where(item => item.File_Path == file.File_Path).Single());
                    txtissues.Text = "000";
                    MyPopup kj = new MyPopup();
                    kj.txt.Text = "Virus Quarantined";
                    kj.ShowDialog();
                }
            }
            catch (Exception lk)
            {

            }
            //try
            //{
            //    sw.Stop();
            //    if (CustomDefectedItemsList.SelectedItems.Count > 0)
            //    {
            //        for (int i = CustomDefectedItemsList.SelectedItems.Count - 1; i >= 0; i--)
            //        {
            //            var file = ((FilePath)CustomDefectedItemsList.SelectedItems[i]).File_Path;

            //            DefectedItems.Remove(DefectedItems.Where(item => item.File_Path == file).Single());

            //            System.IO.File.Delete(file);
            //            txtissues.Text = DefectedItems.Count.ToString();

            //        }

            //        MyPopup hjh = new MyPopup();
            //        hjh.txt.Text = "Virus successfully quarantined..";
            //        hjh.Show();
            //    }

            //    else {
            //        MyPopup hjh = new MyPopup();
            //        hjh.txt.Text = "Please select a file..";
            //        hjh.Show();
                
            //    }
            //}
            //catch (Exception k)
            //{

            //}
           // txtissues.Text = "000";
        }

        private void Fchkselall_Checked(object sender, RoutedEventArgs e)
        {

            FullScanDefectedList.SelectAll();

        }

        private void Fchkselall_Unchecked(object sender, RoutedEventArgs e)
        {

            FullScanDefectedList.UnselectAll();

        }
        private void Cchkselall_Checked(object sender, RoutedEventArgs e)
        {


            CustomDefectedItemsList.SelectAll();

        }

        private void Cchkselall_Unchecked(object sender, RoutedEventArgs e)
        {

            CustomDefectedItemsList.UnselectAll();

        }

        private void CustomDefectedItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CustomScanProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void viewReport_Click(object sender, RoutedEventArgs e)
        {
            Antivirus.Properties.Settings.Default.TimeElapsed = timeTot.Text;
            Antivirus.Properties.Settings.Default.Arraylst = Array1;
            Dashboard d = new Dashboard();
            d.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Antivirus.Properties.Settings.Default.TimeElapsed = timeTot.Text;
            Antivirus.Properties.Settings.Default.Arraylst = Array1;
            Dashboard d = new Dashboard();
            d.Show();
        }

        private void viewReportq_Click(object sender, RoutedEventArgs e)
        {
            Antivirus.Properties.Settings.Default.TimeElapsed = timeTot.Text;
            Antivirus.Properties.Settings.Default.Arraylst = Array1;
            Dashboard d = new Dashboard();
            d.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PotenTially k = new PotenTially();
            k.Show();
        }


        

    }
}

