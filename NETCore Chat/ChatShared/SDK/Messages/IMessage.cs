using ProtoBuf;

namespace ChatShared.SDK.Messages
{
    [ProtoContract]
    [ProtoInclude(101, typeof(MyNameIsMessage))]
    [ProtoInclude(102, typeof(SendText))]
    [ProtoInclude(103, typeof(ForwardText))]
    public interface IMessage
    {
        
    }
}