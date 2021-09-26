#nullable enable
using System;
using System.Collections.Generic;
using ChatServer.Model;

namespace ChatServer
{
    public interface IChatService
    {

        IEnumerable<User> GetConnectedUsers();

        User? GetUserByName(string userName);
        
        User? GetUserById(Guid userId);

        int GetMessagesCount(User user);

    }
    
}