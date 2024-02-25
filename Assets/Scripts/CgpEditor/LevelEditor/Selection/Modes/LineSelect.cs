using System;
using System.Collections;
using System.Collections.Generic;
using CgpEditor.Ux;
using UnityEngine;

namespace CgpEditor.LevelEditor.Selection.Modes
{
    public class LineSelect : ShapeSelectionType
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
                    _points.Add(gc);
                    
                    if (_points.Count >= 2)
                    {
                        SelectionManager.Instance.UndoSelectMaterial();
                        SelectionManager.Instance.ClearIfNotShift();
                        DrawLine(SelectionShapeMode.Select, _points[0], _points[1]);
                        SelectionManager.Instance.DoSelectMaterial();
                    }
                }
            }
        }

        private void RaycastForPainting()
        {
            if (Physics.Raycast(CameraControls.Instance.Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, SelectionManager.Instance.ObjectMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out CGGridCube gc))
                {
                    DrawLine(SelectionShapeMode.Paint, _points[0], gc);
                }
            }
        }
        
        private void DrawLine(SelectionShapeMode mode, CGGridCube point0, CGGridCube point1)
        {
            Vector2 direction = ((Vector2)point1.GridPosition - point0.GridPosition);
            if (direction.x == 0 && direction.y == 0)
            {
                Debug.LogError("No direction!");
                if (mode == SelectionShapeMode.Select)
                {
                    _points.Clear();
                }
                return;
            }

            direction /= Mathf.Abs(Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? direction.x : direction.y);

            Vector2 currentPosition = point0.GridPosition;
            int iterations = 0;
            int maxIterations = CGGrid.CurrentCgGrid.Length;
            do
            {
                Debug.Log($"{point0} to {point1} - at {currentPosition} going at {direction} (initially {point1.GridPosition - point0.GridPosition})");
                Vector2Int closest = new Vector2Int((int)Mathf.Round(currentPosition.x), (int)Mathf.Round(currentPosition.y));

                if (closest.x < 0 || closest.x > CGGrid.CurrentCgGrid.Length - 1 || closest.y < 0 || closest.y > CGGrid.CurrentCgGrid.Length - 1)
                {
                    Debug.Log("Breaking: reached border");
                    break;
                }
                SelectionManager.Instance.SelectOrPaint(mode, CGGrid.CurrentCgGrid.Cubes[closest.x, closest.y]);
                currentPosition += direction;
                iterations++;
            } 
            while (currentPosition != point1.GridPosition && iterations < maxIterations);
            SelectionManager.Instance.SelectOrPaint(mode, point1);
 
            if (mode == SelectionShapeMode.Select)
            {
                _points.Clear();
            }
        }
    }
}