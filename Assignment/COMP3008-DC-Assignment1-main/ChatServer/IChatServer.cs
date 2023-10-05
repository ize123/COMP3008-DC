using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

namespace ChatServer
{
    [ServiceContract]
    public interface IChatServer
    {
        // clients
        [OperationContract]
        bool ClientWithUsernameExists(string clientUsername);

        [OperationContract]
        void CreateClient(string clientUsername);

        [OperationContract]
        string[] GetClientUsernamesAsArray(string chatRoomName);

        [OperationContract]
        void DeleteClient(string clientUsername);

        // chat rooms
        [OperationContract]
        bool ChatRoomWithNameExists(string chatRoomName);

        [OperationContract]
        void CreateChatRoom(string chatRoomName);

        [OperationContract]
        string[] GetChatRoomNamesAsArray();

        [OperationContract]
        string GetMessagesAsString(string chatRoomName);

        // interactions
        [OperationContract]
        void JoinChatRoom(string chatRoomName, string clientUsername);

        [OperationContract]
        void SendMessage(string chatRoomName, string message);

        [OperationContract]
        void SendPrivateMessage(string chatRoomName, string client1Username, string client2Username, string message);

        [OperationContract]
        void UploadFile(FileInfo fileInfo, String chatRoomName);

        [OperationContract]
        string GetPrivateMessagesAsString(string chatRoomName, string client1Username, string client2Username);

        [OperationContract]
        void LeaveChatRoom(string chatRoomName, string clientUsername);

        [OperationContract]
        List<string> GetChatRoomFiles(string chatRoomName);

        [OperationContract]
        MemoryStream DownloadFile(string filePath);
    }
}