using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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

namespace home.Pages
{
    /// <summary>
    /// Interaction logic for cleanup.xaml
    /// </summary>
    /// 
    public partial class cleanup : UserControl
    {


        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/pcclean.xaml", this);
        }

        private void Tile_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/browser.xaml", this);
        }

        private void Tile_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/registry.xaml", this);
        }

        private void Tile_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("Pages/tools.xaml", this);
        }

        private void Tile_MouseEnter(object sender, MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(39, 146, 218);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            pcclean.Background = backgroundBrush;
        }

        private void Tile_MouseLeave(object sender, MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(0, 122, 204);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            pcclean.Background = backgroundBrush;
        }

        private void regist_MouseEnter(object sender, MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(39, 146, 218);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            regist.Background = backgroundBrush;
        }

        private void regist_MouseLeave(object sender, MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(0, 122, 204);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            regist.Background = backgroundBrush;
        }

        private void brows_MouseEnter(object sender, MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(39, 146, 218);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            brows.Background = backgroundBrush;
        }

        private void brows_MouseLeave(object sender, MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(0, 122, 204);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            brows.Background = backgroundBrush;
        }

        private void tools_MouseEnter(object sender, MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(39, 146, 218);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            tools.Background = backgroundBrush;
        }

        private void tools_MouseLeave(object sender, MouseEventArgs e)
        {
            Color color = System.Windows.Media.Color.FromRgb(0, 122, 204);
            SolidColorBrush backgroundBrush = new SolidColorBrush(color);
            tools.Background = backgroundBrush;
        }
    }
}