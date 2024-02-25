using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CgpEditor.Ux
{
    /// <summary>
    /// Some WebGL things need stuff to happen on mousedown, not mouseup, for stuff to work.
    /// https://forum.unity.com/threads/webgl-file-upload.338547/
    /// </summary>
    public class WebGlMouseDownButton : MonoBehaviour, IPointerDownHandler
    {
        private Button _button;

        private void Awake()
        {
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                Destroy(this);
                return;
            }

            _button = GetComponent<Button>();
            _button.transition = Selectable.Transition.None;
            _button.interactable = false;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            _button.onClick.Invoke();
        }
    }
}
