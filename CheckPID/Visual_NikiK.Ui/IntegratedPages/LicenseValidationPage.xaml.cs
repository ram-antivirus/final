using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
    /// Interaction logic for LicenseValidationPage.xaml
    /// </summary>
    [Export(typeof(IBootstrapperPage))]
    public partial class LicenseValidationPage : UserControl, IBootstrapperPage
    {
        private LicenseValidationPageViewModel _model;
        private string _cleanedSerial = String.Empty;

        public LicenseValidationPage()
        {
            InitializeComponent();
        }

        private void SerialWithoutInputTextBoxTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var oldCaretIndex = SerialWithoutInputTextBox.CaretIndex;
            int newCaretIndex = oldCaretIndex;
            var oldSerial = _cleanedSerial;

            _cleanedSerial = SerialWithoutInputTextBox.Text;

            if (_model.UpperCase)
            {
                AllToUpper();
            }

            SerialWithoutInputTextBox.TextChanged -= SerialWithoutInputTextBoxTextChanged;

            SerialWithoutInputTextBox.Text = _cleanedSerial;

            if (oldCaretIndex > SerialWithoutInputTextBox.Text.Length)
            {
                newCaretIndex = SerialWithoutInputTextBox.Text.Length;
            }

            SerialWithoutInputTextBox.CaretIndex = newCaretIndex;

            _model.Validate();

            SerialWithoutInputTextBox.TextChanged += SerialWithoutInputTextBoxTextChanged;
            e.Handled = true;
        }

        private void SerialTextBoxTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
           
        }

        private void AllToUpper()
        {
            _cleanedSerial = _cleanedSerial.ToUpper();
        }

        private string InsertHyphens()
        {
            var displayText = string.Empty;

            for (int i = 0; i <= (_cleanedSerial.Length); i += _model.InputMaskSectionLength)
            {
                if (_cleanedSerial.Length > i + _model.InputMaskSectionLength)
                {
                    displayText += _cleanedSerial.Substring(i, _model.InputMaskSectionLength) + "-";
                }
                else
                {
                    displayText += _cleanedSerial.Substring(i);
                }
            }
            return displayText;
        }


        public string PageName
        {
            get { return "LicenseValidation"; }
        }

        public FrameworkElement Page
        {
            get { return this; }
        }

        public void Initialize(MainWindowViewModel mainViewModel)
        {
            _model = new LicenseValidationPageViewModel(mainViewModel);
            this.DataContext = _model;
            int maxlenght = _model.InputMaskSectionNo * _model.InputMaskSectionLength + _model.InputMaskSectionNo - 1;
            if (maxlenght > 0)
            {
                // Throw Exception
              
            }
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
            try
            {
                if (CheckForInternetConnection() == false)
                {
                    MessageBox.Show(" You are not connected to internet. Please connect internet to activate your product ");
                }
                else
                {
                    Thread.Sleep(1000);
                    StreamReader r = new StreamReader(Directory.GetCurrentDirectory() + "\\response");
                    string getres = r.ReadLine();
                    if (getres == "")
                    {
                        MessageBox.Show("Please Enter A product Key !!!!!!!");
                    }
                    //MessageBox.Show(getres);
                    if (getres == "success")
                    {
                        nxtLicense.IsEnabled = true;
                    }
                }
            }
            catch (Exception k) { MessageBox.Show("!!!!!Something goes wrong"); }
               
            
        
        
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            trialvalod();

        }

        // trial validate function 
        public void trialvalod()
        {
            try
            {
                Process myProcess;
                myProcess = Process.Start(Directory.GetCurrentDirectory() + "\\httpTrialReq.exe");

                // Process.Start(Directory.GetCurrentDirectory() + "\\httpTrialReq.exe");
                //      
                Thread.Sleep(15000);
                myProcess.Close();
                foreach (Process proc in Process.GetProcessesByName("httpTrialReq"))
                {
                    proc.Kill();
                }
                StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\responseTrial");
                string s = sr.ReadLine();
                //MessageBox.Show("readfile-->>" + s);
                if (s == "success")
                {

                    nxtLicense.IsEnabled = true;
                }
                else
                {

                    MessageBox.Show("You have already used trial version");
                }
            }
            catch (Exception kl)
            {
                //MessageBox.Show("Error Trial -->>>" + kl);
            }
        }


        // product validate product 

        public void keyvalid()
        {

            if (CheckForInternetConnection() == false)
            {
                MessageBox.Show(" You are not connected to internet. Please connect internet to activate your product ");
            }
            else
            {
                StreamReader r = new StreamReader(Directory.GetCurrentDirectory() + "\\response");
                string getres = r.ReadLine();
                if (getres == "")
                {
                    MessageBox.Show("Please Enter A product Key !!!!!!!");
                }
                //MessageBox.Show(getres);
                if (getres == "success")
                {
                    nxtLicense.IsEnabled = true;
                }
            }
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void nxtLicense_Click(object sender, RoutedEventArgs e)
        {

        }



    }
}
