using ChatShared.SDK.Payload;
using ProtoBuf;

namespace ChatShared.SDK.Messages
{
    
    [ProtoContract]
    public class SendText : IMessage
    {

        [ProtoMember(1)]
        public readonly TextPayload Text;
        
        public SendText(TextPayload text)
        {
            Text = text;
        }
        
        private SendText(){}
        
    }
}