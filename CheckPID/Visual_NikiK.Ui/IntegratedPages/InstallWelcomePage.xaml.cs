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
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Xeam.VisualInstaller;
using System.Diagnostics;
using System.IO;

namespace Visual_NikiK.Ui
{
    /// <summary>
    /// Interaction logic for Welcome page
    /// </summary>
    [Export(typeof(IBootstrapperPage))]
    public partial class InstallWelcomePage : UserControl, IBootstrapperPage
    {
        InstallWelcomePageViewModel _model;

        public string PageName { get { return "InstallWelcome"; } }
        public FrameworkElement Page { get { return this; } }

        public InstallWelcomePage()
        {
            InitializeComponent();
        }

        public void Initialize(MainWindowViewModel mainViewModel)
        {
            // create view model for the page
            _model = new InstallWelcomePageViewModel(mainViewModel);
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
            Process myProcess;
            myProcess = Process.Start(Directory.GetCurrentDirectory() + "\\HttpPostReq.exe");
        }

    }
}
