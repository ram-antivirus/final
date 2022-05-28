using Antivirus.Pages;
using ContractLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antivirus
{
    class StringReverser: IStringReverser
    {
        public void ReverseString(string value)
        {
            string a = null;
            UsbDetect u = new UsbDetect();
            u.txt.Text = value;
            u.Show();

            
        }
    }
}
