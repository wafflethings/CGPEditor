using System;
using System.Collections.Generic;
using CgpEditor.LevelEditor.Selection.Modes;
using CgpEditor.Ux;
using UnityEngine;

namespace CgpEditor.LevelEditor.Selection
{
    public class SelectionManager : MonoSingleton<SelectionManager>
    {
        public LayerMask ObjectMask;
        public LayerMask GizmoMask;
        public SelectionType CurrentSelection;
        public List<CGGridCube> Objects = new List<CGGridCube>();
        public BrushMode CurrentBrushMode;
        private List<CGGridCube> _currentlyPaintingObjects = new List<CGGridCube>();
        private Dictionary<Type, SelectionType> _selectionTypeFromType = new Dictionary<Type, SelectionType>();

        private void Start()
        {
            foreach (SelectionType st in GetComponents<SelectionType>())
            {
                _selectionTypeFromType.Add(st.GetType(), st);
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UndoSelectMaterial();
                Objects.Clear();
                Gizmo.Instance.Refresh();
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                RaycastForObject();
            }

            ClearPaintingObjects();
            if (CurrentSelection?.IsActive ?? false)
            {
                CurrentSelection.SelectionUpdate();
            }
        }
        
        private void RaycastForObject()
        {
            if (Physics.Raycast(CameraControls.Instance.Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, GizmoMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out IClickable clickable))
                {
                    clickable.Clicked();
                }
            }
        }

        public void SwitchSelection<T>() where T : SelectionType
        {
            CurrentSelection = _selectionTypeFromType[typeof(T)];
            CurrentSelection.enabled = true;
        }
        
        public void UndoSelectMaterial()
        {
            foreach (CGGridCube gc in Objects)
            {
                gc.Deselect();
            }
        }

        public void DoSelectMaterial()
        {
            foreach (CGGridCube gc in Objects)
            {
                gc.Select();
            }
        }

        public void SelectObject(CGGridCube gc)
        {
            if (Objects.Contains(gc))
            {
                return;
            }
            
            Objects.Add(gc);
            Gizmo.Instance.Refresh();
        }
        
        public void DeselectObject(CGGridCube gc)
        {
            if (!Objects.Contains(gc))
            {
                return;
            }
            
            Objects.Remove(gc);
            Gizmo.Instance.Refresh();
        }

        public void UseObjectToPaint(CGGridCube gc)
        {
            if (_currentlyPaintingObjects.Contains(gc))
            {
                return;
            }
            
            gc.Painting();
            _currentlyPaintingObjects.Add(gc);
        }

        public void SelectOrPaint(SelectionShapeMode mode, CGGridCube CGGridCube)
        {
            if (mode == SelectionShapeMode.Select)
            {
                if (CurrentBrushMode == BrushMode.Select)
                {
                    SelectObject(CGGridCube);
                }
                else if (CurrentBrushMode == BrushMode.Deselect)
                {
                    DeselectObject(CGGridCube);
                }
            } 
            else if (mode == SelectionShapeMode.Paint)
            {
                UseObjectToPaint(CGGridCube);
            }
        }

        public void ClearPaintingObjects()
        {
            foreach (CGGridCube gc in _currentlyPaintingObjects)
            {
                gc.StopPainting();
            }
            _currentlyPaintingObjects.Clear();
        }

        public void ClearIfNotShift()
        {
            if (!Input.GetKey(KeyCode.LeftShift) && CurrentBrushMode == BrushMode.Select)
            {
                Objects.Clear();
            }
            Gizmo.Instance.Refresh();
        }

        public void FillSelectionWithPrefab(CGPrefabType type)
        {
            foreach (CGGridCube CGGridCube in Objects)
            {
                CGGrid.CurrentCgGrid.SetPrefab(type, CGGridCube.GridPosition.x, CGGridCube.GridPosition.y);
            }
        }
    }
}