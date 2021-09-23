using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ChatShared;
using ChatShared.SDK.Messages;
using ChatShared.SDK.Payload;

namespace ChatClient
{
    public class Client
    {
        
        public async Task Start(IPAddress address, int port)
        {

            using var tcpClient = new TcpClient();

            await tcpClient.ConnectAsync(address, port);

            using var connection = new Connection(tcpClient.GetStream());

            var userName = PromptForName();
            
            var user = await ClientHandshake(connection, userName);
            
            Console.WriteLine($"Hello {user}");
            
            var token = new CancellationToken();

            while (! token.IsCancellationRequested)
            {

                var message = PromptForMessage();

                await connection.SendMessageAsync(new SendMessage(new ChatMessagePayload(message)));

            }

        }

        private async Task<UserPayload> ClientHandshake(Connection connection, string userName)
        {

            await connection.SendMessageAsync(new MyNameIsMessage(userName));

            var userId = (await connection.ReceiveMessageAsync<HelloMessage>()).UserId;

            return new UserPayload(userId, userName);

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