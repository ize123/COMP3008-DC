using Class_Library_Project;
using server;
using Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessTier
{
    internal class BusinessServer : BusinessServerInterface
    {
        ServerInterface dataAccess;
        private static uint logNumber = 0;

        public BusinessServer()
        {
            ChannelFactory<ServerInterface> dataAccessFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            tcp.SendTimeout = TimeSpan.FromMinutes(5);
            tcp.MaxBufferSize = 2000000000;
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/DataService";
            dataAccessFactory = new ChannelFactory<ServerInterface>(tcp, URL);
            dataAccess = dataAccessFactory.CreateChannel();
        }

        public int GetNumEntries()
        {
            Log(string.Format("Getting the total number of entries in the database: Total = {0}", dataAccess.GetNumEntries()));
            return dataAccess.GetNumEntries();   
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out MemoryStream image)
        {
            try
            {
                dataAccess.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName, out image);
                Log(string.Format("Getting Data values for entry: \nIndex Searched: {0}\nReturned Names: {1} {2}", index, fName, lName));
            }          
            catch(FaultException<ServerError> e)
            {
                Log(string.Format("Fault contract thrown: {0}", e.Message));
                throw e;
            }
        }

        public DataStruct Search(string searchString)
        {
            DataStruct dataStruct = dataAccess.Search(searchString);
            if (dataStruct != null)
            {
                Log(string.Format("Searching for entry in database: {0} SUCCESS", searchString));
            }
            else
            {
                Log(string.Format("Searching for entry in database: {0} FAIL", searchString));
            }           
            return dataStruct;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(string logString)
        {
            String timeString = DateTime.Now.ToString();
            Console.WriteLine(string.Format("Log Number: {0}\nTime: {1}\n{2}", logNumber, timeString, logString));
            logNumber++;
        }
    }
}
