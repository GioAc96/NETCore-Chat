using System;
using System.IO;
using System.Threading.Tasks;
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

        public async Task SendMessageAsync<T>(T message)
        {

            await Task.Run(() =>
            {
                
                SendMessage(message);

            });
            
        }
        
        public void SendMessage<T>(T message)
        {

            Serializer.SerializeWithLengthPrefix(_stream, message, PrefixStyle);
            _stream.Flush();
            
        }

        public T ReceiveMessage<T>()
        {

            return Serializer.DeserializeWithLengthPrefix<T>(_stream, PrefixStyle);
            
        }

        public async Task<T> ReceiveMessageAsync<T>()
        {

            return await Task.Run(ReceiveMessage<T>);

        }
        
        public void Dispose()
        {
            
            _stream.Dispose();
            
        }
        
    }
    
}