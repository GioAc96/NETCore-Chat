using ProtoBuf;

namespace ChatShared.SDK.Messages
{
    [ProtoContract]
    [ProtoInclude(101, typeof(MyNameIsMessage))]
    [ProtoInclude(102, typeof(SendMessage))]
    [ProtoInclude(103, typeof(ForwardMessage))]
    public interface IMessage
    {
        
    }
}