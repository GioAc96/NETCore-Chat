﻿using System;
using ProtoBuf;

namespace ChatShared.SDK.Payload
{
    [ProtoContract]
    public class UserPayload : IEquatable<UserPayload> {

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

        public bool Equals(UserPayload other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserPayload) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    
}