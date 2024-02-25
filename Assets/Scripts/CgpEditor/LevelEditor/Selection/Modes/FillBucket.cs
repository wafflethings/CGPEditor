using System;
using System.Collections.Generic;
using CgpEditor.Ux;
using UnityEngine;

namespace CgpEditor.LevelEditor.Selection.Modes
{
    public class FillBucket : SelectionType
    {
        public FillBucketMode Mode;
        private int _startObjectHeight;
        private static List<CGGridCube> _visited = new List<CGGridCube>();

        public Predicate<CGGridCube> BorderCondition
        {
            get
            {
                switch (Mode)
                {
                    case FillBucketMode.Selection:
                        return cube => SelectionManager.Instance.Objects.Contains(cube) == (SelectionManager.Instance.CurrentBrushMode == BrushMode.Select);
                    case FillBucketMode.Height:
                        return cube => cube.Height != _startObjectHeight;
                }

                return null;
            }
        }
        public override void SelectionUpdate()
        {
            if (Input.GetMouseButtonDown(0) && !OnUi)
            {
                SelectionManager.Instance.UndoSelectMaterial();
                if (Mode != FillBucketMode.Selection)
                {
                    SelectionManager.Instance.ClearIfNotShift();
                }

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
                    SelectionManager.Instance.UndoSelectMaterial();
                    _startObjectHeight = gc.Height;
                    _visited.Clear();
                    FloodFillIteration(CGGrid.CurrentCgGrid.Cubes, gc.GridPosition.x, gc.GridPosition.y, BorderCondition);
                    SelectionManager.Instance.DoSelectMaterial();
                }
            }
        }

        private static void FloodFillIteration(CGGridCube[,] bounds, int x, int y, Predicate<CGGridCube> notCondition)
        {
            if (x < 0 || x > bounds.GetLength(0) - 1 || y < 0 || y > bounds.GetLength(1) - 1 || _visited.Contains(bounds[x,y]) || notCondition.Invoke(bounds[x,y]))
            { 
                return;
            }
            
            _visited.Add(bounds[x,y]);
            SelectionManager.Instance.SelectOrPaint(SelectionShapeMode.Select, bounds[x,y]);
            
            FloodFillIteration(bounds, x - 1, y, notCondition);
            FloodFillIteration(bounds,x + 1, y, notCondition);
            FloodFillIteration(bounds, x , y - 1, notCondition);
            FloodFillIteration(bounds, x, y + 1, notCondition);
        }
    }
}