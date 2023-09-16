using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //this should be more descriptive.
            Console.WriteLine("Hey welcome to my server");
            //This is thea ctual host service system
            ServiceHost host;
            //This represents a tcp/ip binding inWindows network stack
            NetTcpBinding tcp = new NetTcpBinding();

            //Bind server to the implementation of DataServer
            host = new ServiceHost(typeof(DataServer));
            //Present the publicly accessible interface to the client. 0.0.0.0 tells .net to accept on any interface. :8100 means this will use port 8100. DataService is a name for the actual service, this can be any string.
            host.AddServiceEndpoint(typeof(ServerInterface), tcp, "net.tcp://0.0.0.0:8100/DataService");

            //Open the host for business
            host.Open();
            Console.WriteLine("System Online");
            Console.ReadLine();
            //close the host
            host.Close();
        }
    }
}
