using System;
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

        protected override void Awake()
        {
            base.Awake();
            gameObject.SetActive(false);
        }
        
        public void Show(string title, string content, Action<MessageBoxButton> onClick, string leftText = "OK", string rightText = "CANCEL")
        {
            gameObject.SetActive(true);
            Title.text = title;
            Content.text = content;
            Left.onClick.RemoveAllListeners();
            Left.onClick.AddListener(() => onClick.Invoke(MessageBoxButton.Left));
            Left.onClick.AddListener(() => gameObject.SetActive(false));
            Right.onClick.RemoveAllListeners();
            Right.onClick.AddListener(() => onClick.Invoke(MessageBoxButton.Right));
            Right.onClick.AddListener(() => gameObject.SetActive(false));
            LeftText.text = leftText;
            RightText.text = rightText;
        }
    }
}