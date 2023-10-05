using System;
using System.Collections.Generic;

namespace ChatServer
{
    internal class ChatRoom
    {
        private string name;
        private List<Client> clients;
        private List<string> messages;
        private Dictionary<string, List<string>> privateConversations;

        public ChatRoom(string name)
        {
            this.name = name;

            clients = new List<Client>();
            messages = new List<string>();
            privateConversations = new Dictionary<string, List<string>>();
        }

        public string Name
        {
            get { return name; }
        }

        // clients
        public void AddClient(Client client)
        {
            clients.Add(client);
        }

        public string[] GetClientUsernames()
        {
            string[] clientUsernames = new string[clients.Count];

            for (int i = 0; i < clientUsernames.Length; i++)
            {
                clientUsernames[i] = clients[i].Username;
            }

            return clientUsernames;
        }

        public void RemoveClient(Client client)
        {
            clients.Remove(client);
        }

        // messages
        public void AddMessage(string message)
        {
            messages.Add(message);
        }

        public string GetMessagesAsString()
        {
            string messagesText = "";

            foreach (string message in messages)
            {
                messagesText += message + "\n";
            }

            return messagesText;
        }

        // private messages
        public void AddPrivateMessage(string client1Username, string client2Username, string message)
        {
            string key = CreateKey(client1Username, client2Username);

            if (!privateConversations.ContainsKey(key))
            {
                privateConversations.Add(key, new List<string>());
            }

            privateConversations[key].Add(message);
        }

        public string GetPrivateMessagesAsString(string client1Username, string client2Username)
        {
            string key = CreateKey(client1Username, client2Username);
            if (!privateConversations.ContainsKey(key)) { return null; }

            string privateMessagesText = "";

            foreach (string privateMessage in privateConversations[key])
            {
                privateMessagesText += privateMessage + "\n";
            }

            return privateMessagesText;
        }

        private string CreateKey(string client1Username, string client2Username)
        {
            string[] clientUsernames = { client1Username, client2Username };
            Array.Sort(clientUsernames, (leftClientUsername, rightClientUsername) => string.Compare(leftClientUsername, rightClientUsername));

            string key = "";

            foreach (string clientUsername in clientUsernames)
            {
                key += clientUsername + "-";
            }

            return key;
        }

        public void DeletePrivateConversationsByClientUsername(string clientUsername)
        {
            List<string> keys = new List<string>();

            foreach (string key in privateConversations.Keys)
            {
                string[] keyElements = key.Split('-');
                if (keyElements[0] != clientUsername && keyElements[2] != clientUsername) { continue; }
                keys.Add(key);
            }

            foreach (string key in keys)
            {
                privateConversations.Remove(key);
            }
        }
    }
}