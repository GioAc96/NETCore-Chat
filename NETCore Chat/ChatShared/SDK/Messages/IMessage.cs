using ProtoBuf;

namespace ChatShared.SDK.Messages
{
    [ProtoContract]
    [ProtoInclude(101, typeof(MyNameIsMessage))]
    [ProtoInclude(102, typeof(SendTextMessage))]
    [ProtoInclude(103, typeof(ForwardTextMessage))]
    public interface IMessage
    {
        
    }
}