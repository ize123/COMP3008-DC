namespace ChatServer
{
    internal class Client
    {
        private string username;

        public Client(string username)
        {
            this.username = username;
        }

        public string Username
        {
            get { return username; }
        }
    }
}