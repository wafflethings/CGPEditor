using CgpEditor.IO;
using CgpEditor.Ux;
using UnityEngine;

namespace CgpEditor.Ux.MainPanel
{
    public class FilePanel : MonoBehaviour
    {
        public void SaveDialog()
        {
            FileIO.SaveDialog();
        }

        public void LoadDialog()
        {
#if !UNITY_WEBGL
            MessageBox.Instance.Show("-- SAVE --", "Do you want to save before loading?", LoadIfSaved, "YES", "NO");
#else
            FileIO.LoadDialog();
#endif
        }

        private void LoadIfSaved(MessageBoxButton btn)
        {
            if (btn == MessageBoxButton.Left)
            {
                if (!FileIO.SaveDialog())
                {
                    return;
                }
            }

            FileIO.LoadDialog();
        }
    }
}
