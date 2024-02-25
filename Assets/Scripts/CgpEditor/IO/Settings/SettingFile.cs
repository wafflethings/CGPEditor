using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace CgpEditor.IO.Settings
{
    public class SettingFile
    {
        public static string SettingPath => Path.Combine(FileIO.RootPath, "settings.json");
        public Dictionary<string, object> Data = new Dictionary<string, object>();

        public void Save()
        {
            #if !UNITY_WEBGL
            File.WriteAllText(SettingPath, JsonConvert.SerializeObject(this));
            #endif
        }

        public T GetSetting<T>(string id)
        {
            if (Data.TryGetValue(id, out object value))
            {
                Debug.Log($"Trying to get {id} type {value.GetType()}");
                return (T)value;
            }

            throw new Exception("Setting doesn't exist?");
        }

        public void SetSetting(string id, object value)
        {
            Data[id] = value;
            SettingsManager.PublicInvoke(SettingsManager.CurrentSave, new SettingChangeEventArgs { NewValue = value });
        }
    }
}
