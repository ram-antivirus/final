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
using System.Collections.ObjectModel;

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

        public string LocalizedPageName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string LocalizedPageNameId => throw new NotImplementedException();

        public bool AddToProcessStepBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
        //    ObservableCollection<string> collection = new ObservableCollection<string>();
        //    MessageBox.Show("Scanning for Software Dependencies");
        //    ApplicationScan scan = new ApplicationScan();
        //    collection = scan.RunApps();
        //    foreach (var item in collection)
        //    {
        //        Console.WriteLine(item);
        //    }
        //    if (collection.Any(x => x.Contains("Microsoft Visual C++  Redistributable"))) 
        //    {
        //        btnNextInstall.IsEnabled = true;
        //        Binding binding = new Binding();
        //        binding.Path = new PropertyPath("NextCommand"); //Name of the property in Datacontext
        //        btnNextInstall.SetBinding(Button.CommandProperty, binding);
        //    }

        //    else {

        //        try
        //        {

        //            Process.Start("VC_redist.x64.exe");
        //            btnNextInstall.IsEnabled = false;
        //        }
        //        catch (Exception ex)
        //        {

        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        }

    }
}
