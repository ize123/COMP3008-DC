using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

namespace ChatServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, InstanceContextMode = InstanceContextMode.Single)]
    internal class ChatServer : IChatServer
    {
        private Dictionary<string, ChatRoom> chatRooms;
        private Dictionary<string, Client> clients;
        private Dictionary<ChatRoom, List<string>> storedFiles;
        private DirectoryInfo filesDirectoryInfo;

        public ChatServer()
        {
            chatRooms = new Dictionary<string, ChatRoom>();
            clients = new Dictionary<string, Client>();
            storedFiles = new Dictionary<ChatRoom, List<string>>();

            // Instantiate the file directory 
            int projectDirectoryIndex = Environment.CurrentDirectory.IndexOf("bin");
            string projectPath = Environment.CurrentDirectory.Substring(0, projectDirectoryIndex);

            string filesDirectory = Path.Combine(projectPath, "Files");
            filesDirectoryInfo = new DirectoryInfo(filesDirectory);

            // Create a new directory is none exists
            if (!filesDirectoryInfo.Exists)
            {
                filesDirectoryInfo.Create();
            }

            //Clear the directory when the server starts
            foreach (FileInfo file in filesDirectoryInfo.GetFiles())
            {
                file.Delete();
            }
        }

        // clients
        public bool ClientWithUsernameExists(string clientUsername)
        {
            return clients.ContainsKey(clientUsername);
        }

        public void CreateClient(string clientUsername)
        {
            Client client = new Client(clientUsername);
            clients.Add(client.Username, client);
        }

        public string[] GetClientUsernamesAsArray(string chatRoomName)
        {
            ChatRoom chatRoom = chatRooms[chatRoomName];
            return chatRoom.GetClientUsernames();
        }

        public void DeleteClient(string clientUsername)
        {
            Client client = clients[clientUsername];

            foreach (ChatRoom chatRoom in chatRooms.Values)
            {
                chatRoom.DeletePrivateConversationsByClientUsername(clientUsername);
            }

            clients.Remove(clientUsername);
        }

        // chat rooms
        public bool ChatRoomWithNameExists(string chatRoomName)
        {
            return chatRooms.ContainsKey(chatRoomName);
        }

        public void CreateChatRoom(string chatRoomName)
        {
            ChatRoom chatRoom = new ChatRoom(chatRoomName);
            chatRooms.Add(chatRoom.Name, chatRoom);
        }

        public string[] GetChatRoomNamesAsArray()
        {
            Dictionary<string, ChatRoom>.KeyCollection chatRoomKeys = chatRooms.Keys;
            string[] chatRoomNames = new string[chatRoomKeys.Count];
            chatRoomKeys.CopyTo(chatRoomNames, 0);
            return chatRoomNames;
        }

        public string GetMessagesAsString(string chatRoomName)
        {
            ChatRoom chatRoom = chatRooms[chatRoomName];
            return chatRoom.GetMessagesAsString();
        }

        // interactions
        public void JoinChatRoom(string chatRoomName, string clientUsername)
        {
            ChatRoom chatRoom = chatRooms[chatRoomName];
            Client client = clients[clientUsername];
            chatRoom.AddClient(client);
        }

        public void SendMessage(string chatRoomName, string message)
        {
            ChatRoom chatRoom = chatRooms[chatRoomName];
            chatRoom.AddMessage(message);
        }

        public void SendPrivateMessage(string chatRoomName, string client1Username, string client2Username, string message)
        {
            ChatRoom chatRoom = chatRooms[chatRoomName];
            chatRoom.AddPrivateMessage(client1Username, client2Username, message);
        }

        public string GetPrivateMessagesAsString(string chatRoomName, string client1Username, string client2Username)
        {
            ChatRoom chatRoom = chatRooms[chatRoomName];
            return chatRoom.GetPrivateMessagesAsString(client1Username, client2Username);
        }

        public void LeaveChatRoom(string chatRoomName, string clientUsername)
        {
            ChatRoom chatRoom = chatRooms[chatRoomName];
            Client client = clients[clientUsername];
            chatRoom.RemoveClient(client);
        }

        public void UploadFile(FileInfo fileInfo, string chatRoomName)
        {
            // add the file
            string filename = CreateFilename(fileInfo);
            string filePath = Path.Combine(filesDirectoryInfo.FullName, filename);

            // add to dictionary of files
            ChatRoom chatRoom = chatRooms[chatRoomName];

            if (!storedFiles.ContainsKey(chatRoom))
            {
                storedFiles.Add(chatRoom, new List<string>());
            }

            storedFiles[chatRoom].Add(filePath);
            fileInfo.CopyTo(filePath);
        }

        private string CreateFilename(FileInfo fileInfo)
        {
            string filename = fileInfo.Name;
            string filenameName = filename.Substring(0, filename.Length - 4);
            string filenameExtension = filename.Substring(filename.Length - 4, 4);

            int i = 0;
            string newFilename;
            string filePath;
            bool fileExists;

            do
            {
                newFilename = filenameName + i + filenameExtension;
                filePath = Path.Combine(filesDirectoryInfo.FullName, newFilename);
                fileExists = File.Exists(filePath);
                i++;
            } while (fileExists);

            return newFilename;
        }

        public MemoryStream DownloadFile(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                MemoryStream memoryStream = new MemoryStream(fileBytes);
                return memoryStream;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error while downloading file:", e);
            }
        }

        public List<string> GetChatRoomFiles(string chatRoomName)
        {
            ChatRoom chatRoom = chatRooms[chatRoomName];

            if (!storedFiles.ContainsKey(chatRoom))
            {
                storedFiles.Add(chatRoom, new List<string>());
            }

            return storedFiles[chatRoom];
        }
    }
}