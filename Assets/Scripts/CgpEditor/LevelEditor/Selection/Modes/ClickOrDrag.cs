using System;
using CgpEditor.Ux;
using UnityEngine;

namespace CgpEditor.LevelEditor.Selection.Modes
{
    public class ClickOrDrag : SelectionType
    {
        public override void SelectionUpdate()
        {
            if (!OnUi && (Input.GetMouseButtonUp(0) || (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))))
            {
                SelectionManager.Instance.UndoSelectMaterial();
                SelectionManager.Instance.ClearIfNotShift();
                RaycastForObject();
                SelectionManager.Instance.DoSelectMaterial();
            }
        }
        
        private void RaycastForObject()
        {
            if (Physics.Raycast(CameraControls.Instance.Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, SelectionManager.Instance.ObjectMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out EditorObject eo))
                {
                    SelectionManager.Instance.SelectObject(eo);
                }
            }
        }
    }
}