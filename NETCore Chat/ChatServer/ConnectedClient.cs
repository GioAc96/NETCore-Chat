using ChatShared;
using ChatShared.SDK.Payload;

namespace ChatServer
{
    public readonly struct ConnectedClient
    {
        
        public readonly UserPayload User;
        public readonly Connection Connection;

        public ConnectedClient(UserPayload user, Connection connection)
        {
            User = user;
            Connection = connection;
        }

    }
    
}