using ChatShared;

namespace ChatServer
{
    public readonly struct ConnectedClient
    {
        
        public readonly User User;
        public readonly Connection Connection;

        public ConnectedClient(User user, Connection connection)
        {
            User = user;
            Connection = connection;
        }

    }
}