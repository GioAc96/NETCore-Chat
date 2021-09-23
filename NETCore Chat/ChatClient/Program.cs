using System.Net;
using System.Threading.Tasks;

namespace ChatClient {
    class Program {
        
        static async Task Main(string[] args)
        {

            await new Client().Start(IPAddress.Parse("127.0.0.1"), 8000);

        }
    }
}
