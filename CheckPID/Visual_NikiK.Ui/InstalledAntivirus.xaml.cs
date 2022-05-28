using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
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
       
        private ObservableCollection<Person> person;


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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process.Start("appwiz.cpl");

        }

        private void nxtUninstall_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Antivirus("displayName").Equals("Windows Defender"))
            {
                nxtUninstall.IsEnabled = true;
            }
            if (Antivirus("displayName").Equals(null))
            {
                nxtUninstall.IsEnabled = true;
            }
            else
            {

                List<string> ls = new List<string>();
                person = new ObservableCollection<Person>();
                ls.Add(Antivirus("displayName"));
                if (Antivirus("displayName") != "Windows Defender")
                {
                    foreach (var match in ls)
                    {
                        person.Add(new Person() { Path = match });
                    }
                    lstItems.ItemsSource = person;
                    btnUninstall.IsEnabled = true;
                    chkupdate.Content = "Refresh";
                    if (chkupdate.Content == "Refresh")
                    {
                        if (Antivirus("displayName").Equals(null))
                        {

                            nxtUninstall.IsEnabled = true;
                        }
                    }
                }
            }
        }
private static string Antivirus(string type)
        {
            string computer = Environment.MachineName;
            string wmipathstr = @"\\" + Environment.MachineName + @"\root\SecurityCenter2";
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmipathstr,
                  "SELECT * FROM AntivirusProduct");
                ManagementObjectCollection instances = searcher.Get();
                //MessageBox.Show(instances.Count.ToString()); 
                foreach (ManagementObject queryObj in instances)
                {
                    return queryObj[type].ToString();
                }
            }

            catch (Exception e)
            {

            }

            return null;

        }

        }

    }

