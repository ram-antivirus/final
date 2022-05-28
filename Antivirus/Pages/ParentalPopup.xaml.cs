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
    /// Interaction logic for ParentalPopup.xaml
    /// </summary>
    public partial class ParentalPopup : MetroWindow
    {
        public static String internetAccess;
        public ParentalPopup()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(
                   Type.GetTypeFromProgID("HNetCfg.FWRule"));
            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            firewallRule.Description = "Used to block all internet access.";
            firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            firewallRule.Enabled = true;
            firewallRule.InterfaceTypes = "All";
            firewallRule.Name = "Block Internet";

            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            firewallPolicy.Rules.Add(firewallRule);

            MessageBox.Show("Internet Access Blocked ");
            internetAccess = "false";
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
