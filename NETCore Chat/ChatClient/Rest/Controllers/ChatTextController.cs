using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ChatClient.Rest.Controllers
{
    
    [ApiController]
    [Route("texts/")]
    public class ChatTextController : ControllerBase
    {

        private readonly IChatClientService _chatClientService = IChatClientService.GetInstance();

        public sealed class SendChatTextRequestBody
        {
            
            public string TextBody { get; set; }
            
        }
        
        [HttpPut]
        [Route("")]
        public async Task<ActionResult> SendChatText([FromBody] SendChatTextRequestBody requestBody)
        {

            await _chatClientService.SendChatText(requestBody.TextBody);
            return new AcceptedResult();

        }
        
    }
    
    
}