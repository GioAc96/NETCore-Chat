using ProtoBuf;

namespace ChatShared.SDK.Messages
{
    
    [ProtoContract]
    public class HelloMessage : HandshakeResultMessage
    {

        [ProtoMember(1)]
        public readonly int UserId;

        public HelloMessage(int userId)
        {
            UserId = userId;
        }
        
        private HelloMessage() {}
        
    }
}