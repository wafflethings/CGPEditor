using System;
using CgpEditor.LevelEditor.Selection;
using CgpEditor.LevelEditor.Selection.Modes;
using UnityEngine;
using UnityEngine.UI;

namespace CgpEditor.Ux.MainPanel
{
    public class BrushPanel : MonoBehaviour
    {
        public Button ClickDragButton;
        public Button BoundsButton;
        public Button LineButton;
        public Button FillButton;

        private void Start()
        {
            ClickDragButton.onClick.AddListener(() => SelectionManager.Instance.SwitchSelection<ClickOrDrag>());
            BoundsButton.onClick.AddListener(() => SelectionManager.Instance.SwitchSelection<SelectionBox>());
            LineButton.onClick.AddListener(() => SelectionManager.Instance.SwitchSelection<LineSelect>());
            FillButton.onClick.AddListener(() => SelectionManager.Instance.SwitchSelection<FillBucket>());
        }
    }
}