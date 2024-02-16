using System;
using System.Linq;
using CgpEditor.IO;
using CgpEditor.LevelEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CgpEditor.Ux
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject Editor;
        public TMP_InputField NameInput;
        public TMP_InputField SizeInput;

        private void Start()
        {
            SizeInput.onValueChanged.AddListener((value) =>
            {
                SizeInput.text = new string(value.Where(char.IsDigit).ToArray());
            });
        }
        
        public void TryLoadAndDisable()
        {
            if (FileIO.LoadDialog())
            {
                ToggleEditor();
            }
        }

        public void TryCreatePattern()
        {
            string name = NameInput.text;

            if (name == string.Empty || !int.TryParse(SizeInput.text, out int size))
            {
                return;
            }

            FileData.CurrentFile = new FileData(name, size == 16 ? "cgp" : "cgpe");
            CGGrid.CreateGrid(size);
            ToggleEditor();
        }
        
        private void ToggleEditor()
        {
            gameObject.SetActive(false);
            Editor.SetActive(true);
            CameraControls.Instance.Enable();
        }

        public void TryExitEditor()
        {
            MessageBox.Instance.Show("-- SAVE --", "Do you want to save before leaving?", ExitIfSaved, "YES", "NO");
        }
        
        public void EnableAgain()
        {
            gameObject.SetActive(true);
            Editor.SetActive(false);
            CameraControls.Instance.Disable();
        }
        
        private void ExitIfSaved(MessageBoxButton btn)
        {
            if (btn == MessageBoxButton.Left)
            {
                if (!FileIO.SaveDialog())
                {
                    return;
                }
            }
            
            EnableAgain();
        }
    }
}