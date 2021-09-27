using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using ChatShared.SDK.Messages;
using ProtoBuf;

namespace ChatShared
{
    public class Connection : IDisposable
    {
        private const PrefixStyle PrefixStyle = ProtoBuf.PrefixStyle.Base128;

        private readonly TcpClient _tcpClient;

        public Connection(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
        }

        public async Task SendMessageAsync<T>(T message) where T : IMessage
        {
            await Task.Run(() => { SendMessage(message); });
        }

        public void SendMessage<T>(T message) where T : IMessage
        {
            Serializer.SerializeWithLengthPrefix(_tcpClient.GetStream(), message, PrefixStyle);
            _tcpClient.GetStream().Flush();
        }

        public T ReceiveMessage<T>() where T : IMessage
        {
            return Serializer.DeserializeWithLengthPrefix<T>(_tcpClient.GetStream(), PrefixStyle);
        }

        public async Task<T> ReceiveMessageAsync<T>() where T : IMessage
        {
            return await Task.Run(ReceiveMessage<T>);
        }

        public void Dispose()
        {
            _tcpClient?.Dispose();
        }
    }
}