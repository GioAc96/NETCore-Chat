namespace ChatServer
{
    public static class ChatServerProvider
    {
        
        private static readonly object InstanceLock = new object();
        
        private static Chat.ChatServer _instance = null;
        
        public static Chat.ChatServer GetInstance() {
            
            if (_instance != null) return _instance;
            
            lock(InstanceLock)
            {
                _instance ??= new Chat.ChatServer();
            }
            
            return _instance; 
            
        } 
        
    }
}