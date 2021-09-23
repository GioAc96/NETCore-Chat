using System;
using System.IO;
using System.Threading.Tasks;

namespace ChatShared
{
    public class Connection : IDisposable
    {

        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        
        public Connection(Stream stream)
        {

            _reader = new StreamReader(stream);
            _writer = new StreamWriter(stream);

        }

        public async Task SendDataAsync(string data)
        {
            await _writer.WriteLineAsync(data);
            await _writer.FlushAsync();
        }

        public async Task<string> ReceiveDataAsync()
        {
            return await _reader.ReadLineAsync();
        }
        
        
        public void SendData(string data)
        {
            
            SendDataAsync(data).GetAwaiter().GetResult();
            
        }

        public string ReceiveData()
        {
            return ReceiveDataAsync().GetAwaiter().GetResult();
        }
        
        public void Dispose()
        {
            
            _reader.Dispose();
            _writer.Dispose();
            
        }
        
    }
    
}