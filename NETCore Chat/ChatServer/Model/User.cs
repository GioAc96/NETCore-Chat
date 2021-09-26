using System;
using ChatShared.SDK.Payload;

namespace ChatServer.Model
{
    public class User : IEquatable<User>
    {

        public Guid Id { get; }
        public string Name { get; }

        public User(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        
        public UserPayload ToPayload()
        {
            return new UserPayload(Id, Name);
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(User other)
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
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    
}