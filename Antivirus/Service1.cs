﻿using Antivirus.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Antivirus
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public void DoWork(string msg)
        {
            UsbDetect u = new UsbDetect();
            u.txt.Text = msg;
            u.txt.Text = msg;

            u.Show();
        }
    }
}
