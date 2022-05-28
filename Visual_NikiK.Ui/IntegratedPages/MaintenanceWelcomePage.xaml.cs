using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
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
using Xeam.VisualInstaller;

namespace Visual_NikiK.Ui
{
    /// <summary>
    /// Interaction logic for MaintenanceWelcomeView.xaml
    /// </summary>
    [Export(typeof(IBootstrapperPage))]
    public partial class MaintenanceWelcomePage : UserControl, IBootstrapperPage
    {
        MaintenanceWelcomePageViewModel _model;

        public MaintenanceWelcomePage()
        {

            InitializeComponent();
        }

        public string PageName
        {
            get { return "MaintenanceWelcome"; }
        }

        public FrameworkElement Page
        {
            get { return this; }
        }

        public string LocalizedPageName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string LocalizedPageNameId => throw new NotImplementedException();

        public bool AddToProcessStepBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Initialize(MainWindowViewModel mainViewModel)
        {
            _model = new MaintenanceWelcomePageViewModel(mainViewModel);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (IsProcessRunning("Antivirus") == true)
            {
                MessageBox.Show("Close all the running processes");
                Application.Current.Shutdown();
            }
        }

        private static bool IsProcessRunning(string sProcessName)
        {
            System.Diagnostics.Process[] proc = System.Diagnostics.Process.GetProcessesByName(sProcessName);
            if (proc.Length > 0)
            {
                Console.WriteLine(String.Format("{0}is  running!", sProcessName), sProcessName);
                return true;
            }
            else
            {
                Console.WriteLine(String.Format("{0} -->>is not running!", sProcessName), sProcessName);
                // start your process

                return false;
            }
        }

    }
}
