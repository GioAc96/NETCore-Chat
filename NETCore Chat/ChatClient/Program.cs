using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Threading.Tasks;
using ChatClient.Rest;

namespace ChatClient {
    public class Program {
        public static async Task Main(string[] args)
        {

            await new Chat.ChatClient().Start(IPAddress.Parse("127.0.0.1"), 8000);
            
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
