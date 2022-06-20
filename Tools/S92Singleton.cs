using UnityEngine;

namespace Status92.Tools
{
    public class S92Singleton<T> : MonoBehaviour where T : S92Singleton<T> 
    {
        private static T _instance;
        public static bool HasInstance => _instance != null;
        public static T TryGetInstance() => HasInstance ? _instance : null;
        public static T Current => _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<T>();
                if (_instance != null) return _instance;
                var obj = new GameObject
                {
                    name = typeof(T).Name + "_AutoCreated"
                };
                _instance = obj.AddComponent<T>();
                _instance.TryInit();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (!Application.isPlaying) return;
            TryInit();
            _instance = this as T;
        }

        protected virtual void TryInit()
        {
        }
    }
}