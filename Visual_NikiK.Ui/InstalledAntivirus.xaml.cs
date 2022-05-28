using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xeam.VisualInstaller;

namespace Visual_NikiK.Ui
{
    /// <summary>
    /// Interaction logic for InstalledAntivirus.xaml
    /// </summary>
    [Export(typeof(IBootstrapperPage))]
    public partial class InstalledAntivirus : UserControl, IBootstrapperPage
    {
        private InstalledAntivirusViewModel _model;
        

        public InstalledAntivirus()
        {
            InitializeComponent();
        }
     
        public string PageName
        {
            // Note: You can define any name you like for this page. 
            // Be sure the name you defined here is matches exactly the one defined in the sequence of your configuration.xml
            get { return "InstalledAntivirus"; }
        }

        public FrameworkElement Page
        {
            // Create the view model and bind it to xaml page (this).
            get { return this; }
        }

        public string LocalizedPageName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string LocalizedPageNameId => throw new NotImplementedException();

        public bool AddToProcessStepBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Initialize(MainWindowViewModel mainViewModel)
        {
            _model = new InstalledAntivirusViewModel(mainViewModel);
            this.DataContext = _model;
        }

        public bool OnSilentInstall()
        {
            return _model.OnSilentMode();
        }

        public bool OnPassiveInstall()
        {
            return _model.OnPassiveMode();
        }

        /// <summary>
        /// Called before a page is shown
        /// </summary>
        /// <returns>true - the page will be shown
        ///          false - the page will not be shown and the sequence will go to the next page</returns>
        public bool OnBeforeShowPage()
        {
            return true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ManagementObjectSearcher wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
            ManagementObjectCollection data = wmiData.Get();
              string virusCheckerName = null;
              List<string> lstAnti = new List<string>();
              lstItems.ItemsSource = lstAnti;
            foreach (ManagementObject virusChecker in data)
            {
                virusCheckerName = virusChecker["displayName"].ToString();
                if (virusCheckerName != "Windows Defender")
                {
                    lstAnti.Add(virusCheckerName);
                }
                
                
            }
         //   if (lstItems.Items.Count <= 0)
         //   {
                nxtUninstall.IsEnabled = true;
         //   }
            lstItems.ItemsSource = lstAnti;

        }

        private void nxtUninstall_Click(object sender, RoutedEventArgs e)
        {
            //Process myProcess;
            //myProcess = Process.Start(Directory.GetCurrentDirectory() + "\\HttpPostReq.exe");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo pro = new ProcessStartInfo("control.exe");
            Process proStart = new Process();
            proStart.StartInfo = pro;
            proStart.Start();
        }

        private void lstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
