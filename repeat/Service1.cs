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
using System;
using System.Collections.Generic;




namespace repeat
{
    public partial class Service1 : ServiceBase
    {
        private List<NtfsUsnJournal> _usnJournal;
        private NtfsUsnJournal testJ = null;
        private static Mutex mut = new Mutex();
        NetNamedPipeBinding binding;
        EndpointAddress ep;

        private List<Win32Api.USN_JOURNAL_DATA> _usnCurrentJournalState;
        private static object locker = new Object();
        private System.Timers.Timer timer;
        private System.ComponentModel.IContainer components;

        public Service1()
        {
            InitializeComponent();
            int wait = 5 * 1000;
            timer = new System.Timers.Timer(wait);
            timer.Elapsed += timer_Elapsed;

            // We don't want the timer to start ticking again till we tell it to.
            timer.AutoReset = false;

            if (!System.Diagnostics.EventLog.SourceExists("RealTimeService"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "RealTimeService", "RealTimeNewLog");
            }
            //OnStart(null);
        }

        protected override void OnStart(string[] args)
        {

            string address = "net.pipe://localhost/Techesper/NotifyIcon";

            binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            ep = new EndpointAddress(address);

            using (StreamWriter wr = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FileStart.txt", false))
            {
                wr.WriteLine("Service started");

            }
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
            catch (Exception a)
            {
                // MessageBox.Show("Exception in else" + a.Message);
            }
            create();
            savestate();
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < _usnJournal.Count; i++)
            {
                int c = i;
                Task.Run(() => viewUsnChanges(_usnJournal[c], _usnCurrentJournalState[c]));
            }
            timer.Start();

        }

        public void create()
        {

            try
            {
                _usnJournal = new List<NtfsUsnJournal>();

                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in allDrives)
                {

                    testJ = new NtfsUsnJournal(new DriveInfo(drive.Name));
                    _usnJournal.Add(testJ);
                }

            }
            catch (Exception excptn)
            {
            }
        }

        public void savestate()
        {

            _usnCurrentJournalState = new List<Win32Api.USN_JOURNAL_DATA>();
            try
            {
                for (int i = 0; i < _usnJournal.Count; i++)
                {
                    Win32Api.USN_JOURNAL_DATA journalState = new Win32Api.USN_JOURNAL_DATA();
                    NtfsUsnJournal.UsnJournalReturnCode rtn = _usnJournal[i].GetUsnJournalState(ref journalState);

                    if (rtn == NtfsUsnJournal.UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                    {
                        Win32Api.USN_JOURNAL_DATA state = journalState;
                        _usnCurrentJournalState.Add(state);
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception e)
            {


            }
        }

        private void viewUsnChanges(NtfsUsnJournal journal, Win32Api.USN_JOURNAL_DATA state)
        {
            try
            {
                if (state.UsnJournalID != 0)
                {

                    Thread usnJournalThread = new Thread(() => ViewChangesThreadStart(journal, state));
                    usnJournalThread.Start();

                }
                else
                {
                    //MessageBox.Show(" Must Save State before calling View Changes");
                }
            }
            catch (Exception ae)
            {

            }
        }

        private void ViewChangesThreadStart(NtfsUsnJournal journal, Win32Api.USN_JOURNAL_DATA state)
        {
            uint reasonMask = Win32Api.FSCTL_CREATE_USN_JOURNAL;
            int j;
            Win32Api.USN_JOURNAL_DATA newUsnState;
            List<Win32Api.UsnEntry> usnEntries;
            NtfsUsnJournal.UsnJournalReturnCode rtnCode = journal.GetUsnJournalEntries(state, reasonMask, out usnEntries, out newUsnState);

            try
            {
                if (rtnCode == NtfsUsnJournal.UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                {
                    if (usnEntries.Count > 0)
                    {
                        int i;
                        List<string> list = new List<string>();

                        for (i = 0; i < usnEntries.Count; i++)
                        {
                            string path;

                            if (usnEntries[i].Name[0] != '$')
                            {
                                NtfsUsnJournal.UsnJournalReturnCode usnRtnCode = journal.GetPathFromFileReference(usnEntries[i].ParentFileReferenceNumber, out path);

                                if (usnRtnCode == NtfsUsnJournal.UsnJournalReturnCode.USN_JOURNAL_SUCCESS && 0 != string.Compare(path, "Unavailable", true) && !list.Contains(journal.VolumeName.TrimEnd('\\') + path + "\\" + usnEntries[i].Name))
                                {
                                    list.Add(journal.VolumeName.TrimEnd('\\') + path + "\\" + usnEntries[i].Name);
                                }
                            }
                        }

                        for (j = 0; j < _usnJournal.Count; j++)
                        {
                            if (_usnJournal[j].VolumeName == journal.VolumeName)
                                break;
                        }
                        _usnCurrentJournalState[j] = newUsnState;

                        foreach (string name in list)
                        {
                            var clam = new ClamClient("localhost", 3310);
                            var scanResult = clam.ScanFileOnServer(name);  //any file you would like!  

                            switch (scanResult.Result)
                            {
                                case ClamScanResults.Clean:


                                    break;
                                case ClamScanResults.VirusDetected:
                                    try
                                    {

                                        using (StreamWriter wr = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\error.txt", false))
                                        {
                                            wr.WriteLine(name);

                                        }
                                        File.Delete(name);
                                    }
                                    catch (Exception h)
                                    {
                                        using (StreamWriter wr = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\exception.txt", false))
                                        {
                                            wr.WriteLine("exception  " + h);

                                        }




                                    }
                                    try
                                    {
                                        IDesktopAppContract channel = ChannelFactory<IDesktopAppContract>.CreateChannel(binding, ep);
                                        ChannelFactory<IDesktopAppContract> factory = new ChannelFactory<IDesktopAppContract>(binding, ep);
                                        using (channel as IDisposable)
                                        {
                                            try
                                            {

                                                // channel.PrintVirusDetails(new FilePath { File_Path = name, Virus_Type = scanResult.RawResult.Split(':')[2].Split(' ')[1] });
                                                channel.CallPop("Virus Removed : " + name);




                                            }
                                            catch (Exception ex)
                                            {
                                                using (StreamWriter wr = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\exception.txt", false))
                                                {
                                                    wr.WriteLine("exception  " + ex);

                                                }
                                            }
                                        }
                                    }
                                    catch (Exception eof)
                                    {
                                        using (StreamWriter wr = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\exception.txt", false))
                                        {
                                            wr.WriteLine("exception  " + eof);

                                        }
                                    }
                                    break;
                                case ClamScanResults.Error:

                                    break;
                            }
                        }
                        list = null;
                    }
                }
            }
            catch (Exception e)
            {
            }

        }

        private string FormatUsnJournalState(Win32Api.USN_JOURNAL_DATA _usnCurrentJournalState)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Journal ID: {0}", _usnCurrentJournalState.UsnJournalID.ToString("X")));
            sb.AppendLine(string.Format(" First USN: {0}", _usnCurrentJournalState.FirstUsn.ToString("X")));
            sb.AppendLine(string.Format("  Next USN: {0}", _usnCurrentJournalState.NextUsn.ToString("X")));
            sb.AppendLine();
            sb.AppendLine(string.Format("Lowest Valid USN: {0}", _usnCurrentJournalState.LowestValidUsn.ToString("X")));
            sb.AppendLine(string.Format("         Max USN: {0}", _usnCurrentJournalState.MaxUsn.ToString("X")));
            sb.AppendLine(string.Format("        Max Size: {0}", _usnCurrentJournalState.MaximumSize.ToString("X")));
            sb.AppendLine(string.Format("Allocation Delta: {0}", _usnCurrentJournalState.AllocationDelta.ToString("X")));
            return sb.ToString();
        }
    }
}