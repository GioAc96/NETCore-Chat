using ChatServer.Model;
using ChatShared;
using ChatShared.SDK.Payload;

namespace ChatServer.Chat
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