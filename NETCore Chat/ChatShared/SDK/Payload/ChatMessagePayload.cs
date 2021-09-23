using ProtoBuf;

namespace ChatShared.SDK.Payload
{
    [ProtoContract]
    public class ChatMessagePayload {

        [ProtoMember(1)]
        public readonly string Body;

        public ChatMessagePayload(string body)
        {
            Body = body;
        }
        
        private ChatMessagePayload() {}

        public override string ToString()
        {
            return Body;
        }
    }
}