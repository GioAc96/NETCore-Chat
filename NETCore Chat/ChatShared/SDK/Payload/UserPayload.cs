using System;
using ProtoBuf;

namespace ChatShared.SDK.Payload
{
    [ProtoContract]
    public class UserPayload {

        [ProtoMember(1)]
        public readonly Guid Id;
        
        [ProtoMember(2)]
        public readonly string Name;

        public UserPayload(Guid id, string name)
        {
            
            Id = id;
            Name = name;
            
        }
        
        private UserPayload(){}

        public override string ToString()
        {

            return $"{Name}";

        }
    }
}