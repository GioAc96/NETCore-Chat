using System.Collections.Generic;
using ChatServer.Model;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Rest.Controllers
{
    [ApiController]
    [Route("texts/")]
    public class ChatTextsController : ControllerBase
    {
        private static readonly IChatService ChatService = IChatService.GetInstance();

        [HttpGet]
        [Route("index")]
        [Route("")]
        public ActionResult<IEnumerable<ChatText>> Index()
        {
            return new ActionResult<IEnumerable<ChatText>>(ChatService.GetAllTexts());
        }
    }
}