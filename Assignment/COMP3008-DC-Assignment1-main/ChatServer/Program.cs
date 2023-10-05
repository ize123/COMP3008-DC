using System;
using System.ServiceModel;

namespace ChatServer
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Chat server opening...");

            ServiceHost serviceHost = new ServiceHost(typeof(ChatServer));

            NetTcpBinding netTCPBinding = new NetTcpBinding
            {
                MaxReceivedMessageSize = 1_000_000
            };

            string url = "net.tcp://0.0.0.0:8100";

            serviceHost.AddServiceEndpoint(typeof(IChatServer), netTCPBinding, url);
            serviceHost.Open();

            Console.WriteLine("Chat server open.");
            Console.ReadLine();

            serviceHost.Close();
        }
    }
}