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

        private readonly HashSet<string> _userNameIndex = new HashSet<string>();
        private readonly LinkedList<ConnectedClient> _clients = new LinkedList<ConnectedClient>();
        private readonly Queue<ChatText> _outgoingTexts = new Queue<ChatText>();
        private readonly LinkedList<ChatText> _sentTexts = new LinkedList<ChatText>();
        private Task _forwardingMessagesTask = Task.CompletedTask;

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
        
        private async Task HandleNewTcpClient(TcpClient tcpClient)
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

                var receivedTextPayload = (await client.Connection.ReceiveMessageAsync<SendText>()).Text;

                Console.Write($"{client.User} says: ");
                Console.WriteLine(receivedTextPayload.Body);
                
                ForwardText(new ChatText(client.User, receivedTextPayload));

            }
            
        }

        private void ForwardText(ChatText chatText)
        {

            _outgoingTexts.Enqueue(chatText);

            if (_forwardingMessagesTask.IsCompleted)
            {

                _forwardingMessagesTask = DeliverOutgoingTexts();

            }
            
        }

        private async Task DeliverOutgoingTexts()
        {

            while(_outgoingTexts.Count > 0)
            {

                var outgoingText = _outgoingTexts.Dequeue();
                
                var message = new ForwardText(outgoingText.Sender, outgoingText.Text);
                
                foreach (var clientNode in _clients)
                {
                    
                    if (clientNode.User.Equals(outgoingText.Sender))
                    {

                        clientNode.Connection.SendMessageAsync(message);

                    }
                    
                }
                
            }
            
        }

    }
    
}