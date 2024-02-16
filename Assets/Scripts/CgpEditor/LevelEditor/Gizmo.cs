using System;
using System.Collections.Generic;
using CgpEditor.LevelEditor.Selection;
using CgpEditor.Ux;
using UnityEngine;

namespace CgpEditor.LevelEditor
{
    public class Gizmo : MonoSingleton<Gizmo>, IClickable
    {
        public bool Dragging;
        private int _previousY;

        private void Start()
        {
            Refresh();
        }

        private void Update()
        {
            transform.localScale = Vector3.one * (Vector3.Distance(transform.position, CameraControls.Instance.Camera.transform.position) / 30);
            if (!Input.GetMouseButton(0))
            {
                Dragging = false;
            }

            if (Dragging)
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
                    
                    foreach (EditorObject eo in SelectionManager.Instance.Objects)
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
            Dragging = true;
            _previousY = (int)(transform.position.y / CGGridCube.OneCubeSize);
        }
        
        public void Refresh()
        {
            List<EditorObject> objects = SelectionManager.Instance.Objects;
            Vector3 sum = Vector3.zero;
            float maxY = float.MinValue;
                
            foreach (EditorObject eo in objects)
            {
                if (maxY < eo.transform.position.y)
                {
                    maxY = eo.transform.position.y;
                }
                sum += eo.transform.position;
            }

            if (objects.Count != 0)
            {
                gameObject.SetActive(true);
                Vector3 newPos = sum / objects.Count;
                newPos.y = maxY + CGGridCube.YOffset;
                gameObject.transform.position = newPos;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}