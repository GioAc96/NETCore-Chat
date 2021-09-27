using System;
using System.Threading.Tasks;
using ChatShared.Util;

namespace ChatClient
{
    public interface IChatClientService : IDisposable
    {

        public static IChatClientService GetInstance()
        {
            return SettableSingleton<IChatClientService>.GetInstance();
        }
        
        public Task SendChatText(string body);
        
    }
}