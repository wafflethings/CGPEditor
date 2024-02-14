using System;
using System.IO;
using System.Linq;
using CgpEditor.LevelEditor;
using SFB;
using UnityEditor;
using UnityEngine;

namespace CgpEditor.IO
{
    public class FilePanel : MonoBehaviour
    {
        private static ExtensionFilter[] _validExtensions = { new ExtensionFilter("Cybergrind pattern ", "cgp", "cgpe") };
        private static string PatternPath => Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Patterns");

        private void Start()
        {
            if (!Directory.Exists(PatternPath))
            {
                Directory.CreateDirectory(PatternPath);
            }
        }

        public void SaveDialog()
        {
            string path = StandaloneFileBrowser.SaveFilePanel("Save pattern", PatternPath, FileData.CurrentFile.FileName, FileData.CurrentFile.Extension);

            if (path == string.Empty)
            {
                return;
            }

            FileIO.SaveFile(path);
        }

        public void LoadDialog()
        {
            string[] files = StandaloneFileBrowser.OpenFilePanel("Load pattern", PatternPath, _validExtensions, false);

            if (files.Length == 0)
            {
                return;
            }
            
            FileIO.LoadFile(files[0]);
        }
    }
}