using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly HashSet<string> _userNameIndex = new HashSet<string>();
        private readonly LinkedList<ConnectedClient> _clients = new LinkedList<ConnectedClient>();
        private readonly LinkedList<ChatText> _texts = new LinkedList<ChatText>();

        public async Task Start(IPAddress address, int port)
        {
            
            var tcpListener = new TcpListener(address, port);

            tcpListener.Start();

            var token = new CancellationTokenSource().Token;

            while (! token.IsCancellationRequested)
            {

                var tcpClient = await tcpListener.AcceptTcpClientAsync();

                HandleNewTcpClient(tcpClient);

            }
            
        }
        
        private async void HandleNewTcpClient(TcpClient tcpClient)
        {

            using var connection = new Connection(tcpClient.GetStream());

            var client = new ConnectedClient(await ServerHandshake(connection), connection);
            
            _clients.AddFirst(client);

            await StartChatting(client);

        }

        private async Task<UserPayload> ServerHandshake(
            Connection connection
        )
        {

            var userName = await ReceiveValidUserName(connection);
            var userId = Guid.NewGuid();
            
            await connection.SendMessageAsync(new HelloMessage(userId));

            return new UserPayload(userId, userName);
            
        }

        private async Task<string> ReceiveValidUserName(Connection connection)
        {
            CancellationToken token = new CancellationTokenSource().Token;
            
            while (!token.IsCancellationRequested)
            {
                
                var userName = (await connection.ReceiveMessageAsync<MyNameIsMessage>()).UserName;

                if (IsUserNameTaken(userName))
                {
                    await connection.SendMessageAsync(new NameTakenMessage());
                }
                else
                {
                    RegisterUserName(userName);
                    return userName;
                }
                
            }

            throw new OperationCanceledException();
            
        }

        private bool IsUserNameTaken(string userName) => _userNameIndex.Contains(userName);
        private void RegisterUserName(string userName) => _userNameIndex.Add(userName);

        private async Task StartChatting(ConnectedClient client)
        {
            
            var token = new CancellationToken();

            while (!token.IsCancellationRequested)
            {

                var receivedTextPayload = (await client.Connection.ReceiveMessageAsync<SendTextMessage>()).Text;

                Console.Write($"{client.User} says: ");
                Console.WriteLine(receivedTextPayload.Body);
                
                ForwardChatText(new ChatText(client.User, receivedTextPayload));

            }
            
        }

        private void ForwardChatText(ChatText chatText)
        {
            
            foreach (var client in _clients.Where(client => ! client.User.Equals(chatText.Sender)))
            {
                
                SendMessageToConnectedClient(new ForwardTextMessage(chatText.Sender, chatText.Text), client);
                
            }

            _texts.AddFirst(chatText);

        }

        private async void SendMessageToConnectedClient<T>(T message, ConnectedClient connectedClient) where T : IMessage
        {

            try
            {
                await connectedClient.Connection.SendMessageAsync(message);
            }
            catch (Exception)
            {
                
                Console.WriteLine($"Could not deliver message to {connectedClient.User}. Removing from list of connected clients");
                _clients.Remove(connectedClient);

            }
            
        }
        
    }
    
}