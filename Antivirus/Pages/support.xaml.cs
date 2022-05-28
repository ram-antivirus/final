using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for support.xaml
    /// </summary>
    public partial class support : UserControl
    {
        public support()
        {
            InitializeComponent();
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Tile_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://support.ramantivirus.com/");
            }
            catch (Exception k)
            {

            }
        }

        private void Tile_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://ramantivirus.com/contact-us/");
            }
            catch (Exception k)
            {

            }
        }

        private void Tile_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://ramantivirus.com/remote-support/");
            }
            catch (Exception k)
            {

            }
        }

        private void Tile_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://ramantivirus.com/contact-us/");
            }
            catch (Exception k)
            {

            }
        }

        private void Tile_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://ramantivirus.com");
            }
            catch (Exception k)
            {

            }
        }

        private void Tile_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://support.ramantivirus.com/");
            }
            catch (Exception k)
            {

            }
        }
    }
}
