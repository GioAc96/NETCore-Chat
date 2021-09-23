using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ChatShared;

namespace ChatClient
{
    public class Client
    {
        
        public async Task Start(IPAddress address, int port)
        {

            using var tcpClient = new TcpClient();

            await tcpClient.ConnectAsync(address, port);

            using var connection = new Connection(tcpClient.GetStream());

            var name = PromptForName();
            
            await connection.SendDataAsync(name);

            var id = int.Parse(await connection.ReceiveDataAsync());

            User user = new User(id, name);
            
            Console.WriteLine($"Hello {user}");
            
            var token = new CancellationToken();

            while (! token.IsCancellationRequested)
            {

                var message = PromptForMessage();

                await connection.SendDataAsync(message);

            }

        }
        
        private static string PromptForName()
        {

            Console.Write("Insert your name: ");
            return Console.ReadLine();
            
        }

        private static string PromptForMessage()
        {
            Console.WriteLine("Type a message: ");
            return Console.ReadLine();

        }
        
    }
}