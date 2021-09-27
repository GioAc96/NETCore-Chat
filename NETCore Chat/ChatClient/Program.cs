using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ChatClient.Rest;
using ChatShared.Util;

namespace ChatClient {
    public static class Program
    {

        public static async Task Main(string[] args)
        {

            var tokenSource = new CancellationTokenSource();
            
            CreateHostBuilder(args).Build().RunAsync(tokenSource.Token);

            await Singleton<Chat.ChatClient>.GetInstance().StartAsync(IPAddress.Parse("127.0.0.1"), 8000, tokenSource.Token);
            
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
