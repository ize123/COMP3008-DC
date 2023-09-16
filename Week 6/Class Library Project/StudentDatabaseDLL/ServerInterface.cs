using Class_Library_Project;
using server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [ServiceContract]
    public interface ServerInterface
    {
        // Each of these service function contracts. They need to be tagged as OperationContracts.
        [OperationContract]
        int GetNumEntries();
        [OperationContract]
        [FaultContract(typeof(ServerError))]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out MemoryStream image);
        [OperationContract]
        DataStruct Search(string searchString);
    }
}
