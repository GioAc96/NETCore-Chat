namespace ChatShared.Util
{
    public static class Singleton<T> where T : new()
    {

        private static readonly object InstanceLock = new object();
        private static T _instance = default(T);

        public static T GetInstance() {
            
            if (_instance != null) return _instance;
            
            lock(InstanceLock)
            {
                _instance ??= new T();
            }
            
            return _instance; 
            
        } 
        
    }
}