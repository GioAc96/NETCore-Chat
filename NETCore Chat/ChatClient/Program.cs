using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ChatClient.Rest;
using ChatShared.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ChatClient
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var tokenSource = new CancellationTokenSource();
            
            using IChatClientService chatClientService = await new Chat.ChatClient()
                .StartAsync(IPAddress.Parse("127.0.0.1"), 8000, tokenSource.Token);

            SettableSingleton<IChatClientService>.SetInstance(chatClientService);

            await CreateHostBuilder(args).Build().RunAsync(tokenSource.Token);
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}