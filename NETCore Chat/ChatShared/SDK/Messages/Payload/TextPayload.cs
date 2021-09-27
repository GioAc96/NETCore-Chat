using ProtoBuf;

namespace ChatShared.SDK.Messages.Payload
{
    [ProtoContract]
    public class TextPayload
    {
        [ProtoMember(1)] public readonly string Body;

        public TextPayload(string body)
        {
            Body = body;
        }

        private TextPayload()
        {
        }

        public override string ToString()
        {
            return Body;
        }
    }
}