using System;
using CgpEditor.Ux.MainPanel;
using TMPro;
using UnityEngine.UI;

namespace CgpEditor.Ux
{
    public class MessageBox : MonoSingleton<MessageBox>
    {
        public TMP_Text Title;
        public TMP_Text Content;
        public Button Left;
        public Button Right;
        public TMP_Text LeftText;
        public TMP_Text RightText;
        private CrtToggler _crt;

        protected override void Awake()
        {
            base.Awake();
            gameObject.SetActive(false);
            _crt = GetComponent<CrtToggler>();
        }

        public void Show(string title, string content, Action<MessageBoxButton> onClick, string leftText = "OK", string rightText = "CANCEL")
        {
            _crt.Enable();
            Title.text = title;
            Content.text = content;
            Left.onClick.RemoveAllListeners();
            Left.onClick.AddListener(() => onClick.Invoke(MessageBoxButton.Left));
            Left.onClick.AddListener(() => _crt.Disable());
            Right.onClick.RemoveAllListeners();
            Right.onClick.AddListener(() => onClick.Invoke(MessageBoxButton.Right));
            Right.onClick.AddListener(() => _crt.Disable());
            LeftText.text = leftText;
            RightText.text = rightText;
        }

        private void OnDisable()
        {
            gameObject.SetActive(false);
        }
    }
}
