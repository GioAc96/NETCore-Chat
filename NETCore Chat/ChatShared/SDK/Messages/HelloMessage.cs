using System;
using ProtoBuf;

namespace ChatShared.SDK.Messages
{
    
    [ProtoContract]
    public class HelloMessage : HandshakeResultMessage
    {

        [ProtoMember(1)]
        public readonly Guid UserId;

        public HelloMessage(Guid userId)
        {
            UserId = userId;
        }
        
        private HelloMessage() {}
        
    }
}