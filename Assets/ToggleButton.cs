using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CgpEditor
{
    public class ToggleButton : MonoBehaviour
    {
        public bool Enabled;
        [Header("Assets")]
        public Sprite EnabledSprite;
        public Sprite DisabledSprite;
        public Image Image;
        [Header("Events")]
        public UnityEvent EnableEvent;
        public UnityEvent DisableEvent;
        public UnityEvent ClickEvent;

        private void Start()
        {
            Refresh();
        }

        private void Refresh()
        {
            Image.sprite = Enabled ? EnabledSprite : DisabledSprite;
            (Enabled ? EnableEvent : DisableEvent).Invoke();
            ClickEvent.Invoke();
        }

        public void Toggle()
        {
            Enabled = !Enabled;
            Refresh();
        }
    }
}