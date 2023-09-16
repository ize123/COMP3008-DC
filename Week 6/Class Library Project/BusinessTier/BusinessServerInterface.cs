using Class_Library_Project;
using server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessTier
{
    [ServiceContract]
    public interface BusinessServerInterface
    {
        [OperationContract]
        int GetNumEntries();
        [OperationContract]
        [FaultContract(typeof(ServerError))]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out MemoryStream image);
        [OperationContract]
        DataStruct Search(string searchString);
    }
}
