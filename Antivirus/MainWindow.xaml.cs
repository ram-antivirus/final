using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using System;
using System.Collections.Generic;
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
using Microsoft.WindowsAPICodePack.ApplicationServices;
using MS.WindowsAPICodePack.Internal;
namespace Antivirus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
       //     MessageBox.Show("Applicaition started");
            this.ResizeMode = ResizeMode.CanMinimize;
            try
            {

                if (App.ashu.Contains(@"\"))
                {
                    Uri relativeUri = new Uri("Pages/scan.xaml", UriKind.Relative);
                    this.ContentSource = relativeUri;
                }

                else
                {

                    Uri relativeUri = new Uri("Pages/home.xaml", UriKind.Relative);
                    this.ContentSource = relativeUri;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            //Application.Current.MainWindow = this;
            //string url = "/Pages/scan.xaml";
            //NavigationService nav = NavigationService.GetNavigationService(this);
            //nav.Navigate(new System.Uri(url, UriKind.RelativeOrAbsolute));                   
        }
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
        }

        private void RegisterApplicationRecoveryAndRestart()
        {
            if (!CoreHelpers.RunningOnVista)
            {
                return;
            }

            // register for Application Restart
            RestartSettings restartSettings =
                new RestartSettings(string.Empty, RestartRestrictions.None);
            ApplicationRestartRecoveryManager.RegisterForApplicationRestart(restartSettings);

            // register for Application Recovery
            RecoverySettings recoverySettings =
                new RecoverySettings(new RecoveryData(PerformRecovery, null), 10);
            ApplicationRestartRecoveryManager.RegisterForApplicationRecovery(recoverySettings);
        }
        // if app hangs restart it



        /// <summary>
        /// Performs recovery by saving the state 
        /// </summary>
        /// <param name="parameter">Unused.</param>
        /// <returns>Unused.</returns>
        private int PerformRecovery(object parameter)
        {
            try
            {
                ApplicationRestartRecoveryManager.ApplicationRecoveryInProgress();
                // Save your work here for recovery


                ApplicationRestartRecoveryManager.ApplicationRecoveryFinished(true);
            }
            catch
            {
                ApplicationRestartRecoveryManager.ApplicationRecoveryFinished(false);
            }

            return 0;

        }
        private void UnregisterApplicationRecoveryAndRestart()
        {
            if (!CoreHelpers.RunningOnVista)
            {
                return;
            }

            ApplicationRestartRecoveryManager.UnregisterApplicationRestart();
            ApplicationRestartRecoveryManager.UnregisterApplicationRecovery();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
    
          UnregisterApplicationRecoveryAndRestart();
          App.Current.Shutdown();
        }
    }
}
