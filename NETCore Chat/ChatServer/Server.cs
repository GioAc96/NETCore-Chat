using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ChatShared;
using ChatShared.SDK.Messages;
using ChatShared.SDK.Payload;

namespace ChatServer
{
    public class Server
    {
        
        private readonly LinkedList<ConnectedClient> _clients = new LinkedList<ConnectedClient>();
        private readonly IUserIdProvider _idProvider = new IncrementingIntegerUserIdProvider();

        public async Task Start(IPAddress address, int port)
        {
            
            var tcpListener = new TcpListener(address, port);

            tcpListener.Start();

            var token = new CancellationToken();

            while (! token.IsCancellationRequested)
            {

                var tcpClient = await tcpListener.AcceptTcpClientAsync();

                HandleNewTcpClient(tcpClient);

            }
            
        }
        
        private async Task HandleNewTcpClient(TcpClient tcpClient)
        {

            using var connection = new Connection(tcpClient.GetStream());

            var user = await ServerHandshake(connection);
            
            _clients.AddFirst(new ConnectedClient(user, connection));

            var token = new CancellationToken();

            while (!token.IsCancellationRequested)
            {

                var receivedChatMessage = (await connection.ReceiveMessageAsync<SendMessage>()).ChatMessage;

                Console.Write($"{user} says: ");
                Console.WriteLine(receivedChatMessage.Body);
                
                foreach (ConnectedClient client in _clients)
                {

                    if (client.User.Id != user.Id)
                    {

                        await client.Connection.SendMessageAsync(new ForwardMessage(user, receivedChatMessage));

                    }
                    
                }

            }

        }

        private async Task<UserPayload> ServerHandshake(Connection connection)
        {

            var userName = (await connection.ReceiveMessageAsync<MyNameIsMessage>()).UserName;

            var userId = _idProvider.NewId();

            await connection.SendMessageAsync(new HelloMessage(userId));

            return new UserPayload(userId, userName);

        }
        
    }
    
}