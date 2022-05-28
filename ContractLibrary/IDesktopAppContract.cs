using FilePathNameSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ContractLibrary
{
    [ServiceContract(Namespace = "http://NetNamedPipeBindingHost")]
    public interface IDesktopAppContract
    {        
            [OperationContract]
            void PrintVirusDetails(FilePath path);

            [OperationContract]
            void CallPop(string msg);

        

            [OperationContract]
            void Result(string a, string b);
            [OperationContract]
            void callAshu();
        





    }
    [ServiceContract]
    public interface IStringReverser
    {
        [OperationContract]
        void ReverseString(string value);
    }
}

