using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Antivirus
{
    class Potential:INotifyPropertyChanged
    {
          private string name;
            public string Name
            {
                get { return this.name; }
                set
                {

                    if (this.name != value)
                    {
                        this.name = value;
                        this.NotifyPropertyChanged("Name");
                    }
                }
            }
            private string publisher;
            public string Publisher
            {
                get { return this.publisher; }
                set
                {

                    if (this.publisher != value)
                    {
                        this.publisher = value;
                        this.NotifyPropertyChanged("Publisher");
                    }
                }
            }
            private string path;
            public string Path
            {
                get { return this.path; }
                set
                {

                    if (this.path != value)
                    {
                        this.path = value;
                        this.NotifyPropertyChanged("Path");
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
