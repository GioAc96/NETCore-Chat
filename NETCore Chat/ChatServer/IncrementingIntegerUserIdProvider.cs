using ChatServer;

namespace ChatServer
{
    public class IncrementingIntegerUserIdProvider : IUserIdProvider
    {

        private int _nextId = 0;
        
        public int NewId()
        {

            return ++_nextId;

        }
    }
}