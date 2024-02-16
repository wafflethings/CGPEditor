using UnityEngine;

namespace CgpEditor
{
    public class MonoSingleton<T> : MonoSingleton where T : MonoSingleton<T>
    {
        public static T Instance => _instance;
        private static T _instance;

        protected virtual void Awake()
        {
            _instance = (T)this;
        }
    }
    
    public class MonoSingleton : MonoBehaviour
    {
        
    }
}