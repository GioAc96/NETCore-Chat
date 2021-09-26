using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ChatShared;
using ChatShared.SDK.Messages;
using ChatShared.SDK.Payload;

namespace ChatClient.Chat
{
    public class ChatClient
    {
        
        public async Task Start(IPAddress address, int port)
        {

            using var tcpClient = new TcpClient();

            await tcpClient.ConnectAsync(address, port);

            using var connection = new Connection(tcpClient.GetStream());
            
            var user = await ClientHandshake(connection);
            
            Console.WriteLine($"Hello {user}");

            await StartChatting(connection);

        }

        private async Task<UserPayload> ClientHandshake(Connection connection)
        {

            var token = new CancellationToken();

            while (!token.IsCancellationRequested)
            {

                var userName = PromptForName();
                
                await connection.SendMessageAsync(new MyNameIsMessage(userName));
                
                var handshakeResult = await connection.ReceiveMessageAsync<HandshakeResultMessage>();

                if (handshakeResult is HelloMessage helloMessage)
                {

                    return new UserPayload(helloMessage.UserId, userName);

                } else if (handshakeResult is NameTakenMessage)
                {
                    
                    Console.WriteLine("Name is already taken.");
                    
                }
                
            }

            throw new OperationCanceledException();

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

        private async Task StartChatting(Connection connection)
        {
            
            StartReceivingMessages(connection);
            await StartSendingMessages(connection);

        }

        private async Task StartSendingMessages(Connection connection)
        {

            while (true)
            {

                Console.Write("Type your message: ");
                var messageBody = Console.ReadLine();
                Console.WriteLine();

                await connection.SendMessageAsync(new SendTextMessage(

                    new TextPayload(messageBody)

                ));

            }
            
        }

        private async Task StartReceivingMessages(Connection connection)
        {

            while (true)
            {

                var receiveMessage = await connection.ReceiveMessageAsync<ForwardTextMessage>();
                
                Console.WriteLine();
                Console.WriteLine($"{receiveMessage.Sender} says: {receiveMessage.Text.Body}");
                Console.Write("Type your message: ");
                
            }
            
        }
        
    }
}