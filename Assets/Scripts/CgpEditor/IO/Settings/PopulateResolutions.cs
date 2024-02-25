using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CgpEditor.IO.Settings
{
    public class PopulateResolutions : MonoBehaviour
    {
        private void Awake()
        {
            TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();
            dropdown.AddOptions(Screen.resolutions.Select(res => $"{res.width}x{res.height}").ToList());
        }
    }
}
