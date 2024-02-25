using UnityEngine;
using UnityEngine.UI;

namespace CgpEditor.IO.Settings.Setters
{
    public class ToggleBoolSetter : MonoBehaviour
    {
        public Toggle Toggle;
        public string Id;

        private void Start()
        {
            Toggle.onValueChanged.AddListener(value => SettingsManager.CurrentSave.SetSetting(Id, value));
            Toggle.isOn = SettingsManager.CurrentSave.GetSetting<bool>(Id);
        }
    }
}
