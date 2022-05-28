using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.Web.Script.Serialization;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AccessFile;

namespace CheckPID
{
    public class CustomActions
    {
        // We will invoke CustomAction in UserRegistrationDlg
        [CustomAction]
        public static ActionResult CheckPID(Session session) // defaults as CustomAction1
        {
            session.Log("Begin CheckPID");
            string Pid = session["PIDKEY"];
            string username = session["USERNAME"];            
            
            var s = Request(Pid);
            
            dynamic obj = JsonConvert.DeserializeObject(s.Result);

            Account account = new Account() { 
                Name = username,
                Email = obj.email,
                Activation = obj.activation_date,
                Expiration = obj.exp_date
            };


            var json = new JavaScriptSerializer().Serialize(account);
            FileReadWrite.WriteFile(Path.GetTempPath()+"account.abc", json.ToString());                      
            
            if(obj.result != "success")
                session["PIDACCEPTED"] = "0";
            else
                session["PIDACCEPTED"] = "1";
            return ActionResult.Success;
        }

        async static Task<string> Request(String Pid)
        {
            var client = new HttpClient();

            // Create the HttpContent for the form to be posted.
            var requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("text", "This is a block of text"),
                new KeyValuePair<string, string>("secret_key", "57f61909cffb34.96918495"),
                new KeyValuePair<string, string>("slm_action", "slm_activate"),
                new KeyValuePair<string, string>("first_name", "sahil"),
                new KeyValuePair<string, string>("last_name", "lakhwani"),
                new KeyValuePair<string, string>("email", "shail@isn.in"),
                new KeyValuePair<string, string>("license_key", Pid),
                new KeyValuePair<string, string>("client_address", "sadasd545454"),
            });

            // Get the response.
            HttpResponseMessage response = await client.PostAsync(
                "http://www.yippeetechnology.com",
                requestContent);

            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            var a = "";
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.
                a = await reader.ReadToEndAsync();               
            }

            return a;
        }
    }
}

public class FingerPrint
{
    private static string fingerPrint = string.Empty;
    public static string Value()
    {
        if (string.IsNullOrEmpty(fingerPrint))
        {
            fingerPrint = GetHash("CPU >> " + cpuId() + "\nBIOS >> " + biosId() + "\nBASE >> " + baseId()
                + "\nDISK >> " + diskId() + "\nVIDEO >> " + videoId() + "\nMAC >> " + macId()
                                 );
        }
        return fingerPrint;
    }
    public static string GetHash(string s)
    {
        MD5 sec = new MD5CryptoServiceProvider();
        ASCIIEncoding enc = new ASCIIEncoding();
        byte[] bt = enc.GetBytes(s);
        return GetHexString(sec.ComputeHash(bt));
    }
    private static string GetHexString(byte[] bt)
    {
        string s = string.Empty;
        for (int i = 0; i < bt.Length; i++)
        {
            byte b = bt[i];
            int n, n1, n2;
            n = (int)b;
            n1 = n & 15;
            n2 = (n >> 4) & 15;
            if (n2 > 9)
                s += ((char)(n2 - 10 + (int)'A')).ToString();
            else
                s += n2.ToString();
            if (n1 > 9)
                s += ((char)(n1 - 10 + (int)'A')).ToString();
            else
                s += n1.ToString();
            if ((i + 1) != bt.Length && (i + 1) % 2 == 0) s += "-";
        }
        return s;
    }
    #region Original Device ID Getting Code
    //Return a hardware identifier
    private static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
    {
        string result = "";
        System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
        System.Management.ManagementObjectCollection moc = mc.GetInstances();
        foreach (System.Management.ManagementObject mo in moc)
        {
            if (mo[wmiMustBeTrue].ToString() == "True")
            {
                //Only get the first one
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
        }
        return result;
    }
    //Return a hardware identifier
    private static string identifier(string wmiClass, string wmiProperty)
    {
        string result = "";
        System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
        System.Management.ManagementObjectCollection moc = mc.GetInstances();
        foreach (System.Management.ManagementObject mo in moc)
        {
            //Only get the first one
            if (result == "")
            {
                try
                {
                    result = mo[wmiProperty].ToString();
                    break;
                }
                catch
                {
                }
            }
        }
        return result;
    }
    public static string cpuId()
    {
        //Uses first CPU identifier available in order of preference
        //Don't get all identifiers, as very time consuming
        string retVal = identifier("Win32_Processor", "UniqueId");
        if (retVal == "") //If no UniqueID, use ProcessorID
        {
            retVal = identifier("Win32_Processor", "ProcessorId");
            if (retVal == "") //If no ProcessorId, use Name
            {
                retVal = identifier("Win32_Processor", "Name");
                if (retVal == "") //If no Name, use Manufacturer
                {
                    retVal = identifier("Win32_Processor", "Manufacturer");
                }
                //Add clock speed for extra security
                retVal += identifier("Win32_Processor", "MaxClockSpeed");
            }
        }
        return retVal;
    }
    //BIOS Identifier
    private static string biosId()
    {
        return identifier("Win32_BIOS", "Manufacturer")
        + identifier("Win32_BIOS", "SMBIOSBIOSVersion")
        + identifier("Win32_BIOS", "IdentificationCode")
        + identifier("Win32_BIOS", "SerialNumber")
        + identifier("Win32_BIOS", "ReleaseDate")
        + identifier("Win32_BIOS", "Version");
    }
    //Main physical hard drive ID
    private static string diskId()
    {
        return identifier("Win32_DiskDrive", "Model")
        + identifier("Win32_DiskDrive", "Manufacturer")
        + identifier("Win32_DiskDrive", "Signature")
        + identifier("Win32_DiskDrive", "TotalHeads");
    }
    //Motherboard ID
    private static string baseId()
    {
        return identifier("Win32_BaseBoard", "Model")
        + identifier("Win32_BaseBoard", "Manufacturer")
        + identifier("Win32_BaseBoard", "Name")
        + identifier("Win32_BaseBoard", "SerialNumber");
    }
    //Primary video controller ID
    private static string videoId()
    {
        return identifier("Win32_VideoController", "DriverVersion")
        + identifier("Win32_VideoController", "Name");
    }
    //First enabled network card ID
    private static string macId()
    {
        return identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
    }
    #endregion
}

public class Account
{
    private string name;
    public string Name
    {
        get { return this.name; }
        set
        {
            if (this.name!= value)
            {
                this.name= value;
            }
        }
    }

    private string email;    
    public string Email
    {
        get { return this.email; }
        set
        {
            if (this.email!= value)
            {
                this.email= value;
            }
        }
    }

    private DateTime activation;
    public DateTime Activation
    {
        get { return this.activation; }
        set
        {
            if (this.activation != value)
            {
                this.activation = value;
            }
        }
    }

    private DateTime expiration;
    public DateTime Expiration
    {
        get { return this.expiration; }
        set
        {
            if (this.expiration != value)
            {
                this.expiration = value;
            }
        }
    }
}

