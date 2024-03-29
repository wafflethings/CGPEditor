﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using CgpEditor.LevelEditor;
using CgpEditor.Ux;
using SFB;
using UnityEngine;

namespace CgpEditor.IO
{
    public static class FileIO
    {
        public static readonly Dictionary<CGPrefabType, char> PrefabToChar = new Dictionary<CGPrefabType, char>()
        {
            {CGPrefabType.None, '0'},
            {CGPrefabType.Melee, 'n'},
            {CGPrefabType.Projectile, 'p'},
            {CGPrefabType.JumpPad, 'J'},
            {CGPrefabType.Stairs, 's'},
            {CGPrefabType.Mass, 'H'},
        };

        public static readonly Dictionary<char, CGPrefabType> CharToPrefab = new Dictionary<char, CGPrefabType>()
        {
            {'0', CGPrefabType.None},
            {'n', CGPrefabType.Melee},
            {'p', CGPrefabType.Projectile},
            {'J', CGPrefabType.JumpPad},
            {'s', CGPrefabType.Stairs},
            {'H', CGPrefabType.Mass},
        };

        private static ExtensionFilter[] _validExtensions = { new ExtensionFilter("Cybergrind pattern ", "cgp", "cgpe") };

        public static string RootPath => Directory.GetParent(Application.dataPath).FullName;
        private static string PatternPath => Path.Combine(RootPath, "Patterns");

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

        [DllImport("__Internal")]
        private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);
#endif

#if !UNITY_WEBGL
        public static void SaveFile(string path)
        {
            File.WriteAllLines(path, SerializeGrid(CGGrid.CurrentCgGrid));
        }

        public static void LoadFile(string path)
        {
            Debug.Log("Loading from " + path);
            string ext = path.Split('.').Last();
            string fileName = path.Split(Path.DirectorySeparatorChar).Last();
            fileName = fileName.Substring(0, fileName.IndexOf(ext, StringComparison.Ordinal) - 1);
            FileData.CurrentFile = new FileData(fileName, ext);
            DeserializeGrid(File.ReadAllLines(path));
            MainMenuRandomPattern.Add(path);
        }
#else
        public static void SaveFileWeb()
        {
            string fileName = $"{FileData.CurrentFile.FileName}.{FileData.CurrentFile.Extension}";
            byte[] array = Encoding.UTF8.GetBytes(string.Join("\n", SerializeGrid(CGGrid.CurrentCgGrid)));
            DownloadFile(WebGlFileReceiver.Instance.gameObject.name, nameof(WebGlFileReceiver.OnFileDownload), fileName, array, array.Length);
        }

        public static IEnumerator LoadFileWeb(string path)
        {
            Debug.Log("Loading from " + path);
            WWW loader = new WWW(path);
            yield return loader;
            string[] file = loader.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            string ext = file.Length != ((16 * 2) + 1) ? "cgpe" : "cgp";
            FileData.CurrentFile = new FileData("pattern", ext);
            Debug.Log("Loaded " + loader.text);
            DeserializeGrid(file);
        }
#endif

        public static bool SaveDialog()
        {
#if !UNITY_WEBGL
            EnsurePatternFolder();
            string path = StandaloneFileBrowser.SaveFilePanel("Save pattern", PatternPath, FileData.CurrentFile.FileName, FileData.CurrentFile.Extension);

            if (path == string.Empty)
            {
                return false;
            }

            SaveFile(path);
            return true;
#else
            SaveFileWeb();
            return true;
#endif
        }

        public static bool LoadDialog()
        {
#if !UNITY_WEBGL
            EnsurePatternFolder();
            string[] files = StandaloneFileBrowser.OpenFilePanel("Load pattern", PatternPath, _validExtensions, false);

            if (files.Length == 0)
            {
                return false;
            }

            LoadFile(files[0]);
            return true;
#else
            UploadFile(WebGlFileReceiver.Instance.gameObject.name, nameof(WebGlFileReceiver.OnFileUpload), "", false);
            return true;
#endif
        }

        private static void EnsurePatternFolder()
        {
            if (!Directory.Exists(PatternPath))
            {
                Directory.CreateDirectory(PatternPath);
            }
        }

        public static string[] SerializeGrid(CGGrid grid)
        {
            List<string> result = new List<string>();
            result.AddRange(SerializeHeights(grid.Heights));
            result.Add(string.Empty);
            result.AddRange(SerializePrefabs(grid.Prefabs));
            return result.ToArray();
        }

        private static void DeserializeGrid(string[] lines)
        {
            List<string> environment = new List<string>();
            List<string> enemies = new List<string>();
            List<string> targetList = environment;

            foreach (string line in lines)
            {
                if (line == string.Empty)
                {
                    targetList = enemies;
                    continue;
                }

                targetList.Add(line);
            }

            int[,] heights = DeserializeHeights(environment);
            CGPrefabType[,] prefabs = DeserializePrefabs(enemies);
            CGGrid cgGrid = CGGrid.CreateGrid(heights.GetLength(0));
            cgGrid.Heights = heights;
            cgGrid.Prefabs = prefabs;

            foreach (CGGridCube gc in cgGrid.Cubes)
            {
                gc.RefreshPosition();
                gc.RefreshPrefab();
            }
        }

        public static int[,] DeserializeHeights(List<string> pattern)
        {
            int[,] result = new int[pattern.Count, pattern.Count];

            int y = 0;
            foreach (string row in pattern)
            {
                if (!CorrectlyClosedBrackets(row))
                {
                    Debug.LogError("Brackets don't close in pattern.");
                    return null;
                }

                if (!MinusesInValidPositions(row))
                {
                    Debug.LogError("Minuses are invalid in pattern.");
                    return null;
                }

                bool insideBracket = false;
                string newRow = string.Empty;

                foreach (char character in row)
                {
                    //dont need to check if its opening or closing since CorrectlyClosedBrackets does that
                    if (character == '(' || character == ')')
                    {
                        if (character == ')')
                        {
                            newRow += "|";
                        }
                        insideBracket = !insideBracket;
                        continue;
                    }

                    if (!insideBracket)
                    {
                        newRow += $"{character}|";
                    }
                    else
                    {
                        newRow += character;
                    }
                }

                newRow = newRow.Replace("(", "|").Replace(")", "|");
                newRow = newRow.Substring(0, newRow.Length - 1); //remove last |

                int x = 0;
                foreach (int number in newRow.Split('|').Select(str => int.Parse(str)))
                {
                    result[x, y] = number;
                    x++;
                }

                y++;
            }

            if (result.GetLength(0) != result.GetLength(1))
            {
                Debug.LogError("Pattern is not square.");
                return null;
            }

            return result;
        }

        public static CGPrefabType[,] DeserializePrefabs(List<string> lines)
        {
            CGPrefabType[,] result = new CGPrefabType[lines.Count, lines.Count];

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                for (int j = 0; j < line.Length; j++)
                {
                    char character = line[j];
                    result[j, i] = CharToPrefab[character];
                }
            }

            return result;
        }

        public static string[] SerializePrefabs(CGPrefabType[,] prefabs)
        {
            string[] result = new string[prefabs.GetLength(1)];

            for (int x = 0; x < prefabs.GetLength(0); x++)
            {
                string row = string.Empty;

                for (int y = 0; y < prefabs.GetLength(1); y++)
                {
                    row += PrefabToChar[prefabs[x, y]];
                }

                result[x] = row;
            }

            return result;
        }

        public static string[] SerializeHeights(int[,] heights)
        {
            string[] result = new string[heights.GetLength(1)];

            for (int x = 0; x < heights.GetLength(0); x++)
            {
                string row = string.Empty;

                for (int y = 0; y < heights.GetLength(1); y++)
                {
                    string number = heights[x, y].ToString();
                    row += number.Length > 1 ? $"({number})" : number;
                }

                result[x] = row;
            }

            return result;
        }

        private static bool CorrectlyClosedBrackets(string line) => Regex.Matches(line, @"\(").Count == Regex.Matches(line, @"\)").Count;

        private static bool MinusesInValidPositions(string line)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != '-')
                {
                    continue;
                }

                //character after minus is a digit
                if (char.IsDigit(line[i + 1]))
                {
                    continue;
                }

                //character before minus is a (
                if (line[i - 1] != '(')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
