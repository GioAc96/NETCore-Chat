using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ChatShared;
using ChatShared.SDK.Messages;
using ChatShared.SDK.Messages.Payload;

namespace ChatClient.Chat
{
    public class ChatClient : IDisposable, IChatClientService
    {

        private Connection Connection { get; set; }
        
        public async Task StartAsync(IPAddress address, int port, CancellationToken cancellationToken)
        {

            using var tcpClient = new TcpClient();

            await tcpClient.ConnectAsync(address, port);

            Connection = new Connection(tcpClient.GetStream());
            
            var user = await ClientHandshakeAsync(Connection, cancellationToken);
            
            Console.WriteLine($"Hello {user}");
            
            await StartChattingAsync(Connection, cancellationToken);

        }

        private static async Task<UserPayload> ClientHandshakeAsync(Connection connection, CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {

                var userName = PromptForName();
                
                await connection.SendMessageAsync(new MyNameIsMessage(userName));
                
                var handshakeResult = await connection.ReceiveMessageAsync<HandshakeResultMessage>();

                if (handshakeResult is HelloMessage helloMessage)
                {

                    return new UserPayload(helloMessage.UserId, userName);

                } 
                
                if (handshakeResult is NameTakenMessage)
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

        
        private static async Task StartChattingAsync(Connection connection, CancellationToken cancellationToken)
        {
            
            StartReceivingMessages(connection, cancellationToken);
            await StartSendingMessages(connection, cancellationToken);

        }

        private static async Task StartSendingMessages(Connection connection, CancellationToken cancellationToken)
        {
            
            while (! cancellationToken.IsCancellationRequested)
            {

                Console.Write("Type your message: ");
                var chatTextBody = Console.ReadLine();
                Console.WriteLine();

                await SendChatText(connection, chatTextBody);

            }
            
        }

        private static async Task SendChatText(Connection connection, string body)
        {
            await connection.SendMessageAsync(new SendTextMessage(

                new TextPayload(body)

            ));
        }

        private static async void StartReceivingMessages(Connection connection, CancellationToken cancellationToken)
        {

            while (! cancellationToken.IsCancellationRequested)
            {

                var receiveMessage = await connection.ReceiveMessageAsync<ForwardTextMessage>();
                
                Console.WriteLine();
                Console.WriteLine($"{receiveMessage.Sender} says: {receiveMessage.Text.Body}");
                Console.Write("Type your message: ");
                
            }
            
        }
        
        public void Dispose()
        {
            Connection?.Dispose();
        }

        public async Task SendChatText(string body)
        {

            await SendChatText(Connection, body);

        }
    }
}