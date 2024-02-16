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
        public List<EditorObject> Objects = new List<EditorObject>();
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
            if (Input.GetMouseButtonDown(0))
            {
                RaycastForObject();
            }

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
            foreach (EditorObject eo in Objects)
            {
                eo.Deselect();
            }
        }

        public void DoSelectMaterial()
        {
            foreach (EditorObject eo in Objects)
            {
                eo.Select();
            }
        }

        public void SelectObject(EditorObject eo)
        {
            if (Objects.Contains(eo))
            {
                return;
            }
            
            Objects.Add(eo);
            Gizmo.Instance.Refresh();
        }

        public void ClearIfNotShift()
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                Objects.Clear();
            }
            Gizmo.Instance.Refresh();
        }
    }
}