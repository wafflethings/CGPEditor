using CgpEditor.Ux;
using UnityEngine;

namespace CgpEditor.LevelEditor.Selection.Modes
{
    public class SelectionBox : SelectionType
    {
        private Vector3 _startMousePos;
        private bool _wasDraggingBox;
        
        private bool DraggingBox => Input.GetMouseButton(0) && Vector3.SqrMagnitude(_startMousePos - (Vector3)FixedMousePos) > 3500 && !Gizmo.Instance.Dragging;
        private Vector2 FixedMousePos => new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        
        public override void SelectionUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startMousePos = FixedMousePos;
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
            ScreenRendering.DrawRectBetween2Points(_startMousePos, FixedMousePos, new Color(0.75f, 1, 1, 0.5f));
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
            
            SelectionManager.Instance.UndoSelectMaterial();
            SelectionManager.Instance.ClearIfNotShift();
            
            foreach (CGGridCube cube in CGGrid.CurrentCgGrid.Cubes)
            {
                Vector3 fixedCubePos = CameraControls.Instance.Camera.WorldToScreenPoint(cube.transform.position + (CGGridCube.YOffset * Vector3.up));
                fixedCubePos.y = Screen.height - fixedCubePos.y;
                if (rect.Contains(fixedCubePos) && Physics.Raycast(CameraControls.Instance.transform.position, 
                        cube.transform.position - CameraControls.Instance.transform.position, SelectionManager.Instance.ObjectMask))
                {
                    SelectionManager.Instance.SelectObject(cube.Object);
                }
            }
            
            SelectionManager.Instance.DoSelectMaterial();
        }
    }
}