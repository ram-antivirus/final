using NotificationWindow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using System.ServiceProcess;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SystemTray
{
    static class 
        Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static HashSet<string> res = new HashSet<string>();
        [STAThread]
        static void Main(string[] args)
        {
            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                NotifyIcon notifyIcon = new NotifyIcon();
                notifyIcon.ContextMenuStrip = GetContext();
                notifyIcon.Icon = new Icon(@AppDomain.CurrentDomain.BaseDirectory + "iconR.ico");
               
                string abc = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                // string a = abc.



                WshShell wshShell = new WshShell();
                IWshRuntimeLibrary.IWshShortcut shortcut;
                string startUpFolderPath =
                  Environment.GetFolderPath(Environment.SpecialFolder.Startup);

                // Create the shortcut
                shortcut =
                  (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(
                    startUpFolderPath + "\\" +
                    Application.ProductName + ".lnk");
                shortcut.TargetPath = Application.ExecutablePath;
                shortcut.WorkingDirectory = Application.StartupPath;
                shortcut.Description = "RamAntivirus";
                // shortcut.IconLocation = Application.StartupPath + @"\App.ico";
                shortcut.Save();

               
               

                //Create a File for RamWorkerService
                try
                {
                    
                    if (!Directory.Exists(abc))
                    {
                        Directory.CreateDirectory(abc);
                    }
                    

                }
                catch (Exception ej)
                {

                }





                //  MessageBox.Show(abc);
                //       MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory + "Logs\\");

                notifyIcon.Visible = true;
               
                    var watcher = new FileSystemWatcher(abc);

                    watcher.NotifyFilter = NotifyFilters.Attributes
                                         | NotifyFilters.CreationTime
                                         | NotifyFilters.DirectoryName
                                         | NotifyFilters.FileName
                                         | NotifyFilters.LastAccess
                                         | NotifyFilters.LastWrite
                                         | NotifyFilters.Security
                                         | NotifyFilters.Size;

                    watcher.Changed += OnChanged;


                    //MessageBox.Show("ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt");
                    watcher.Filter = "ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
                    watcher.IncludeSubdirectories = true;
                    watcher.EnableRaisingEvents = true;

                // MessageBox.Show("Press enter to exit.");
               

                Application.Run();
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
        }

       

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (e.ChangeType != WatcherChangeTypes.Changed)
                {
                    return;
                }
                string abc = AppDomain.CurrentDomain.BaseDirectory;
                string filepath = abc + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
                string message = System.IO.File.ReadAllText(filepath);
                if (!res.Contains(message))
                {
                    res.Add(message);
                    PopupForm myForm = new PopupForm();
                    //  myForm.SetBounds(10, 10, 200, 200);
                    myForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    myForm.MinimizeBox = false;
                    myForm.MaximizeBox = false;
                    myForm.TopMost = true;
                    // Do not allow form to be displayed in taskbar.
                    myForm.ShowInTaskbar = false;
                    myForm.ShowDialog();
                }
                
              
            }
            catch (Exception r)
            {
              //  MessageBox.Show(r.InnerException.ToString());
            }
           

        }
       
        private static ContextMenuStrip GetContext()
        {
            ContextMenuStrip CMS = new ContextMenuStrip();
            CMS.Items.Add("Exit", null, new EventHandler(Exit_Click));
            CMS.Items.Add("Show Window", null, new EventHandler(Show_Window));
            return CMS;
        }

        private static void Show_Window(object sender, EventArgs e)
        {
            string abc = AppDomain.CurrentDomain.BaseDirectory;
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Antivirus.exe");
        }

        private static void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
