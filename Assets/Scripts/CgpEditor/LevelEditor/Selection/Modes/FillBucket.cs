using System;
using CgpEditor.Ux;
using UnityEngine;

namespace CgpEditor.LevelEditor.Selection.Modes
{
    public class FillBucket : SelectionType
    {
        public override void SelectionUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectionManager.Instance.UndoSelectMaterial();
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
                    SelectionManager.Instance.UndoSelectMaterial();
                    FloodFillIteration(CGGrid.CurrentCgGrid.Cubes, eo.GridPosition.x, eo.GridPosition.y,(cube) => SelectionManager.Instance.Objects.Contains(cube.Object));
                    SelectionManager.Instance.DoSelectMaterial();
                }
            }
        }

        private static void FloodFillIteration(CGGridCube[,] bounds, int x, int y, Predicate<CGGridCube> notCondition)
        {
            if (x < 0 || x > bounds.GetLength(0) - 1 || y < 0 || y > bounds.GetLength(1) - 1 || notCondition.Invoke(bounds[x,y]))
            {
                return;
            }
            SelectionManager.Instance.SelectObject(bounds[x,y].Object);
            
            FloodFillIteration(bounds, x - 1, y, notCondition);
            FloodFillIteration(bounds,x + 1, y, notCondition);
            FloodFillIteration(bounds, x , y - 1, notCondition);
            FloodFillIteration(bounds, x, y + 1, notCondition);
        }
    }
}