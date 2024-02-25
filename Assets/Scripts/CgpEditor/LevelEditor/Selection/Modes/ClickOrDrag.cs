using System;
using CgpEditor.Ux;
using UnityEngine;

namespace CgpEditor.LevelEditor.Selection.Modes
{
    public class ClickOrDrag : SelectionType
    {
        public override void SelectionUpdate()
        {
            if ((Input.GetMouseButtonUp(0) || (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))) && !OnUi)
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
                if (hit.collider.gameObject.TryGetComponent(out CGGridCube gc))
                {
                    SelectionManager.Instance.SelectOrPaint(SelectionShapeMode.Select, gc);
                }
            }
        }
    }
}