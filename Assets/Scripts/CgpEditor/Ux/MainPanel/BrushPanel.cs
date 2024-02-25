using System;
using System.Linq;
using CgpEditor.LevelEditor;
using CgpEditor.LevelEditor.Selection;
using CgpEditor.LevelEditor.Selection.Modes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CgpEditor.Ux.MainPanel
{
    public class BrushPanel : MonoBehaviour
    {
        public Button ClickDragButton;
        public Button BoundsButton;
        public Button LineButton;
        public Button FillSelectionButton;
        public Button FillHeightButton;
        public Button ChangeByButton;
        public Button SetToButton;
        public TMP_Text ToggleSelectDeselect;
        public TMP_InputField ChangeByField;
        public TMP_InputField SetToField;

        private void Start()
        {
            ClickDragButton.onClick.AddListener(() => SelectionManager.Instance.SwitchSelection<ClickOrDrag>());
            BoundsButton.onClick.AddListener(() => SelectionManager.Instance.SwitchSelection<SelectRect>());
            LineButton.onClick.AddListener(() => SelectionManager.Instance.SwitchSelection<LineSelect>());
            FillSelectionButton.onClick.AddListener(() =>
            {
                SelectionManager.Instance.SwitchSelection<FillBucket>();
                (SelectionManager.Instance.CurrentSelection as FillBucket).Mode = FillBucketMode.Selection;
            });
            FillHeightButton.onClick.AddListener(() =>
            {
                SelectionManager.Instance.SwitchSelection<FillBucket>();
                (SelectionManager.Instance.CurrentSelection as FillBucket).Mode = FillBucketMode.Height;
            });

            ChangeByField.onValueChanged.AddListener(value =>
            {
                ChangeByField.text = (value.StartsWith("-") ? "-" : string.Empty) + new string(value.Where(char.IsDigit).ToArray());
            });

            SetToField.onValueChanged.AddListener(value =>
            {
                SetToField.text = (value.StartsWith("-") ? "-" : string.Empty) + new string(value.Where(char.IsDigit).ToArray());
            });

            ChangeByButton.onClick.AddListener(() =>
            {
                if (ChangeByField.text.Length == 0)
                {
                    return;
                }
                
                ChangeBy(int.Parse(ChangeByField.text));
            });

            SetToButton.onClick.AddListener(() =>
            {
                if (SetToField.text.Length == 0)
                {
                    return;
                }

                SetTo(int.Parse(SetToField.text));
            });
        }

        public void ToggleSelection()
        {
            if (SelectionManager.Instance.CurrentBrushMode == BrushMode.Select)
            {
                SelectionManager.Instance.CurrentBrushMode = BrushMode.Deselect;
                ToggleSelectDeselect.text = "BRUSHES\nDESELECT";
            }
            else if (SelectionManager.Instance.CurrentBrushMode == BrushMode.Deselect)
            {
                SelectionManager.Instance.CurrentBrushMode = BrushMode.Select;
                ToggleSelectDeselect.text = "BRUSHES\nSELECT";
            }
        }

        public void SetTo(int amount)
        {
            foreach (CGGridCube cube in SelectionManager.Instance.Objects)
            {
                cube.SetHeight(amount);
            }

            Gizmo.Instance.Refresh();
        }

        public void ChangeBy(int amount)
        {
            foreach (CGGridCube cube in SelectionManager.Instance.Objects)
            {
                cube.SetHeight(cube.Height + amount);
            }

            Gizmo.Instance.Refresh();
        }
    }
}
