using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CgpEditor.IO.Settings.Setters
{
    public class DropdownIntSetter : MonoBehaviour
    {
        public TMP_Dropdown Dropdown;
        public string Id;

        private void Start()
        {
            Dropdown.onValueChanged.AddListener(value => SettingsManager.CurrentSave.SetSetting(Id, (long)value));
            Dropdown.value = (int)SettingsManager.CurrentSave.GetSetting<long>(Id);
        }
    }
}
