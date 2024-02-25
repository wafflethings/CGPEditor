using System;
using System.Collections.Generic;
using CgpEditor.LevelEditor.Selection;
using CgpEditor.Ux;
using UnityEngine;

namespace CgpEditor.LevelEditor
{
    public class Gizmo : MonoSingleton<Gizmo>, IClickable
    {
        public Material NormalMaterial;
        public Material ClickedMaterial;
        public bool Dragging;
        private float _previousY;
        private MeshRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponentInChildren<MeshRenderer>();
            Refresh();
        }

        private void Update()
        {
            transform.localScale = Vector3.one * (Vector3.Distance(transform.position, CameraControls.Instance.Camera.transform.position) / 30);
            if (!Input.GetMouseButton(0))
            {
                _renderer.material = NormalMaterial;
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
                    float height = (int)(ray.GetPoint(enterPoint).y / CGGridCube.OneCubeHeight) * CGGridCube.OneCubeHeight; // snap to nearest OneCubeHeight// snap to nearest OneCubeHeight
                    height = Mathf.Clamp(height, -CGGridCube.ClampLimit * CGGridCube.HeightToWidthMultiplier, CGGridCube.ClampLimit * CGGridCube.HeightToWidthMultiplier);
                    float currentY = height / 5;// convert world space to grid
                    float heightsDifference = currentY - _previousY; // this is still as a height (2.5 based) not on grid
                    int yDifference = (int)(CGGridCube.HeightToWidthMultiplier * heightsDifference); // convert height to grid coord
                    transform.position += new Vector3(0, yDifference * CGGridCube.OneCubeHeight, 0);
                    _previousY = currentY;

                    foreach (CGGridCube gc in SelectionManager.Instance.Objects)
                    {
                        gc.SetHeight(gc.Height + yDifference);
                    }
                }
            }
        }

        public void Clicked()
        {
            Dragging = true;
            _renderer.material = ClickedMaterial;
            _previousY = (int)(transform.position.y / CGGridCube.OneCubeSize);
        }

        public void Refresh()
        {
            List<CGGridCube> cubes = SelectionManager.Instance.Objects;
            Vector3 sum = Vector3.zero;
            float maxY = float.MinValue;

            foreach (CGGridCube cube in cubes)
            {
                if (maxY < cube.transform.position.y)
                {
                    maxY = cube.transform.position.y;
                }
                sum += cube.transform.position;
            }

            if (cubes.Count != 0)
            {
                gameObject.SetActive(true);
                Vector3 newPos = sum / cubes.Count;
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
