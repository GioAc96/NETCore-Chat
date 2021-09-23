using System;
using System.Net;
using System.Threading.Tasks;

namespace ChatServer {
    
    static class Program {
        
        private const int Port = 8000;
        private static readonly IPAddress Address = IPAddress.Any;

        static async Task Main(string[] args)
        {

            await new Server().Start(Address, Port);

        }
    }
}
