using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using Visual_NikiK.Ui.IntegratedPages;

using System.Net.NetworkInformation;

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

        public string LocalizedPageName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string LocalizedPageNameId => throw new NotImplementedException();

        public bool AddToProcessStepBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            btntrial.IsEnabled = false;
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
                    MessageBox.Show("Please Enter A product Key!");
                }
                //MessageBox.Show(getres);
                if (getres == "success")
                {
                    nxtLicense.IsEnabled = true;
                }
            }
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
                MessageBox.Show("readfile-->>" + s);
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
              //  MessageBox.Show("Error Trial -->>>" + kl);
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
                    MessageBox.Show("Please Enter A product Key!");
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string Name = textBoxName.Text;
            string email = textBox_Email.Text;
            string country = textBox_Country.Text;
            string contactNo = textBox_Contact.Text;
            string city1 = city.Text;
            string Dealer_Code = Dealercode.Text;
            try
            {

                using (WebClient client = new WebClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;
                    byte[] response =
                    client.UploadValues("https://ramantivirus.com/yippee_trial_checker.php", new NameValueCollection()
       {
          { "secure_key", "D816E18914FF42BAF3D93C6BFECE7" },
                {"trial_key","YIPPEETRIAL"},

                {"mac_addr",getmac()},
                {"product_type","RAM Ultimate AntiVirus"},
                {"first_name",Name},
                {"Dealer_Code",Dealer_Code },
                {"email_id",email},
                {"country",country },
                {"contact_number",contactNo },
                { "city",city1 }
       });

                    string result = System.Text.Encoding.UTF8.GetString(response);
                    //   MessageBox.Show(result);
                    //nxtLicense.IsEnabled = true;
                    if (result.Contains("success"))
                    {
                        nxtLicense.IsEnabled = true;
                        StreamWriter fg = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\accountultimate.abc");
                        fg.WriteLine(result);
                        fg.Flush();
                        fg.Close();
                    }
                    else {
                        MessageBox.Show("You might have already used a trial version.Try again with a valid key");
                    }

                }
            }
            catch (Exception lo)
            {
                MessageBox.Show("Try Again!!");

            }
             
        }



        static string getmac()
        {
            var macAddr =
               (
                   from nic in NetworkInterface.GetAllNetworkInterfaces()
                   where nic.OperationalStatus == OperationalStatus.Up
                   select nic.GetPhysicalAddress().ToString()
               ).FirstOrDefault();

            return macAddr;

        }

        private void firsttxt_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (firsttxt.Text.Length == 5)
            {
                secondtxt.Focus();
            }
        }

        private void secondtxt_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (secondtxt.Text.Length == 5)
            {
                thirdtxt.Focus();
            }
        }

        private void thirdtxt_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (thirdtxt.Text.Length == 5)
            {
                fourthtxt.Focus();
            }
        }

        private void fourthtxt_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (fourthtxt.Text.Length == 5)
            {
                fifthtxt.Focus();
            }
        }

        private void fifthtxt_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            int sum;

            if (fifthtxt.Text.Length == 5)
            {
                sum = firsttxt.Text.Length + secondtxt.Text.Length + thirdtxt.Text.Length + fourthtxt.Text.Length + fifthtxt.Text.Length;
                if (sum == 25)
                {

                }
                else
                {
                    MessageBox.Show("Enter a complete key!");
                }
            }
        }

        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            string finalkey = firsttxt.Text + secondtxt.Text + thirdtxt.Text + fourthtxt.Text + fifthtxt.Text;
            string Name = textBoxName.Text;
            string email = textBox_Email.Text;
            string country = textBox_Country.Text;
            string contactNo = textBox_Contact.Text;
            string city1 = city.Text;
            string Dealer_Code = Dealercode.Text;
            //  MessageBox.Show(Name +"\n"+ email +"\n"+country+"\n"+contactNo+"\n"+city1);
            NameValueCollection resp = new NameValueCollection(){
                { "slm_action", "slm_activate" },

                { "secret_key", "59ad3e7eb41330.32902137" },
                {"license_key",finalkey},
                { "client_address",getmac()},
                {"pc_name",System.Net.Dns.GetHostName()},
                {"product_id","18031"},
                {"product_name","RAM Ultimate AntiVirus"},
                {"first_name",Name},
               {"Dealer_Code",Dealer_Code },
                {"email_id",email},
                {"country",country },
                {"contact_number",contactNo },
                { "city",city1 }
                    };
            //MessageBox.Show("   KEY        VALUE");
            //  for (int i = 0; i < resp.Count; i++)
            //  {
            //      string first = resp.GetKey(i);
            //      string second = resp.Get(i);
            //      MessageBox.Show(i.ToString()+"\t"+first +"\t"+second);
            //  }

            try
            {


                using (WebClient client = new WebClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;
                    byte[] response =
                    client.UploadValues("https://ramantivirus.com/", resp);

                    //  MessageBox.Show(System.Text.Encoding.UTF8.GetString(response));
                    string result = System.Text.Encoding.UTF8.GetString(response);
                    //     MessageBox.Show(result);
                    if (result.Contains("success"))
                    {
                        if (String.IsNullOrEmpty(textBoxName.Text) && String.IsNullOrEmpty(textBox_Email.Text) && String.IsNullOrEmpty(textBox_Contact.Text) && String.IsNullOrEmpty(textBox_Country.Text) && String.IsNullOrEmpty(city.Text))
                        {
                            MessageBox.Show("Please Enter All Details");
                        }
                        else
                        {
                            nxtLicense.IsEnabled = true;
                            StreamWriter fg = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\accountultimate.abc");
                            fg.WriteLine(result);
                            fg.Flush();
                            fg.Close();
                            MessageBox.Show("Key is validated successfully, now click on next button");
                        }
                    }
                    else
                    {
                        MessageBox.Show("You have entered a wrong key, please try again!");
                    }

                }
            }
            catch (Exception lo)
            {
                MessageBox.Show("Try Again!");

            }

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
