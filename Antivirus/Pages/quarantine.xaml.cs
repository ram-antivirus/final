using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Collections.ObjectModel;
using System.IO;
using FilePathNameSpace;

namespace Antivirus.Pages
{
    /// <summary>
    /// Interaction logic for aboutus.xaml
    /// </summary>
    public partial class quarantine : UserControl
    {
        public ObservableCollection<FilePath> QuarantinedItems;


        public quarantine()
        {
            InitializeComponent();                           
        }              

      

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        { 
           
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //CheckBox chk = sender as CheckBox;
            //Episodes epi = chk.DataContext as Episodes;
            //var season = epi.Name;
            //if (chk.IsChecked == false)
            //{
            //    MessageBox.Show("Unchecked");
               
              
            //}
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string QuarantinedFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"TechesperData\Quarantine"); ;
            for (int i = QuarantinedList.SelectedItems.Count - 1; i >= 0; i--)
            {
                var file = ((FilePath)QuarantinedList.SelectedItems[i]).File_Path;

                string deletePath = QuarantinedFolder+"\\" + System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(file),".qua");
                File.Delete(deletePath);
                string info =System.IO.Path.GetFileName(file)+ ".bin";
                File.Delete(QuarantinedFolder+"\\"+info);


                QuarantinedItems.Remove(QuarantinedItems.Where(item => item.File_Path == file).Single());                            

            }
        }
        
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            QuarantinedItems = new ObservableCollection<FilePath>();
            QuarantinedList.ItemsSource = QuarantinedItems;

            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),@"TechesperData\Quarantine");
            
            string[] files = Directory.GetFiles(path,"*.bin");

            foreach(var file in files)
            {
                using (Stream stream = File.Open(file, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    FilePath newFilePath = (FilePath)bformatter.Deserialize(stream);

                    QuarantinedItems.Add(newFilePath);
                }
            }
            
        }

        private void SelectAll_Checked(object sender, RoutedEventArgs e)
        {
            QuarantinedList.SelectAll();
        }

        private void SelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            QuarantinedList.UnselectAll();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string QuarantinedFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"TechesperData\Quarantine"); ;
            for (int i = QuarantinedList.SelectedItems.Count - 1; i >= 0; i--)
            {
                var file = ((FilePath)QuarantinedList.SelectedItems[i]).File_Path;

                string localFile = System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(file),".qua");

                File.Move(QuarantinedFolder+"\\"+localFile, file);


                string info = System.IO.Path.GetFileName(file) + ".bin";
                File.Delete(QuarantinedFolder + "\\" + info);

                QuarantinedItems.Remove(QuarantinedItems.Where(item => item.File_Path == file).Single());                            
            }
        }
       
    }
}
