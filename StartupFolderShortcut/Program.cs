using System;
using System.IO;
using System.Windows.Forms;

using System.Diagnostics;
using IWshRuntimeLibrary;
using Shell32;
using IWshRuntimeLibrary;

namespace StartupFolderShortcut
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "RAMServiceUpdate.exe");
            }
            catch (Exception q)
            {

            }
  //    Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "RAMProcessMonitorPopup.exe");
     
      CreateStartupFolderShortcut();

    //  Application.Run(new Form1());
    }
    public static void CreateStartupFolderShortcut()
    {
        WshShellClass wshShell = new WshShellClass();
        IWshRuntimeLibrary.IWshShortcut shortcut;
        string startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

        // Create the shortcut
        shortcut = (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(startUpFolderPath + "\\" + Application.ProductName + ".lnk");

        shortcut.TargetPath = Application.ExecutablePath;
        shortcut.WorkingDirectory = Application.StartupPath;
        shortcut.Description = "Launch My Application";
        //      shortcut.IconLocation = Application.StartupPath + @"\App.ico";
        shortcut.Save();
    }

    public static string GetShortcutTargetFile(string shortcutFilename)
    {
        string pathOnly = Path.GetDirectoryName(shortcutFilename);
        string filenameOnly = Path.GetFileName(shortcutFilename);

        Shell32.Shell shell = new Shell32.ShellClass();
        Shell32.Folder folder = shell.NameSpace(pathOnly);
        Shell32.FolderItem folderItem = folder.ParseName(filenameOnly);
        if (folderItem != null)
        {
            Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
            return link.Path;
        }

        return String.Empty; // Not found
    }
  }
}
