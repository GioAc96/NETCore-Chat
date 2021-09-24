using ChatShared.SDK.Payload;
using ProtoBuf;

namespace ChatShared.SDK.Messages
{
    [ProtoContract]
    public class ForwardText : IMessage
    {

        [ProtoMember(1)]
        public readonly TextPayload Text;

        [ProtoMember(2)]
        public readonly UserPayload Sender;

        public ForwardText(UserPayload sender, TextPayload text)
        {
            Sender = sender;
            Text = text;
        }
        
        private ForwardText() {}
        
    }
}