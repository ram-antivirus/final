using FirstFloor.ModernUI.Windows.Controls;
using MahApps.Metro.Controls;
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

namespace Antivirus.Pages
{
    /// <summary>
    /// Interaction logic for MyPopup.xaml
    /// </summary>
    public partial class MyPopup : MetroWindow
    {
        Antivirus.Pages.ApplicationScan a = new ApplicationScan();
       
        public MyPopup()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }
    }
}
