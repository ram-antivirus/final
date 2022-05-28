using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractLibrary;
using FilePathNameSpace;
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using System.Windows.Controls.Primitives;
using Antivirus.Pages;
using System.ServiceModel;
using System.Threading;
using System.IO;
using Tulpep.NotificationWindow;

namespace Antivirus
{
    public class ClientEndPoint : IDesktopAppContract
    {
        public TaskbarIcon notifyIcon;        

        public void PrintVirusDetails(FilePath path)
        {
            //notifyIcon = (TaskbarIcon)Application.Current.FindResource("NotifyIcon");
            
            //notifyIcon.ShowCustomBalloon(new FancyPopup(path.File_Path,path.Virus_Type),PopupAnimation.Slide,10000);

            UsbDetect r = new UsbDetect();
            r.CustomScanProgressBar.Visibility = Visibility.Hidden;
            r.txt.Text = path.File_Path + ":->" + path.Virus_Type + "Virus Removed";
        }
        
        public void CallPop(string msg)
        {
            try
            {

                PopupNotifier popup = new PopupNotifier();
                popup.TitleText = "RAM Antivirus";
                popup.ContentText = msg;
                popup.Popup();// show  
            }
            catch (Exception e)
            {

            }
        }
        public void Result(string a,string b)
        {
            UsbDetect r = new UsbDetect();
            r.CustomScanProgressBar.Visibility = Visibility.Hidden;
            r.txt.Text = a + ":->" + b + "Virus Removed";
        }
        
        public void callAshu()
        {
            throw new NotImplementedException();
        }
    }
   
}
