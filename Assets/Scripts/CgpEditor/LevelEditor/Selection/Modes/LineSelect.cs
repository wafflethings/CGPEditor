using System;
using System.Collections.Generic;
using CgpEditor.Ux;
using UnityEngine;

namespace CgpEditor.LevelEditor.Selection.Modes
{
    public class LineSelect : SelectionType
    {
        private List<Vector3> _points = new List<Vector3>();
        
        public override void SelectionUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastForObject();
            }
        }
        
        private void RaycastForObject()
        {
            if (Physics.Raycast(CameraControls.Instance.Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, SelectionManager.Instance.ObjectMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out EditorObject eo))
                {
                    SelectionManager.Instance.UndoSelectMaterial();
                    _points.Add(eo.transform.position);
                    SelectionManager.Instance.SelectObject(eo);

                    if (_points.Count >= 2)
                    {
                        DrawLine();
                    }
                    
                    SelectionManager.Instance.DoSelectMaterial();
                }
            }
        }

        private void DrawLine()
        {
            foreach (RaycastHit hit in Physics.RaycastAll(_points[0], _points[1] - _points[0], Vector3.Distance(_points[0], _points[1])))
            {
                if (hit.collider.gameObject.TryGetComponent(out EditorObject eo))
                {
                    SelectionManager.Instance.SelectObject(eo);
                }
            }
            
            _points.Clear();
        }
    }
}