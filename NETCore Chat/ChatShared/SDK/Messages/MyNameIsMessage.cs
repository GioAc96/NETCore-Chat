﻿using ProtoBuf;

namespace ChatShared.SDK.Messages
{
    
    [ProtoContract]
    public class MyNameIsMessage
    {

        [ProtoMember(1)]
        public readonly string UserName;

        public MyNameIsMessage(string userName)
        {
            UserName = userName;
        }
        
        private MyNameIsMessage(){}
        
    }
}