
using ChatShared.SDK.Payload;

namespace ChatServer.Chat
{
    public class ChatText
    {

        public readonly TextPayload Text;

        public readonly UserPayload Sender;

        public ChatText(UserPayload sender, TextPayload text)
        {
            
            Sender = sender;
            Text = text;
            
        }
        
    }
    
}