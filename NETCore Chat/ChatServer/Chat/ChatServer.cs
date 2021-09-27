using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ChatServer.Model;
using ChatShared;
using ChatShared.SDK.Messages;

namespace ChatServer.Chat
{
    public class ChatServer : IChatRepository, IDisposable
    {
        private readonly LinkedList<ConnectedClient> _clients = new LinkedList<ConnectedClient>();
        private readonly LinkedList<ChatText> _texts = new LinkedList<ChatText>();

        public IEnumerable<ChatText> GetTexts()
        {
            return _texts;
        }

        public IEnumerable<User> GetConnectedUsers()
        {
            return from client in _clients select client.User;
        }

        public void Dispose()
        {
            foreach (var client in _clients) client.Connection.Dispose();
        }

        public async void StartAsync(IPAddress address, int port, CancellationToken cancellationToken)
        {
            var tcpListener = new TcpListener(address, port);

            tcpListener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();

                HandleNewTcpClientAsync(tcpClient, cancellationToken);
            }
        }

        private async void HandleNewTcpClientAsync(TcpClient tcpClient, CancellationToken cancellationToken)
        {
            using var connection = new Connection(tcpClient.GetStream());

            User user;

            try
            {
                user = await ServerHandshakeAsync(connection, cancellationToken);
            }
            catch (Exception)
            {
                Console.WriteLine("Handshake failed.");
                return;
            }

            var client = new ConnectedClient(user, connection);

            ClientConnected(client);

            await StartForwardingIncomingMessagesAsync(client, cancellationToken);
        }

        private async Task<User> ServerHandshakeAsync(
            Connection connection,
            CancellationToken cancellationToken
        )
        {
            var userName = await ReceiveValidUserNameAsync(connection, cancellationToken);
            var userId = Guid.NewGuid();

            await connection.SendMessageAsync(new HelloMessage(userId));

            return new User(userId, userName);
        }

        private async Task<string> ReceiveValidUserNameAsync(Connection connection, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var userName = (await connection.ReceiveMessageAsync<MyNameIsMessage>()).UserName;

                if (IsUserNameTaken(userName))
                    await connection.SendMessageAsync(new NameTakenMessage());
                else
                    return userName;
            }

            throw new OperationCanceledException();
        }

        private bool IsUserNameTaken(string userName)
        {
            return _clients.Any(client => client.User.Name.Equals(userName));
        }

        private async Task StartForwardingIncomingMessagesAsync(
            ConnectedClient client,
            CancellationToken cancellationToken
        )
        {
            while (!cancellationToken.IsCancellationRequested)
                try
                {
                    var message = await client.Connection.ReceiveMessageAsync<SendTextMessage>();

                    Console.Write($"{client.User} says: ");
                    Console.WriteLine(message.Text.Body);

                    ForwardChatText(new ChatText(client.User, message.Text.Body));
                }
                catch (Exception)
                {
                    ClientDisconnected(client);
                    break;
                }
        }

        private void ForwardChatText(
            ChatText chatText
        )
        {
            foreach (var client in _clients.Where(client => !client.User.Equals(chatText.Sender)))
            {
                var message = new ForwardTextMessage(chatText.Sender.ToPayload(), chatText.ToPayload());
                SendMessageToConnectedClient(message, client);
            }

            _texts.AddFirst(chatText);
        }

        private async void SendMessageToConnectedClient<T>(T message, ConnectedClient client) where T : IMessage
        {
            try
            {
                await client.Connection.SendMessageAsync(message);
            }
            catch (Exception)
            {
                ClientDisconnected(client);
            }
        }

        private void ClientConnected(ConnectedClient client)
        {
            Console.WriteLine($"{client.User} connected");
            _clients.AddFirst(client);
        }

        private void ClientDisconnected(ConnectedClient client)
        {
            Console.WriteLine($"{client.User} disconnected.");
            _clients.Remove(client);
        }
    }
}