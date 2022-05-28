using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePathNameSpace
{
    [Serializable]
    public class FilePath
    {
        private string file_path;
        public string File_Path
        {
            get { return this.file_path; }
            set
            {
                if (this.file_path != value)
                {
                    this.file_path = value;
                    this.NotifyPropertyChanged("Path");
                }
            }
        }

        private string virus_type;
        public string Virus_Type
        {
            get { return this.virus_type; }
            set
            {
                if (this.virus_type != value)
                {
                    this.virus_type = value;
                    this.NotifyPropertyChanged("Virus_Type");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
