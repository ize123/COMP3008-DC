using ChatServer;
using Microsoft.Win32;
using System;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using Path = System.IO.Path;

namespace Client
{
    public partial class MainWindow : Window
    {
        private IChatServer chatServer;

        // client state
        private bool isLoggedIn;
        private string thisClientUsername;
        private string chatRoomName;
        private string otherClientUsername;
        private string desktopPath;

        // threading
        private readonly TimeSpan DELAY = new TimeSpan(0, 0, 5); // polling delay in seconds
        private CancellationToken token = (new CancellationTokenSource()).Token; // used to close the polling thread

        public MainWindow()
        {
            InitializeComponent();
            isLoggedIn = false;
            desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            CreateChannel();
            CreateListeners(); // listeners persist for life of application -> could swap to life of login?
        }

        private void CreateChannel()
        {
            NetTcpBinding netTCPBinding = new NetTcpBinding
            {
                MaxReceivedMessageSize = 2_000_000
            };

            string url = "net.tcp://localhost:8100";
            ChannelFactory<IChatServer> channelFactory = new ChannelFactory<IChatServer>(netTCPBinding, url);
            chatServer = channelFactory.CreateChannel();
        }

        // listener loop
        private void CreateListeners()
        {
            var updateListener = Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested) // as
                {
                    // need to access the UI from a different thread using dispatcher

                    Dispatcher.Invoke(() =>
                    {
                        Console.WriteLine("polled server");

                        UpdateChatRoomsListBox();
                        UpdateClientsListBox();
                        UpdateMessagesTextBlock();
                        UpdateFilesBox();

                        ChatRoomsListBox.SelectedItem = chatRoomName;
                        ClientsListBox.SelectedItem = otherClientUsername;
                    });

                    Thread.Sleep(DELAY);
                }
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void LoginLogoutButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isLoggedIn)
            {
                string clientUsername = ClientUsernameTextBox.Text;

                if (!CanLoginWithUsername(clientUsername))
                {
                    ClientUsernameTextBox.Text = "Client Username"; // resets user input placeholder
                    return;
                }

                Login(clientUsername);
            }
            else
            {
                Logout();
            }
        }

        private void CreateChatRoomButtonClick(object sender, RoutedEventArgs e)
        {
            string newChatRoomName = ChatRoomNameTextBox.Text;
            ChatRoomNameTextBox.Text = "Chat Room Name"; // resets user input placeholder

            if (chatServer.ChatRoomWithNameExists(newChatRoomName))
            {
                InformationTextBlock.Text = "ERROR: a chat room with the name '" + newChatRoomName + "' already exists.";
                return;
            }

            chatServer.CreateChatRoom(newChatRoomName);
            InformationTextBlock.Text = "SUCCESS: created a chat room with the name '" + newChatRoomName + "'.";
            UpdateChatRoomsListBox();
        }

        private void ChatRoomsListBoxPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            object selectedChatRoom = ChatRoomsListBox.SelectedItem;
            if (selectedChatRoom == null) { return; } // prevents response to clicking in empty area

            // update server
            LeaveCurrentChatRoom();
            chatRoomName = ChatRoomsListBox.SelectedItem.ToString(); // updates client state
            chatServer.JoinChatRoom(chatRoomName, thisClientUsername);

            // update clients
            ClientsListBox.Items.Clear();
            UpdateClientsListBox();
            otherClientUsername = null;
            ClientsListBox.SelectedItem = otherClientUsername;

            // update messages
            UpdateMessagesTextBlock();
            SetSendMessageIsEnabled(true);

            // update files
            UpdateFilesBox();
        }

        private void ClientsListBoxPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            object selectedClient = ClientsListBox.SelectedItem;
            if (selectedClient == null) { return; } // prevents response to clicking in empty area

            otherClientUsername = ClientsListBox.SelectedItem.ToString(); // updates client state
            UpdateMessagesTextBlock();
        }

        private void SendMessageButtonClick(object sender, RoutedEventArgs e)
        {
            string message = thisClientUsername + ": " + MessageTextBox.Text;

            if (otherClientUsername == null) // selected chat room
            {
                chatServer.SendMessage(chatRoomName, message);
                InformationTextBlock.Text = "SUCCESS: sent a message to the chat room with the name '" + chatRoomName + "'.";
            }
            else // selected client
            {
                chatServer.SendPrivateMessage(chatRoomName, thisClientUsername, otherClientUsername, message);
                InformationTextBlock.Text = "SUCCESS: sent a message to the client with the username '" + otherClientUsername + "'.";
            }

            MessageTextBox.Text = "Message"; // resets user input placeholder
        }

        private void UploadFileButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "(*.txt;*.png)|*.txt;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName;
                FileInfo fileInfo = new FileInfo(filename);

                ChatRoomsListBox.SelectedItem = chatRoomName;

                var fileUploadAsync = Task.Factory.StartNew(() =>
                {
                    chatServer.UploadFile(fileInfo, chatRoomName);
                });

                UpdateFilesBox();
            }
        }

        // helpers
        private bool CanLoginWithUsername(string clientUsername)
        {
            if (clientUsername.Contains("-"))
            {
                InformationTextBlock.Text = "ERROR: the username '" + clientUsername + "' contains a '-'.";
                return false;
            }

            if (chatServer.ClientWithUsernameExists(clientUsername))
            {
                InformationTextBlock.Text = "ERROR: a client with the username '" + clientUsername + "' already exists.";
                return false;
            }

            return true;
        }

        private void Login(string clientUsername)
        {
            // update server
            chatServer.CreateClient(clientUsername);

            // update client state
            isLoggedIn = true;
            thisClientUsername = clientUsername;

            // update toolbar
            InformationTextBlock.Text = "SUCCESS: logged in with the username '" + thisClientUsername + "'.";
            ClientUsernameTextBox.IsEnabled = false;
            LoginLogoutButton.Content = "Logout";

            // update chat rooms
            SetCreateChatRoomIsEnabled(true);
            UpdateChatRoomsListBox();
        }

        private void UpdateChatRoomsListBox()
        {
            ChatRoomsListBox.Items.Clear();

            if (thisClientUsername == null) { return; } // prevents chat rooms being loaded by the update thread when a user is logged out

            string[] chatRoomNames = chatServer.GetChatRoomNamesAsArray();

            foreach (string chatRoomName in chatRoomNames)
            {
                ChatRoomsListBox.Items.Add(chatRoomName);
            }
        }

        private void UpdateClientsListBox()
        {
            ClientsListBox.Items.Clear();

            if (chatRoomName == null) { return; }

            string[] clientUsernames = chatServer.GetClientUsernamesAsArray(chatRoomName);

            foreach (string clientUsername in clientUsernames)
            {
                ClientsListBox.Items.Add(clientUsername);
            }
        }

        private void UpdateMessagesTextBlock()
        {
            MessagesTextBlock.Text = "";

            if (chatRoomName == null) { return; }

            if (otherClientUsername == null) // selected chat room
            {
                MessagesTextBlock.Text = chatServer.GetMessagesAsString(chatRoomName);
            }
            else // selected client
            {
                MessagesTextBlock.Text = chatServer.GetPrivateMessagesAsString(chatRoomName, thisClientUsername, otherClientUsername);
            }
        }

        private void LeaveCurrentChatRoom()
        {
            if (chatRoomName == null) { return; }
            chatServer.LeaveChatRoom(chatRoomName, thisClientUsername);
        }

        private void Logout()
        {
            // update server
            LeaveCurrentChatRoom();
            chatServer.DeleteClient(thisClientUsername);
            InformationTextBlock.Text = "SUCCESS: logged out from the username '" + thisClientUsername + "'.";

            // update client state
            isLoggedIn = false;
            thisClientUsername = null;
            chatRoomName = null;
            otherClientUsername = null;

            // update toolbar
            ClientUsernameTextBox.IsEnabled = true;
            ClientUsernameTextBox.Text = "Client Username";
            LoginLogoutButton.Content = "Login";

            // update chat rooms
            SetCreateChatRoomIsEnabled(false);
            ChatRoomsListBox.Items.Clear();

            // update clients
            ClientsListBox.Items.Clear();

            // update messages
            MessagesTextBlock.Text = "";
            SetSendMessageIsEnabled(false);

            // update files
            FilesLinkBox.Text = "";
        }

        private void SetCreateChatRoomIsEnabled(bool isEnabled)
        {
            ChatRoomNameTextBox.IsEnabled = isEnabled;
            CreateChatRoomButton.IsEnabled = isEnabled;
        }

        private void SetSendMessageIsEnabled(bool isEnabled)
        {
            MessageTextBox.IsEnabled = isEnabled;
            SendMessageButton.IsEnabled = isEnabled;
            UploadFileButton.IsEnabled = isEnabled;
        }

        private void ClearOnFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            tb.Clear();
        }

        // Will populate the box at the bottom where all the files in the chatroom will appear
        private void UpdateFilesBox()
        {
            FilesLinkBox.Inlines.Clear();

            if (chatRoomName == null) { return; }

            Dispatcher.Invoke(() =>
            {
                var list = chatServer.GetChatRoomFiles(chatRoomName);

                if (list == null) { return; }

                foreach (var path in list)
                {
                    string fileName = Path.GetFileName(path);

                    Hyperlink hl = new Hyperlink();
                    hl.Inlines.Add(fileName);
                    hl.Tag = path;
                    hl.Click += Hyperlink_Click;
                    FilesLinkBox.Inlines.Add(hl);
                    FilesLinkBox.Inlines.Add("\n");
                }
            });
        }

        // Event to handle the hyper_link click 
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            // Ensure that the Tag property is set for the clicked hyperlink
            if (((Hyperlink)sender).Tag != null)
            {
                string filePath = ((Hyperlink)sender).Tag.ToString();

                // Getting the text out of the hyperlink
                string fileName = new TextRange(((Hyperlink)sender).Inlines.FirstInline.ContentStart, ((Hyperlink)sender).Inlines.LastInline.ContentEnd).Text;

                // Now you have the file path and can use it for further processing
                SaveFileToComputer(chatServer.DownloadFile(filePath), fileName);
            }
            else
            {
                // Handle the case where Tag is not set for the hyperlink
                MessageBox.Show("File path not set for this hyperlink.");
            }
        }

        // Takes in a memory stream and write the file to the desktop
        private void SaveFileToComputer(MemoryStream stream, string fileName)
        {
            string localPath = Path.Combine(desktopPath, fileName);

            try
            {
                using (FileStream fileStream = File.Create(localPath))
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fileStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                InformationTextBlock.Text = "ERROR: unauthorised access to the local path '" + localPath + "'.";
            }
        }
    }
}