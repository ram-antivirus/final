using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for blockUrl.xaml
    /// </summary>
    public partial class blockUrl : UserControl
    {
        private ObservableCollection<Person> person;
        String str;
        public blockUrl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (urltext.Text=="")
            {
                MyPopup ulrt = new MyPopup();
                ulrt.txt.Text = "You can block the website from specific advertising URLs appearing on your computer. You need to add a website address followed by https://  inside the box which you want to block";
               
                ulrt.ShowDialog();
            }
            else
            {
                string geturl = urltext.Text.ToString();
                String path = @"C:\Windows\System32\drivers\etc\hosts";
                StreamWriter sw = new StreamWriter(path, true);
                String sitetoblock = "\n 127.0.0.1" + " " + geturl;
                sw.Write(sitetoblock);
                sw.Close();
                MessageBox.Show("Site Blocked");
                urltext.Clear();
            }
        }

        private void lstItems_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            try
            {
                urltext.Clear();
                string line = null;
                List<string> ls = new List<string>();

                

                //else
                //{
                using (StreamReader reader = new StreamReader(@"C:\Windows\System32\drivers\etc\hosts"))
                {
                    using (StreamWriter writer = new StreamWriter(@"C:\Windows\System32\drivers\etc\output.txt"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {


                            if (line.Contains("www") || line.Contains(".com") && line.Contains("127.0.0.1"))
                            {
                                person = new ObservableCollection<Person>();
                                ls.Add(line);
                                foreach (var match in ls)
                                {
                                    person.Add(new Person() { Path = match });
                                }

                                lstItems.ItemsSource = person;

                            }
                        }
                        writer.Close();
                    }
                    reader.Close();
                }

                if (lstItems.Items.Count == 0)
                {
                    MyPopup shw = new MyPopup();
                    shw.txt.Text = "Url unblock feature removes blocked website from the list of all blocked websites. It allows access to all URL's except the one's you blocked. To use this feature you need to add atlest one URL to unblock.";
                    shw.ShowDialog();
                }
            }
            catch (Exception k)
            { }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Person p1 = (Person)lstItems.SelectedItem;
            try
           {
              
               if (lstItems.SelectedItem == null)
               {
                   MyPopup unblk = new MyPopup();
                   unblk.txt.Text = "Select an option to unblock";
                   unblk.ShowDialog();
                   
               }
            urltext.Clear();
            
            str = p1.Path;
            string line = null;
           
                using (StreamReader reader = new StreamReader(@"C:\Windows\System32\drivers\etc\hosts"))
                {
                    using (StreamWriter writer = new StreamWriter(@"C:\Windows\System32\drivers\etc\output.txt"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line == str)
                                continue;
                            writer.WriteLine(line);
                        }
                        writer.Close();
                    }
                    reader.Close();

                }
            }
            catch(Exception l)
            {
                //MessageBox.Show("File is being used by another process-->>"+l);
            }

            try
            {
                string str2;
                StreamReader reader1 = new StreamReader(@"C:\Windows\System32\drivers\etc\output.txt");
                str2 = reader1.ReadToEnd();
               // MessageBox.Show(str2);
                reader1.Close();
                StreamWriter writer1 = new StreamWriter(@"C:\Windows\System32\drivers\etc\hosts");
                writer1.Write(str2);
                writer1.Flush();
                writer1.Close();
                Person p = new Person();
                person.Remove(p1);
            }
            catch
            {
                //MessageBox.Show("File is being used by another process");
            }
        }

       
    }
    public class Person
    {
        public string Path { get; set; }
        public int Address { get; set; }
    }
}
