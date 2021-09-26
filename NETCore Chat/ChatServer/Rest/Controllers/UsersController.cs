using System;
using System.Collections.Generic;
using ChatServer.Model;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Rest.Controllers {
    
    [ApiController]
    [Route("[controller]/")]
    public class UsersController : ControllerBase
    {

        private static readonly IChatService ChatService = IChatService.GetInstance();

        public class UserDetails : User
        {

            public int MessagesCount => ChatService.GetMessagesCount(this);

            private UserDetails(User user) : base(user.Id, user.Name)
            {
            }

            public static UserDetails FromUser(User user)
            {
                
                if (user is null)
                {
                    return null;
                }

                return new UserDetails(user);
            }
            
        }
        
        [HttpGet]
        [Route("index")]
        [Route("")]
        public ActionResult<IEnumerable<User>> Index()
        {

            return new ActionResult<IEnumerable<User>>(ChatService.GetConnectedUsers());

        }

        [HttpGet]
        [Route("{userId}")]
        public ActionResult<UserDetails> FindById([FromRoute] Guid userId)
        {
            
            return UserDetails.FromUser(ChatService.GetUserById(userId));
            
        }

        [HttpGet]
        [Route("findByName/{userName}")]
        public ActionResult<UserDetails> FindByName([FromRoute] string userName)
        {
            
            return UserDetails.FromUser(ChatService.GetUserByName(userName));

        }
        
    }
    
}
