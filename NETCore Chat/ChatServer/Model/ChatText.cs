
using ChatShared.SDK.Payload;

namespace ChatServer.Model
{
    public class ChatText
    {

        public readonly string Text;
        public readonly User Sender;

        public ChatText(User sender, string text)
        {
            Sender = sender;
            Text = text;
        }

        public TextPayload ToPayload()
        {
            return new TextPayload(Text);
        }
        
    }
    
}