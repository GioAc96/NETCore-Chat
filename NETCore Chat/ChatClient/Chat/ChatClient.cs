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
    public class ChatClient 
    {

        public sealed class ChatClientService : IChatClientService, IDisposable
        {

            private readonly Connection _connection;

            public ChatClientService(Connection connection)
            {
                _connection = connection;
            }


            public async Task SendChatText(string body)
            {
                
                await _connection.SendMessageAsync(new SendTextMessage(new TextPayload(body)));
                
            }

            public void Dispose()
            {
                _connection?.Dispose();
            }
        }
   
        public async Task<IChatClientService> StartAsync(IPAddress address, int port, CancellationToken cancellationToken)
        {
            var tcpClient = new TcpClient();

            await tcpClient.ConnectAsync(address, port);

            var connection = new Connection(tcpClient);

            var user = await ClientHandshakeAsync(connection, cancellationToken);

            Console.WriteLine($"Hello {user}");

            Task.Run(() => StartChatting(connection, cancellationToken));

            return new ChatClientService(connection);

        }

        private static async Task<UserPayload> ClientHandshakeAsync(Connection connection,
            CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var userName = PromptForName();

                await connection.SendMessageAsync(new MyNameIsMessage(userName));

                var handshakeResult = await connection.ReceiveMessageAsync<HandshakeResultMessage>();

                if (handshakeResult is HelloMessage helloMessage) return new UserPayload(helloMessage.UserId, userName);

                if (handshakeResult is NameTakenMessage) Console.WriteLine("Name is already taken.");
            }

            throw new OperationCanceledException();
        }

        private static string PromptForName()
        {
            Console.Write("Insert your name: ");
            return Console.ReadLine();
        }


        private static void StartChatting(Connection connection, CancellationToken cancellationToken)
        {
            StartReceivingMessagesAsync(connection, cancellationToken);
            StartSendingMessagesAsync(connection, cancellationToken);
        }

        private static async void StartSendingMessagesAsync(Connection connection, CancellationToken cancellationToken)
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

        private static async void StartReceivingMessagesAsync(Connection connection, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var receiveMessage = await connection.ReceiveMessageAsync<ForwardTextMessage>();

                Console.WriteLine();
                Console.WriteLine($"{receiveMessage.Sender} says: {receiveMessage.Text.Body}");
                Console.Write("Type your message: ");
            }
        }
    }
}