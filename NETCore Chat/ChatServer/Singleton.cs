using System.Security.AccessControl;
using System.Text.Json;

namespace ChatServer
{
    public static class Singleton<T> where T : new()
    {

        private static readonly object _instanceLock = new object();
        private static T _instance = default(T);

        public static T GetInstance() {
            
            if (_instance != null) return _instance;
            
            lock(_instanceLock)
            {
                _instance ??= new T();
            }
            
            return _instance; 
            
        } 
        
    }
}