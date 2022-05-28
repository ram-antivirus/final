using FilePathNameSpace;
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
    /// Interaction logic for FancyPopup.xaml
    /// </summary>
    public partial class FancyPopup : UserControl
    {
        public FancyPopup(string fileName, string virusName)
        {
            this.VirusName.Text = fileName + ":->" + virusName +".... File Quarantined";
            try
            {
                    


                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine");

                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine\"+System.IO.Path.GetFileName(fileName)));

                    {

                        File.Delete((Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine\" + System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(fileName), ".qua")));
                        File.Delete(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine", System.IO.Path.GetFileName(fileName) + ".bin"));

                      //  MessageBox.Show("File Exist:-" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine", Path.GetFileName(file.File_Path) + ".bin"));
                    }

                    File.Move(fileName, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine\" + System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(fileName), ".qua"));

                    string serializationFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TechesperData\Quarantine", System.IO.Path.GetFileName(fileName) + ".bin");

                    //serialize
                    using (Stream stream = File.Open(serializationFile, FileMode.CreateNew))
                    {
                        var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                        bformatter.Serialize(stream, fileName);
                    }


                
            }
            catch (Exception lk)
            {
               
            }
            InitializeComponent();
        }

       
    }
}
