using UnityEngine;
using UnityEngine.Events;

namespace CgpEditor
{
    public class InvokeOnWebGl : MonoBehaviour
    {
        public UnityEvent OnWebGl;
        public UnityEvent OnNotWebGl;

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                OnWebGl.Invoke();
            }
            else
            {
                OnNotWebGl.Invoke();
            }
        }
    }
}
