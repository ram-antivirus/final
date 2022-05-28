using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace repeat
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        private static System.ComponentModel.IContainer components;
        private static System.Diagnostics.EventLog eventLog1;
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("RealTimeProgram"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "RealTimeProgram", "RealTimeNewLog");
            }
            eventLog1.Source = "ProgramSource";
            eventLog1.Log = "ProgramNewLog";
//#if (!DEBUG)
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service4() 
            };
            ServiceBase.Run(ServicesToRun);
//#else
//            Service1 myServ = new Service1();

//            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
//#endif
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            string file = @"C:\erorr.txt";
            try
            {
               // Logger.Append(Severity.CRITICAL, "Unhandlable error : " + e.ExceptionObject);
                eventLog1.WriteEntry(e.ToString());
                using (StreamWriter sw = File.AppendText(file))
                {
                    sw.WriteLine("from view changes" + e.ExceptionObject);
                }


            }
            catch(Exception ex) 
            {
                eventLog1.WriteEntry(ex.StackTrace);
            }
            
            Environment.Exit(10);
        }
    }
}
