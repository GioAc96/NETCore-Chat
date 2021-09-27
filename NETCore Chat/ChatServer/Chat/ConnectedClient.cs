using System;
using ChatServer.Model;
using ChatShared;
using ChatShared.SDK.Messages.Payload;

namespace ChatServer.Chat
{
    public readonly struct ConnectedClient : IDisposable
    {
        
        public readonly User User;
        public readonly Connection Connection;

        public ConnectedClient(User user, Connection connection)
        {
            User = user;
            Connection = connection;
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
        
    }
    
}