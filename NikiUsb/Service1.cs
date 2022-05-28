using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;


using UsnJournal;
using PInvoke;
using System.IO;
using System.Windows;

using System.Threading;
using System.Timers;
using System.ServiceModel;
using ContractLibrary;
using FilePathNameSpace;
using nClam;
using System.Management;



namespace repeat
{
    public partial class Service7 : ServiceBase
    {

        private System.Timers.Timer timer1 = null;
       

        private List<NtfsUsnJournal> _usnJournal;
        private NtfsUsnJournal testJ = null;
        private static Mutex mut = new Mutex();
        NetNamedPipeBinding binding;
        EndpointAddress ep;
        
        private List<Win32Api.USN_JOURNAL_DATA> _usnCurrentJournalState;
        private static object locker = new Object();
        private System.Timers.Timer timer;
        private System.ComponentModel.IContainer components;        
      
        public Service7()
        {
            InitializeComponent();
            //int wait = 10 * 1000;
            //timer = new System.Timers.Timer(wait);
            //timer.Elapsed += timer_Elapsed;

            // We don't want the timer to start ticking again till we tell it to.
            //   timer.AutoReset = false;

            if (!System.Diagnostics.EventLog.SourceExists("USBService"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "USBService", "USBService");
            }
            //OnStart(null);
        }

        protected override void OnStart(string[] args)
        {

            string address = "net.pipe://localhost/Techesper/NotifyIconUsb";

            binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            ep = new EndpointAddress(address);
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                var service = services.FirstOrDefault(s => s.ServiceName == "ClamD");
                if (service != null)
                    Console.WriteLine(service);
                else
                {
                    //MessageBox.Show("In else Service Didnt Installed yet");
                    //Process p = new Process();
                    //p.StartInfo.UseShellExecute = false;
                    //p.StartInfo.RedirectStandardOutput = true;
                    //p.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\clamd.exe";
                    //p.StartInfo.Arguments = "--install";
                    //p.Start();
                    //p.WaitForExit();
                    //services = ServiceController.GetServices();
                    //service = services.FirstOrDefault(s => s.ServiceName == "ClamD");
                    //service.DisplayName = "Ram Antivirus Service";
                    //service.ServiceName = "Ram Antivirus Service";

                }



                switch (service.Status)
                {
                    case ServiceControllerStatus.Stopped:
                        service.Start();
                        break;
                }
            }
            catch (Exception w)
            {
            }

            timer1 = new System.Timers.Timer();
            this.timer1.Interval = 10 * 1000; //every 30 secs
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;
            //Library.WriteErrorLog("Test window service started");
            
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            //Write code here to do some job depends on your requirement
            AddInsertUSBHandler();
            AddRemoveUSBHandler();
            //while (true)
            //{

            //}

            //Library.WriteErrorLog("Timer ticked and some job has been done successfully" + DateTime.Now.ToLongTimeString());
        }
        protected override void OnStop()
        {
            timer1.Enabled = false;
        }

        //void timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    for (int i = 0; i < _usnJournal.Count; i++)
        //    {
        //        int c = i;
        //        Task.Run(() => viewUsnChanges(_usnJournal[c], _usnCurrentJournalState[c]));
        //    }
        //    timer.Start();
        //}

        //public void create()
        //{
            
        //    try
        //    {
        //        _usnJournal = new List<NtfsUsnJournal>();

        //        DriveInfo[] allDrives = DriveInfo.GetDrives();

        //        foreach (DriveInfo drive in allDrives)
        //        {

        //            testJ = new NtfsUsnJournal(new DriveInfo(drive.Name));
        //            _usnJournal.Add(testJ);                
        //        }
                          
        //    }
        //    catch (Exception excptn)
        //    {               
        //    }
        //}

        //public void savestate()
        //{
            
        //    _usnCurrentJournalState = new List<Win32Api.USN_JOURNAL_DATA>();
        //    try
        //    {
        //        for (int i = 0; i < _usnJournal.Count; i++)
        //        {
        //            Win32Api.USN_JOURNAL_DATA journalState = new Win32Api.USN_JOURNAL_DATA();
        //            NtfsUsnJournal.UsnJournalReturnCode rtn = _usnJournal[i].GetUsnJournalState(ref journalState);

        //            if (rtn == NtfsUsnJournal.UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
        //            {
        //                Win32Api.USN_JOURNAL_DATA state = journalState;
        //                _usnCurrentJournalState.Add(state);                    
        //            }
        //            else
        //            {
                      
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
                

        //    }
        //}

        //private void viewUsnChanges(NtfsUsnJournal journal, Win32Api.USN_JOURNAL_DATA state)
        //{
        //    try
        //    {
        //        if (state.UsnJournalID != 0)
        //        {
                    
        //            Thread usnJournalThread = new Thread(() => ViewChangesThreadStart(journal, state));
        //            usnJournalThread.Start();
                                    
        //        }
        //        else
        //        {
        //            //MessageBox.Show(" Must Save State before calling View Changes");
        //        }
        //    }
        //    catch (Exception ae)
        //    {               
                        
        //    }
        //}

        //private void ViewChangesThreadStart(NtfsUsnJournal journal, Win32Api.USN_JOURNAL_DATA state)
        //{
        //    uint reasonMask = Win32Api.USN_REASON_FILE_CREATE;
        //    int j;
        //    Win32Api.USN_JOURNAL_DATA newUsnState;
        //    List<Win32Api.UsnEntry> usnEntries;
        //    NtfsUsnJournal.UsnJournalReturnCode rtnCode = journal.GetUsnJournalEntries(state, reasonMask, out usnEntries, out newUsnState);
            
        //    try
        //    {
        //        if (rtnCode == NtfsUsnJournal.UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
        //        {
        //            if (usnEntries.Count > 0)
        //            {
        //                int i;
        //                List<string> list = new List<string>();

        //                for (i = 0; i < usnEntries.Count; i++)
        //                {
        //                    string path;

        //                    if (usnEntries[i].Name[0] != '$')
        //                    {
        //                        NtfsUsnJournal.UsnJournalReturnCode usnRtnCode = journal.GetPathFromFileReference(usnEntries[i].ParentFileReferenceNumber, out path);

        //                        if (usnRtnCode == NtfsUsnJournal.UsnJournalReturnCode.USN_JOURNAL_SUCCESS && 0 != string.Compare(path, "Unavailable", true) && !list.Contains(journal.VolumeName.TrimEnd('\\') + path + "\\" + usnEntries[i].Name))
        //                        {
        //                            list.Add(journal.VolumeName.TrimEnd('\\') + path + "\\" + usnEntries[i].Name);
        //                        }
        //                    }
        //                }

        //                for (j = 0; j < _usnJournal.Count; j++)
        //                {
        //                    if (_usnJournal[j].VolumeName == journal.VolumeName)
        //                        break;
        //                }
        //                _usnCurrentJournalState[j] = newUsnState;

        //                foreach (string name in list)
        //                {
        //                    var clam = new ClamClient("localhost", 3310);
        //                    var scanResult = clam.ScanFileOnServer(name);  //any file you would like!  
                        
        //                    switch (scanResult.Result)
        //                    {
        //                        case ClamScanResults.Clean:


        //                            break;
        //                        case ClamScanResults.VirusDetected:

        //                          using(System.IO.StreamWriter file = new System.IO.StreamWriter("E:\\error.txt", true)) 
        //                          {
        //                              file.WriteLine(name);
        //                          }
        //                            try
        //                            {
        //                                IDesktopAppContract channel = ChannelFactory<IDesktopAppContract>.CreateChannel(binding, ep);
        //                                ChannelFactory<IDesktopAppContract> factory = new ChannelFactory<IDesktopAppContract>(binding, ep);
        //                                using (channel as IDisposable)
        //                                {
        //                                    try
        //                                    {
        //                                        channel.PrintVirusDetails(new FilePath { File_Path = name, Virus_Type = scanResult.RawResult.Split(':')[2].Split(' ')[1] });
        //                                    }
        //                                    catch (Exception ex)
        //                                    {

        //                                    }
        //                                }                                        
        //                            }
        //                            catch (Exception eof)
        //                            {                                        
        //                            }
        //                            break;
        //                        case ClamScanResults.Error:
        //                            break;
        //                    }                            
        //                }
        //                list = null;
        //            }                    
        //        }               
        //    }
        //    catch (Exception e)
        //    {                                                            
        //    }

        //}
    
        //private string FormatUsnJournalState(Win32Api.USN_JOURNAL_DATA _usnCurrentJournalState)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine(string.Format("Journal ID: {0}", _usnCurrentJournalState.UsnJournalID.ToString("X")));
        //    sb.AppendLine(string.Format(" First USN: {0}", _usnCurrentJournalState.FirstUsn.ToString("X")));
        //    sb.AppendLine(string.Format("  Next USN: {0}", _usnCurrentJournalState.NextUsn.ToString("X")));
        //    sb.AppendLine();
        //    sb.AppendLine(string.Format("Lowest Valid USN: {0}", _usnCurrentJournalState.LowestValidUsn.ToString("X")));
        //    sb.AppendLine(string.Format("         Max USN: {0}", _usnCurrentJournalState.MaxUsn.ToString("X")));
        //    sb.AppendLine(string.Format("        Max Size: {0}", _usnCurrentJournalState.MaximumSize.ToString("X")));
        //    sb.AppendLine(string.Format("Allocation Delta: {0}", _usnCurrentJournalState.AllocationDelta.ToString("X")));
        //    return sb.ToString();
        //} 
        ManagementEventWatcher w = null;
        void AddRemoveUSBHandler()
        {

            WqlEventQuery q;
            ManagementScope scope = new ManagementScope("root\\CIMV2");
            scope.Options.EnablePrivileges = true;

            try
            {

                q = new WqlEventQuery();
                q.EventClassName = "__InstanceDeletionEvent";
                q.WithinInterval = new TimeSpan(0, 0, 3);
                q.Condition = "TargetInstance ISA 'Win32_USBControllerdevice'";
                w = new ManagementEventWatcher(scope, q);
                w.EventArrived += USBRemoved;

                w.Start();


            }
            catch (Exception e)
            {


                Console.WriteLine(e.Message);
                if (w != null)
                {
                    w.Stop();

                }
            }

        }
        void AddInsertUSBHandler()
        {

            WqlEventQuery q;
            ManagementScope scope = new ManagementScope("root\\CIMV2");
            scope.Options.EnablePrivileges = true;

            try
            {

                q = new WqlEventQuery();
                q.EventClassName = "__InstanceCreationEvent";
                q.WithinInterval = new TimeSpan(0, 0, 3);
                q.Condition = "TargetInstance ISA 'Win32_USBControllerdevice'";
                w = new ManagementEventWatcher(scope, q);
                w.EventArrived += USBInserted;

                w.Start();


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                if (w != null)
                {
                    w.Stop();

                }
            }

        }

        public void USBInserted(object sender, EventArgs e)
        {
            int i=1;
            using (StreamWriter wr = new StreamWriter(@"C:\UsbDetected1.txt", false))
            {
                wr.WriteLine("Usb Detected" + DateTime.Now.ToLongTimeString());
                IDesktopAppContract channel = ChannelFactory<IDesktopAppContract>.CreateChannel(binding, ep);
                ChannelFactory<IDesktopAppContract> factory = new ChannelFactory<IDesktopAppContract>(binding, ep);
                if (i == 1)
                {
                using (channel as IDisposable)
                {
                    try
                    {
                        channel.CallPop("USB detected and scanning in progress....");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                    i = 2;
                }
            }
            //try
            //{


            //    IDesktopAppContract channel = ChannelFactory<IDesktopAppContract>.CreateChannel(binding, ep);
            //    ChannelFactory<IDesktopAppContract> factory = new ChannelFactory<IDesktopAppContract>(binding, ep);
            //    using (channel as IDisposable)
            //    {
            //        try
            //        {
            //            channel.PrintVirusDetails(new FilePath { File_Path = "Hello", Virus_Type = "NIKI" });
            //        }
            //        catch (Exception ex)
            //        {

            //        }
            //    }
            //}

            //catch (Exception eof)
            //{
            //}

            pathDrive();

        }

        void USBRemoved(object sender, EventArgs e)
        {

            using (StreamWriter wr = new StreamWriter(@"C:\UsbDetected.txt", false))
            {
                wr.WriteLine("Usb Removed" + DateTime.Now.ToLongTimeString());
            };


            //try
            //{


            //    IDesktopAppContract channel = ChannelFactory<IDesktopAppContract>.CreateChannel(binding, ep);
            //    ChannelFactory<IDesktopAppContract> factory = new ChannelFactory<IDesktopAppContract>(binding, ep);
            //    using (channel as IDisposable)
            //    {
            //        try
            //        {
            //            channel.CallPop();
            //        }
            //        catch (Exception ex)
            //        {

            //        }
            //    }
            //}

            //catch (Exception eof)
            //{
            //}

        }

        void pathDrive()
        {
            System.IO.DriveInfo[] mydrives = System.IO.DriveInfo.GetDrives();

            foreach (System.IO.DriveInfo mydrive in mydrives)
            {
                if (mydrive.DriveType == System.IO.DriveType.Removable)
                {


                    using (StreamWriter wr = new StreamWriter(@"C:\UsbDetected.txt", false))
                    {
                        wr.WriteLine("Drive: {0}", mydrive.Name + DateTime.Now.ToLongTimeString());
                        wr.WriteLine("Type: {0}", mydrive.DriveType);
                    }

                   // clamscan(mydrive.Name);
                    DirSearch(mydrive.Name);
                }
            }
        }


         void DirSearch(string sDir)
        {
            

            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        using (StreamWriter wr = new StreamWriter(@"C:\CheckFiles.txt", true))
                        {
                            wr.WriteLine("The file is clean! " + f);

                        }
                        clamscan(f);
                    }
                    DirSearch(d);
                }
            }
            catch (System.Exception excpt)
            {
                // Console.WriteLine(excpt.Message);
            }
            finally 
            {
                
 
            }
        }

        // clam scan 

        public void clamscan(string path)
        {

            Task.Run(() =>
            {
                var clam = new ClamClient("localhost", 3310);
                var t = path.Replace(@"\", "");
              
                var scanResult = clam.ScanFileOnServer(path);

                    switch (scanResult.Result)
                    {
                        case ClamScanResults.Clean:

                            using (StreamWriter wr = new StreamWriter(@"E:\UsbDetectedResult.txt", true))
                            {
                                wr.WriteLine("The file is clean! " + path);

                            }

                            //try
                            //{


                            //    IDesktopAppContract channel = ChannelFactory<IDesktopAppContract>.CreateChannel(binding, ep);
                            //    ChannelFactory<IDesktopAppContract> factory = new ChannelFactory<IDesktopAppContract>(binding, ep);
                            //    using (channel as IDisposable)
                            //    {
                            //        try
                            //        {
                            //            channel.CallPop();
                            //        }
                            //        catch (Exception ex)
                            //        {

                            //        }
                            //    }
                            //}

                            //catch (Exception eof)
                            //{
                            //}

                            break;
                        case ClamScanResults.VirusDetected:

                            
                            
                            try
                            {
                                using (StreamWriter wr = new StreamWriter(@"C:\UsbDetectedResult.txt", true))
                                {
                                    wr.WriteLine("Virus name: {0} ", scanResult.InfectedFiles.First().VirusName);
                                    wr.WriteLine("Path " + path);

                                }
                                File.Delete(path);

                                IDesktopAppContract channel = ChannelFactory<IDesktopAppContract>.CreateChannel(binding, ep);
                                ChannelFactory<IDesktopAppContract> factory = new ChannelFactory<IDesktopAppContract>(binding, ep);
                                using (channel as IDisposable)
                                {
                                    try
                                    {
                                        File.Delete(path);
                                        channel.CallPop("Virus Removed : " + path);
                                    }
                                    catch (Exception ex)
                                    {
                                        using (StreamWriter wr = new StreamWriter(@"C:\Exception.txt", true))
                                        {
                                            wr.WriteLine("The file is clean! " + ex);

                                        }
                                    }
                                }

                            }



                            catch (Exception eof)
                            {
                                using (StreamWriter wr = new StreamWriter(@"C:\Exception.txt", true))
                                {
                                    wr.WriteLine("The file is clean! " + eof);

                                }
                            }
                            break;

                        case ClamScanResults.Error:
                            using (StreamWriter wr = new StreamWriter(@"C:\UsbDetectedResult.txt", true))
                            {
                                wr.WriteLine("Woah an error occured! Error: {0}", scanResult.RawResult);
                                wr.WriteLine("Path " + path);

                            }
                            //try
                            //{


                            //    IDesktopAppContract channel = ChannelFactory<IDesktopAppContract>.CreateChannel(binding, ep);
                            //    ChannelFactory<IDesktopAppContract> factory = new ChannelFactory<IDesktopAppContract>(binding, ep);
                            //    using (channel as IDisposable)
                            //    {
                            //        try
                            //        {
                            //            channel.CallPop();
                            //        }
                            //        catch (Exception ex)
                            //        {

                            //        }
                            //    }
                            //}

                            //catch (Exception eof)
                            //{
                            //}


                            break;
                    }
                
            }).Wait();

        }
    }
}


