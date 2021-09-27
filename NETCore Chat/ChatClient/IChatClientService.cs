using System.Threading.Tasks;
using ChatShared.Util;

namespace ChatClient
{
    public interface IChatClientService
    {
        public static IChatClientService GetInstance()
        {
            return Singleton<Chat.ChatClient>.GetInstance();
        }

        public Task SendChatText(string body);
    }
}