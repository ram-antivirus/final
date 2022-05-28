using System;
using System.IO;
using System.Windows.Forms;

// Using IWshRuntimeLibrary requires a COM reference to 'Windows Script Host Object Model'
using IWshRuntimeLibrary;

// Using Shell32 requires a COM reference to 'Microsoft Shell Controls and Automation'
using Shell32;
using System.Diagnostics;
using Microsoft.Win32;

namespace StartupFolderShortcut
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
            
            Process.Start(@"F:\projects\WindowsServiceUpdate\WindowsServiceUpdate\bin\Debug\WindowsServiceUpdate.exe");
      CreateStartupFolderShortcut();
     this.Hide();
    }

    private void buttonCreate_Click(object sender, EventArgs e)
    {
      
    }

    private void buttonDelete_Click(object sender, EventArgs e)
    {
      DeleteStartupFolderShortcuts(Path.GetFileName(Application.ExecutablePath)) ;
    }

    public void CreateStartupFolderShortcut()
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

    public string GetShortcutTargetFile(string shortcutFilename)
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

    public void DeleteStartupFolderShortcuts(string targetExeName)
    {
      string startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

      DirectoryInfo di = new DirectoryInfo(startUpFolderPath);
      FileInfo[] files = di.GetFiles("*.lnk");

      foreach (FileInfo fi in files)
      {
        string shortcutTargetFile = GetShortcutTargetFile(fi.FullName);
        Console.WriteLine("{0} -> {1}", fi.Name, shortcutTargetFile);

        if (shortcutTargetFile.EndsWith(targetExeName, StringComparison.InvariantCultureIgnoreCase))
        {
          System.IO.File.Delete(fi.FullName);
        }
      }
    }

  }
}
