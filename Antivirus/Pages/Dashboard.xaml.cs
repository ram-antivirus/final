using FirstFloor.ModernUI.Windows.Controls;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Antivirus.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : MetroWindow
    {
        public Dashboard()
        {
            InitializeComponent();
            ScnDate.Text = Antivirus.Properties.Settings.Default.ScanDate.ToString();
            ScnTime.Text = Antivirus.Properties.Settings.Default.ScanTime.ToString();
            ObjScn.Text = Antivirus.Properties.Settings.Default.TotScanHome.ToString();
            time.Text = Antivirus.Properties.Settings.Default.TimeElapsed.ToString();
            IssuesFound.Text = Antivirus.Properties.Settings.Default.Totalssues.ToString();
            foreach (var y in Antivirus.Properties.Settings.Default.Arraylst)
            {
                    lstIssues.Items.Add(y);
            }

        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {



        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {




        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Report"; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                StreamWriter wr = new StreamWriter(filename);
                wr.WriteLine("Yippee Technology");

                wr.WriteLine("www.ramantivirus.com" + Environment.NewLine);
                wr.WriteLine("-Log Details-" + Environment.NewLine);
                wr.WriteLine("Scan Date: {0}" + Environment.NewLine + "Scan Time: {1}" + Environment.NewLine + "Logfile: {2}" + Environment.NewLine, Antivirus.Properties.Settings.Default.ScanDate, Antivirus.Properties.Settings.Default.ScanTime, dlg.FileName);
                wr.WriteLine("-Scan Summary-" + Environment.NewLine + "Objects Scanned:{0}" + Environment.NewLine + "Time Elapsed: {1}" + Environment.NewLine, Antivirus.Properties.Settings.Default.TotScanHome, Antivirus.Properties.Settings.Default.TimeElapsed);
                wr.WriteLine(Environment.NewLine + "---Issues Found---");
                foreach (var obj in Antivirus.Properties.Settings.Default.Arraylst)
                {
                    wr.WriteLine(obj);
                }
                wr.Flush();
                wr.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
