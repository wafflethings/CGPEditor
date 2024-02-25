using System.Collections.Generic;
using CgpEditor.Ux;
using UnityEngine;

namespace CgpEditor.LevelEditor.Selection.Modes
{
    public class SelectRect : ShapeSelectionType
    {
        public override void SelectionUpdate()
        {
            base.SelectionUpdate();
            
            if (Input.GetMouseButtonDown(0) && !OnUi)
            {
                RaycastForObject();
            }
            else
            {
                if (_points.Count == 1)
                {
                    RaycastForPainting();
                }
            }
        }
        
        private void RaycastForObject()
        {
            if (Physics.Raycast(CameraControls.Instance.Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, SelectionManager.Instance.ObjectMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out CGGridCube gc))
                {
                    SelectionManager.Instance.UndoSelectMaterial();
                    _points.Add(gc);
                    SelectionManager.Instance.SelectObject(gc);

                    if (_points.Count >= 2)
                    {
                        DrawBox(SelectionShapeMode.Select, _points[0], _points[1]);
                        _points.Clear();
                    }
                    else
                    {
                        SelectionManager.Instance.ClearIfNotShift();
                    }
                    
                    SelectionManager.Instance.DoSelectMaterial();
                }
            }
        }
        
        private void RaycastForPainting()
        {
            if (Physics.Raycast(CameraControls.Instance.Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, SelectionManager.Instance.ObjectMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out CGGridCube gc))
                {
                    DrawBox(SelectionShapeMode.Paint, _points[0], gc);
                }
            }
        }

        private void DrawBox(SelectionShapeMode mode, CGGridCube point0, CGGridCube point1)
        {
            Vector2Int xRange = new Vector2Int(point0.GridPosition.x, point1.GridPosition.x);
            Vector2Int yRange = new Vector2Int(point0.GridPosition.y, point1.GridPosition.y);

            if (xRange.y < xRange.x)
            {
                (xRange.x, xRange.y) = (xRange.y, xRange.x);
            }
            
            if (yRange.y < yRange.x)
            {
                (yRange.x, yRange.y) = (yRange.y, yRange.x);
            }

            foreach (CGGridCube cube in CGGrid.CurrentCgGrid.Cubes)
            {
                if (cube.GridPosition.x >= xRange.x && cube.GridPosition.x <= xRange.y &&
                    cube.GridPosition.y >= yRange.x && cube.GridPosition.y <= yRange.y)
                {
                    SelectionManager.Instance.SelectOrPaint(mode, cube);
                }
            }
        }
    }
}