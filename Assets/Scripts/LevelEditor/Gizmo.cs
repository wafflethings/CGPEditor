using System;
using CgpEditor.Ux;
using UnityEngine;

namespace CgpEditor.LevelEditor
{
    public class Gizmo : MonoBehaviour, IClickable
    {
        private int _previousY;
        private bool _dragging = false;

        private void Update()
        {
            transform.localScale = Vector3.one * (Vector3.Distance(transform.position, CameraControls.Instance.Camera.transform.position) / 30);
            if (!Input.GetMouseButton(0))
            {
                _dragging = false;
            }

            if (_dragging)
            {
                Debug.Log("Dragging");
                Vector3 direction = CameraControls.Instance.transform.position - transform.position;
                direction.y = 0;
                Plane gizmoPlane = new Plane(direction.normalized, direction.magnitude);
                
                Ray ray = CameraControls.Instance.Camera.ScreenPointToRay(Input.mousePosition);
                if (gizmoPlane.Raycast(ray, out float enterPoint))
                {
                    Vector3 hitPoint = ray.GetPoint(enterPoint);
                    int currentY = (int)(hitPoint.y / CGGridCube.OneCubeSize);
                    transform.position = new Vector3(transform.position.x, currentY * CGGridCube.OneCubeSize, transform.position.z);
                    int yDifference = currentY - _previousY;
                    _previousY = currentY;
                    
                    foreach (EditorObject eo in Selection.Instance.Objects)
                    {
                        if (!eo.TryGetComponent(out CGGridCube gc))
                        {
                            return;
                        }
                        
                        gc.SetHeight(gc.Height + yDifference);
                    }
                }
            }
        }

        public void Clicked()
        {
            _dragging = true;
            _previousY = (int)(transform.position.y / CGGridCube.OneCubeSize);
        }
    }
}