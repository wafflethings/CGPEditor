using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace CgpEditor.IO.Settings
{
    public class SettingsManager : MonoSingleton<SettingsManager>
    {
        public delegate void SettingChangeEventHandler(object sender, SettingChangeEventArgs e);
        public static event SettingChangeEventHandler OnSettingChange;

        public static SettingFile CurrentSave
        {
            get
            {
                if (_currentSave == null)
                {
#if !UNITY_WEBGL
                    if (File.Exists(SettingFile.SettingPath))
                    {
                        _currentSave = JsonConvert.DeserializeObject<SettingFile>(File.ReadAllText(SettingFile.SettingPath));

                        foreach (var idc in _currentSave.Data)
                        {
                            Debug.Log($"{idc.Key} {idc.Value} ({idc.Value.GetType()})");
                        }
                    }
                    else
                    {
                        _currentSave = new SettingFile();
                    }
#else
                    _currentSave = new SettingFile();
#endif
                }

                return _currentSave;
            }
        }
        private static SettingFile _currentSave;

        public static void PublicInvoke(object sender, SettingChangeEventArgs e)
        {
            OnSettingChange.Invoke(sender, e);
        }

        private void CreateSetting<T>(string id, T defaultValue, Action<T> onChange)
        {
            if (!CurrentSave.Data.ContainsKey(id))
            {
                CurrentSave.Data.Add(id, defaultValue);
            }

            OnSettingChange += (sender, args) =>
            {
                onChange.Invoke(CurrentSave.GetSetting<T>(id));
            };
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
#if !UNITY_WEBGL
            CreateSetting<bool>("fullscreen", true, val => Screen.fullScreen = val);
            CreateSetting<long>("resolution", Array.IndexOf(Screen.resolutions, Screen.currentResolution), val =>
            {
                Resolution res = Screen.resolutions[val];
                Screen.SetResolution(res.width, res.height, CurrentSave.GetSetting<bool>("fullscreen"));
            });
#endif
            OnSettingChange?.Invoke(this, new SettingChangeEventArgs());
        }

        private void OnDestroy()
        {
            CurrentSave.Save();
        }
    }
}
