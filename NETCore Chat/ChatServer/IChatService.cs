#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using ChatServer.Model;
using ChatShared.Util;

namespace ChatServer
{
    public interface IChatService
    {
        public static IChatService GetInstance()
        {
            return Singleton<ChatRepositoryAdapter>.GetInstance();
        }

        IEnumerable<User> GetConnectedUsers();

        User? GetUserByName(string userName);

        User? GetUserById(Guid userId);

        int GetMessagesCount(User user);

        IEnumerable<ChatText> GetAllTexts();

        private class ChatRepositoryAdapter : IChatService
        {
            private readonly IChatRepository _chatRepository = Singleton<Chat.ChatServer>.GetInstance();

            public IEnumerable<User> GetConnectedUsers()
            {
                return _chatRepository.GetConnectedUsers();
            }

            public User? GetUserByName(string userName)
            {
                return _chatRepository.GetConnectedUsers()
                    .FirstOrDefault(user => user.Name.Equals(userName));
            }

            public User? GetUserById(Guid userId)
            {
                return _chatRepository.GetConnectedUsers()
                    .FirstOrDefault(user => user.Id.Equals(userId));
            }

            public int GetMessagesCount(User user)
            {
                return _chatRepository.GetTexts().Count(text => text.Sender.Equals(user));
            }

            public IEnumerable<ChatText> GetAllTexts()
            {
                return _chatRepository.GetTexts();
            }
        }
    }
}