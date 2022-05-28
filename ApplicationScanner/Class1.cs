using System;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace ApplicationScanner
{
    public class Class1
    {
        ObservableCollection<string> myapp = new ObservableCollection<string>();
        public async void RunApps()
        {
            try
            {
                string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                using (var baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (var key = baseKey.OpenSubKey(uninstallKey))
                    {
                        foreach (string skName in key.GetSubKeyNames())
                        {
                            using (RegistryKey sk = key.OpenSubKey(skName))
                            {
                                try
                                {

                                    string displayName = sk.GetValue("DisplayName").ToString();
                                  
                                    Console.WriteLine(displayName);
                                    myapp.Add(displayName);

                                  

                                }
                                catch (Exception vdf)
                                {

                                }


                            }
                        }

                    }
                }

                // current user 

                using (var baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    using (var key = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))
                    {
                        foreach (string skName in key.GetSubKeyNames())
                        {
                            using (RegistryKey sk = key.OpenSubKey(skName))
                            {
                                try
                                {

                                    string displayName = sk.GetValue("DisplayName").ToString();
                                   
                                    Console.WriteLine(displayName);
                                    myapp.Add(displayName);

                                 
                                }
                                catch (Exception vdf)
                                {

                                }


                            }

                        }

                    }
                }

                // wow path 

                using (var baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (var key = baseKey.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"))
                    {
                        foreach (string skName in key.GetSubKeyNames())
                        {
                            using (RegistryKey sk = key.OpenSubKey(skName))
                            {
                                try
                                {

                                    string displayName = sk.GetValue("DisplayName").ToString();
                                   
                                    Console.WriteLine(displayName);
                                    myapp.Add(displayName);

                                  
                                }
                                catch (Exception vdf)
                                {

                                }


                            }
                        }

                    }
                }

             

            }
            catch (Exception j)
            {
                // MessageBox.Show("Exception In RunApps " + j);
            }
        }

    }
}
