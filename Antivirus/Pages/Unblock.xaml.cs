using FirstFloor.ModernUI.Windows.Controls;
using MahApps.Metro.Controls;
using NetFwTypeLib;
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
    /// Interaction logic for Unblock.xaml
    /// </summary>
    public partial class Unblock : MetroWindow
    {
        string internetAccess;
        public Unblock()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            firewallPolicy.Rules.Remove("Block Internet");


            MessageBox.Show("Internet Access UnBlocked ");
            internetAccess = "true";
            String path = @"C:\Users\" + Environment.UserName + "\\Documents\\test.txt";
            StreamWriter sw = new StreamWriter(path, false);
            sw.Write(internetAccess);
            sw.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
