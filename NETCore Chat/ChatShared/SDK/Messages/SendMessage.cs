using ChatShared.SDK.Payload;
using ProtoBuf;

namespace ChatShared.SDK.Messages
{
    
    [ProtoContract]
    public class SendMessage
    {

        [ProtoMember(1)]
        public readonly ChatMessagePayload ChatMessage;
        
        public SendMessage(ChatMessagePayload chatMessage)
        {
            ChatMessage = chatMessage;
        }
        
        private SendMessage(){}
        
    }
}