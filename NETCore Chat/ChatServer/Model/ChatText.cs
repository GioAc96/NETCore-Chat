using ChatShared.SDK.Messages.Payload;

namespace ChatServer.Model
{
    public class ChatText
    {
        public ChatText(User sender, string text)
        {
            Sender = sender;
            Text = text;
        }

        public string Text { get; }
        public User Sender { get; }

        public TextPayload ToPayload()
        {
            return new TextPayload(Text);
        }
    }
}