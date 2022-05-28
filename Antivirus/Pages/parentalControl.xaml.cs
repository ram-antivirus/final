using FirstFloor.ModernUI.Windows.Controls;
//using Microsoft.TeamFoundation.Build.Common;
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
    /// Interaction logic for parentalControl.xaml
    /// </summary>
    public partial class parentalControl : UserControl
    {
        public static String internetAccess;
        public parentalControl()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            StreamReader reader = new StreamReader(@"C:\Users\"+Environment.UserName+"\\Documents\\test.txt");
            internetAccess = reader.ReadLine();
            reader.Close();


            if (internetAccess == "true")
            {
                //var dlg = new ModernDialog
                //{
                //    Content = "Your Internet Access will be Blocked. Click Ok to Block the Internet access.",
                //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                //};
                //dlg.Buttons = new Button[] { dlg.OkButton, dlg.CancelButton };
                //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                //// dlg.OkButton.FontWeight = 
                //dlg.OkButton.FontWeight = FontWeights.Bold;
                //dlg.ShowDialog();
                //if (dlg.DialogResult.HasValue && dlg.DialogResult.Value)
                //{
                //    INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(
                //    Type.GetTypeFromProgID("HNetCfg.FWRule"));
                //    firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                //    firewallRule.Description = "Used to block all internet access.";
                //    firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
                //    firewallRule.Enabled = true;
                //    firewallRule.InterfaceTypes = "All";
                //    firewallRule.Name = "Block Internet";

                //    INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                //        Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                //    firewallPolicy.Rules.Add(firewallRule);

                //    MessageBox.Show("Internet Access Blocked ");
                //    internetAccess = "false";
                //    String path = @"C:\Users\" + Environment.UserName + "\\Documents\\test.txt";
                //    StreamWriter sw = new StreamWriter(path, false);
                //    sw.Write(internetAccess);
                //    sw.Close();

                ParentalPopup lkl = new ParentalPopup ();
                lkl.Show();


                //}
                //else
                //{

                //    MessageBox.Show("Cancel clicked");
                //}
            }
            else
            {
                //var dlg = new ModernDialog
                //{
                //    Content = "Your Internet Access is Blocked. Click Ok to UnBlock the Internet access.",
                //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                //};
                //dlg.Buttons = new Button[] { dlg.OkButton, dlg.CancelButton };
                //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                //// dlg.OkButton.FontWeight = 
                //dlg.OkButton.FontWeight = FontWeights.Bold;
                //dlg.ShowDialog();
                //if (dlg.DialogResult.HasValue && dlg.DialogResult.Value)
                //{
                //    INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                //    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                //    firewallPolicy.Rules.Remove("Block Internet");


                //    MessageBox.Show("Internet Access UnBlocked ");
                //    internetAccess = "true";
                //    String path = @"C:\Users\" + Environment.UserName + "\\Documents\\test.txt";
                //    StreamWriter sw = new StreamWriter(path, false);
                //    sw.Write(internetAccess);
                //    sw.Close();

                //}
                //else
                //{

                //    MessageBox.Show("Cancel clicked");
                //}

                Unblock kjk = new Unblock();
                kjk.Show();


            }
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/blockUrl.xaml", this);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/blockUrl.xaml", this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!File.Exists(@"C:\Users\" + Environment.UserName + "\\Documents\\test.txt"))
                {
                    StreamWriter wr = new StreamWriter(@"C:\Users\" + Environment.UserName + "\\Documents\\test.txt");
                    wr.Write("true");
                    wr.Flush();
                    wr.Close();

                }
                StreamReader reader = new StreamReader(@"C:\Users\" + Environment.UserName + "\\Documents\\test.txt");
                internetAccess = reader.ReadLine();
                reader.Close();
            }
            catch (Exception u)
            { 
            }
            try
            {

                if (internetAccess == "true")
                {
                    //var dlg = new ModernDialog
                    //{
                    //    Content = "Your Internet Access will be Blocked. Click Ok to Block the Internet access.",
                    //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                    //};
                    //dlg.Buttons = new Button[] { dlg.OkButton, dlg.CancelButton };
                    //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                    //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                    //// dlg.OkButton.FontWeight = 
                    //dlg.OkButton.FontWeight = FontWeights.Bold;
                    //dlg.ShowDialog();
                    //if (dlg.DialogResult.HasValue && dlg.DialogResult.Value)
                    //{
                    //    INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(
                    //    Type.GetTypeFromProgID("HNetCfg.FWRule"));
                    //    firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                    //    firewallRule.Description = "Used to block all internet access.";
                    //    firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
                    //    firewallRule.Enabled = true;
                    //    firewallRule.InterfaceTypes = "All";
                    //    firewallRule.Name = "Block Internet";

                    //    INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                    //        Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                    //    firewallPolicy.Rules.Add(firewallRule);

                    //    MessageBox.Show("Internet Access Blocked ");
                    //    internetAccess = "false";
                    //    String path = @"C:\Users\" + Environment.UserName + "\\Documents\\test.txt";
                    //    StreamWriter sw = new StreamWriter(path, false);
                    //    sw.Write(internetAccess);
                    //    sw.Close();

                     ParentalPopup lkl = new ParentalPopup ();
                     lkl.Show();


                    
                    //else
                    //{

                    //  //  MessageBox.Show("Cancel clicked");
                    //}
                }
                else
                {
                    //var dlg = new ModernDialog
                    //{
                    //    Content = "Your Internet Access is Blocked. Click Ok to UnBlock the Internet access.",
                    //    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"))
                    //};
                    //dlg.Buttons = new Button[] { dlg.OkButton, dlg.CancelButton };
                    //dlg.OkButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF007ACC"));
                    //dlg.OkButton.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                    //// dlg.OkButton.FontWeight = 
                    //dlg.OkButton.FontWeight = FontWeights.Bold;
                    //dlg.ShowDialog();
                    //if (dlg.DialogResult.HasValue && dlg.DialogResult.Value)
                    //{
                    //    INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                    //    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                    //    firewallPolicy.Rules.Remove("Block Internet");


                    //    MessageBox.Show("Internet Access UnBlocked ");
                    //    internetAccess = "true";
                    //    String path = @"C:\Users\" + Environment.UserName + "\\Documents\\test.txt";
                    //    StreamWriter sw = new StreamWriter(path, false);
                    //    sw.Write(internetAccess);
                    //    sw.Close();

                    //}
                    //else
                    //{

                    //   // MessageBox.Show("Cancel clicked");
                    //}


                    Unblock kjj = new Unblock();
                    kjj.Show();

                }
            }
            catch (Exception o)
            { }
        }

        private void restrict_MouseEnter(object sender, MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(39, 146, 218);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            restrict.Background = backgroundBrush;
        }

        private void restrict_MouseLeave(object sender, MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(0, 122, 204);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            restrict.Background = backgroundBrush;
        }

        private void acess_MouseEnter(object sender, MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(39, 146, 218);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            acess.Background = backgroundBrush;
        }

        private void acess_MouseLeave(object sender, MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(0, 122, 204);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            acess.Background = backgroundBrush;
        }

        private void Hyperlink_RequestNavigate_1(object sender, RequestNavigateEventArgs e)
        {

        }

       
    }
}
