using System;
using System.Collections.Generic;
using System.Linq;
using CgpEditor.Ux;
using UnityEngine;
using UnityEngine.UI;

namespace CgpEditor.LevelEditor
{
    public class Selection : MonoSingleton<Selection>
    {
        public LayerMask ObjectMask;
        public GameObject Arrow;
        public Image SelectRect;
        public List<EditorObject> Objects = new List<EditorObject>();
        private Vector3 _startMousePos;
        private bool _wasDraggingBox;
        
        private bool DraggingBox => Input.GetMouseButton(0) && Vector3.SqrMagnitude(_startMousePos - (Vector3)FixedMousePos) > 3500;
        private Vector2 FixedMousePos => new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startMousePos = FixedMousePos;
            }
            
            if (!DraggingBox && (Input.GetMouseButtonUp(0) || (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))))
            {
                TrySelect();
            }

            if (DraggingBox)
            {
                _wasDraggingBox = true;
            }
            else
            {
                if (_wasDraggingBox)
                {
                    DragBoxReleased();
                }

                _wasDraggingBox = false;
            }
        }

        private void OnGUI()
        {
            if (DraggingBox)
            {
                DoSelectRect();
            }
        }

        private void DoSelectRect()
        {
            ScreenRendering.DrawRectBetween2Points(_startMousePos, FixedMousePos, new Color(1, 1, 1, 0.5f));
        }

        private void DragBoxReleased()
        {
            Rect rect = new Rect
            {
                xMin = FixedMousePos.x,
                xMax = _startMousePos.x,
                yMin = FixedMousePos.y,
                yMax = _startMousePos.y
            };
            rect = rect.FixNegativeSize();
            
            foreach (CGGridCube cube in CGGrid.CurrentCgGrid.Cubes)
            {
                Vector3 fixedCubePos = CameraControls.Instance.Camera.WorldToScreenPoint(cube.transform.position + (CGGridCube.YOffset * Vector3.up));
                fixedCubePos.y = Screen.height - fixedCubePos.y;
                if (rect.Contains(fixedCubePos))
                {
                    SelectObject(cube.Object);
                }
            }
        }

        private void UndoSelectMaterial()
        {
            foreach (EditorObject eo in Objects)
            {
                eo.Deselect();
            }
        }

        private void DoSelectMaterial()
        {
            foreach (EditorObject eo in Objects)
            {
                eo.Select();
            }
        }

        public void SelectObject(EditorObject eo, bool deselectOthers = false)
        {
            if (Objects.Contains(eo))
            {
                return;
            }
            
            UndoSelectMaterial();
            if (deselectOthers && !Input.GetKey(KeyCode.LeftShift))
            {
                Objects.Clear();
            }
            Objects.Add(eo);
            DoSelectMaterial();
            RefreshGizmo();
        }

        private void RefreshGizmo()
        {
            Vector3 sum = Vector3.zero;
            float maxY = float.MinValue;
                
            foreach (EditorObject eo in Objects)
            {
                if (maxY < eo.transform.position.y)
                {
                    maxY = eo.transform.position.y;
                }
                sum += eo.transform.position;
            }

            if (Objects.Count != 0)
            {
                Arrow.SetActive(true);
                Vector3 newPos = sum / Objects.Count;
                newPos.y = maxY + 25;
                Arrow.transform.position = newPos;
            }
            else
            {
                Arrow.SetActive(false);
            }
        }

        private void TrySelect()
        {
            if (Physics.Raycast(CameraControls.Instance.Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, ObjectMask))
            {
                Debug.Log($"Just hit {hit.collider.gameObject}");

                foreach (IClickable clickable in hit.collider.gameObject.GetComponents<IClickable>())
                {
                    clickable.Clicked();
                }
            }
        }
    }
}