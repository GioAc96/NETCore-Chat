using ChatShared.SDK.Messages.Payload;
using ProtoBuf;

namespace ChatShared.SDK.Messages
{
    [ProtoContract]
    public class ForwardTextMessage : IMessage
    {
        [ProtoMember(2)] public readonly UserPayload Sender;

        [ProtoMember(1)] public readonly TextPayload Text;

        public ForwardTextMessage(UserPayload sender, TextPayload text)
        {
            Sender = sender;
            Text = text;
        }

        private ForwardTextMessage()
        {
        }
    }
}