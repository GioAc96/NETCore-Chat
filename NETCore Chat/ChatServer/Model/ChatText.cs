using ChatShared.SDK.Messages.Payload;

namespace ChatServer.Model
{
    public class ChatText
    {

        public string Text {get;}
        public User Sender {get;}

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