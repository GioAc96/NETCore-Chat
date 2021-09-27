using System;
using System.IO;
using System.Threading.Tasks;
using ChatShared.SDK.Messages;
using ProtoBuf;

namespace ChatShared
{
    public class Connection : IDisposable
    {
        private const PrefixStyle PrefixStyle = ProtoBuf.PrefixStyle.Base128;

        private readonly Stream _stream;

        public Connection(Stream stream)
        {
            _stream = stream;
        }

        public void Dispose()
        {
            _stream?.Dispose();
        }

        public async Task SendMessageAsync<T>(T message) where T : IMessage
        {
            await Task.Run(() => { SendMessage(message); });
        }

        public void SendMessage<T>(T message) where T : IMessage
        {
            Serializer.SerializeWithLengthPrefix(_stream, message, PrefixStyle);
            _stream.Flush();
        }

        public T ReceiveMessage<T>() where T : IMessage
        {
            return Serializer.DeserializeWithLengthPrefix<T>(_stream, PrefixStyle);
        }

        public async Task<T> ReceiveMessageAsync<T>() where T : IMessage
        {
            return await Task.Run(ReceiveMessage<T>);
        }
    }
}