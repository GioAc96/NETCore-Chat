using ChatShared.SDK.Payload;
using ProtoBuf;

namespace ChatShared.SDK.Messages
{
    [ProtoContract]
    public class ForwardMessage : IMessage
    {

        [ProtoMember(1)]
        public readonly ChatMessagePayload ChatMessage;

        [ProtoMember(2)]
        public readonly UserPayload Sender;

        public ForwardMessage(UserPayload sender, ChatMessagePayload chatMessage)
        {
            Sender = sender;
            ChatMessage = chatMessage;
        }
        
        private ForwardMessage() {}
        
    }
}