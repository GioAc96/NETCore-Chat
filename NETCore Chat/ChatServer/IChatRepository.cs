using System.Collections.Generic;
using ChatServer.Model;

namespace ChatServer
{
    public interface IChatRepository
    {
        IEnumerable<ChatText> GetTexts();
        IEnumerable<User> GetConnectedUsers();
    }
}