using ChatShared.SDK.Messages.Payload;
using ProtoBuf;

namespace ChatShared.SDK.Messages
{
    
    [ProtoContract]
    public class SendTextMessage : IMessage
    {

        [ProtoMember(1)]
        public readonly TextPayload Text;
        
        public SendTextMessage(TextPayload text)
        {
            Text = text;
        }
        
        private SendTextMessage(){}
        
    }
}