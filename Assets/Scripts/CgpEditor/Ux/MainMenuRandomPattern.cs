using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CgpEditor.LevelEditor;
using UnityEngine;

namespace CgpEditor.IO
{
    public class MainMenuRandomPattern : MonoBehaviour
    {
        private static List<string> s_patterns = new List<string>();

        private static string FilePath => Path.Combine(FileIO.RootPath, "lastpatterns");

        public static void Add(string path)
        {
            if (s_patterns.Contains(path))
            {
                return;
            }
            
            s_patterns.Add(path);

            while (s_patterns.Count > 10)
            {
                s_patterns.RemoveAt(0);
            }
        }

        public static string Get()
        {
            s_patterns.RemoveAll(str => !File.Exists(str));

            if (s_patterns.Count == 0)
            {
                return string.Empty;
            }
            
            return s_patterns[UnityEngine.Random.Range(0, s_patterns.Count)];
        }

        private void Start()
        {
            Load();
            string patternPath = Get();
            
            if (patternPath == string.Empty)
            {
                CGGrid.CreateGrid(16);
            }
            else
            {
                FileIO.LoadFile(patternPath);
            }
        }

        private void OnDestroy()
        {
            Save();
        }

        private void Save()
        {
            File.WriteAllLines(FilePath, s_patterns);
        }

        private void Load()
        {
            s_patterns.Clear();
            s_patterns.AddRange(File.ReadAllLines(FilePath));
        }
    }
}