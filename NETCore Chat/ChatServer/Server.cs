using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ChatShared;

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

            var userName = await connection.ReceiveDataAsync();
            var userId = _idProvider.NewId();

            var user = new User(userId, userName);

            await connection.SendDataAsync(userId.ToString());
            
            _clients.AddFirst(new ConnectedClient(user, connection));
            
            var token = new CancellationToken();

            while (!token.IsCancellationRequested)
            {

                var messageBody = await connection.ReceiveDataAsync();

                Console.WriteLine($"{user} says: {messageBody}");

            }

        }
        
    }
    
}