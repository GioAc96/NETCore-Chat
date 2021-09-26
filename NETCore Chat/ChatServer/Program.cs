using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Threading.Tasks;
using ChatServer.Rest;

namespace ChatServer {
    
    public static class Program {
        
        private const int ChatPort = 8000;
        private static readonly IPAddress ChatIpAddress = IPAddress.Any;

        
        public static async Task Main(string[] args)
        {

            var chatServer = Singleton<Chat.ChatServer>.GetInstance();

            chatServer.Start(ChatIpAddress, ChatPort);
            
            await CreateHostBuilder(args).Build().RunAsync();
            
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
