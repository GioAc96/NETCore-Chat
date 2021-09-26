using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Threading.Tasks;
using ChatClient.Rest;
using ChatShared.Util;

namespace ChatClient {
    public class Program {
        public static async Task Main(string[] args)
        {

            await Singleton<Chat.ChatClient>.GetInstance().Start(IPAddress.Parse("127.0.0.1"), 8000);

            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
