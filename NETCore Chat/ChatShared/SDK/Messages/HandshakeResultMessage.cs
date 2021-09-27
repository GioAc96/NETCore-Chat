using ProtoBuf;

namespace ChatShared.SDK.Messages
{
    [ProtoContract]
    [ProtoInclude(201, typeof(HelloMessage))]
    [ProtoInclude(202, typeof(NameTakenMessage))]
    public class HandshakeResultMessage : IMessage
    {
    }
}