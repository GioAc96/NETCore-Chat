using System;

namespace ChatShared.Util
{
    public static class SettableSingleton<T>
    {
        private static readonly object InstanceLock = new object();
        private static T _instance;

        public static T GetInstance()
        {
            if (_instance != null) return _instance;

            lock (InstanceLock)
            {

                if (_instance is null) throw new InvalidOperationException();
                
            }

            return _instance;

        }

        public static void SetInstance(T instance)
        {

            lock (InstanceLock)
            {
                if (! (_instance is null))
                {
                    throw new InvalidOperationException();
                }

                _instance = instance;

            }

        }
        
    }
}