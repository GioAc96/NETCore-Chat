using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ChatServer.Rest;
using ChatShared.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ChatServer
{
    public static class Program
    {
        private const int ChatPort = 8000;
        private static readonly IPAddress ChatIpAddress = IPAddress.Any;


        public static async Task Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            using var chatServer = Singleton<Chat.ChatServer>.GetInstance();

            chatServer.StartAsync(ChatIpAddress, ChatPort, cancellationTokenSource.Token);

            await CreateHostBuilder(args).Build().RunAsync(cancellationTokenSource.Token);
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}