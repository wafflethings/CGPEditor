using System.Collections.Generic;
using CgpEditor.LevelEditor.Selection;
using UnityEngine;

namespace CgpEditor.LevelEditor
{
    public class CGGridCube : MonoBehaviour
    {
        public const float OneCubeSize = 5;
        public const float OneCubeHeight = OneCubeSize / 2f;
        public const float HeightToWidthMultiplier = OneCubeSize / OneCubeHeight;
        public const float YOffset = 5 * OneCubeSize;
        public const int ClampLimit = 50;
        public Vector2Int GridPosition;
        public Material[] DefaultMaterials;
        public Material[] SelectedMaterials;
        public Material[] PaintingMaterials;
        public MeshRenderer Renderer;
        private GameObject _currentPrefabObject;
        private Vector3 _targetPosition;

        public int Height
        {
            get => CGGrid.CurrentCgGrid.Heights[GridPosition.x, GridPosition.y];
            private set => CGGrid.CurrentCgGrid.Heights[GridPosition.x, GridPosition.y] = Mathf.Clamp(value, -ClampLimit, ClampLimit);
        }

        private void Start()
        {
            RefreshPosition();
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, ((transform.position - _targetPosition).sqrMagnitude * 3 + 3) * Time.deltaTime);
        }

        public void SetHeight(int targetY)
        {
            Height = targetY;
            RefreshPosition();
        }

        public IEnumerable<CGGridCube> GetTouching()
        {
            for (int x = -1; x <= 1; x++)
            {
                int totalX = GridPosition.x + x;
                if (totalX < 0 || totalX > CGGrid.CurrentCgGrid.Length - 1)
                {
                    continue;
                }

                for (int y = -1; y <= 1; y++)
                {
                    int totalY = GridPosition.y + y;
                    if (totalY < 0 || totalY > CGGrid.CurrentCgGrid.Length - 1)
                    {
                        continue;
                    }

                    yield return CGGrid.CurrentCgGrid.Cubes[totalX, totalY];
                }
            }
        }

        public void RefreshPosition()
        {
            _targetPosition = new Vector3(transform.position.x, Height * OneCubeHeight, transform.position.z);

            foreach (CGGridCube cube in GetTouching())
            {
                cube.RefreshPrefab();
            }
        }

        public void RefreshPrefab()
        {
            SetPrefabVisuals(CGGrid.CurrentCgGrid.Prefabs[GridPosition.x, GridPosition.y]);
        }

        public void SetPrefabVisuals(CGPrefabType prefab)
        {
            bool shouldResetPosition = true;

            if (_currentPrefabObject != null)
            {
                shouldResetPosition = prefab != CGGrid.CurrentCgGrid.Prefabs[GridPosition.x, GridPosition.y];
                Destroy(_currentPrefabObject);
            }

            if (prefab == CGPrefabType.None)
            {
                return;
            }

            _currentPrefabObject = Instantiate(AssetsStorage.Instance.GetPrefab(prefab), transform, true);
            _currentPrefabObject.transform.localPosition = shouldResetPosition ? Vector3.zero : new Vector3(0, YOffset, 0);
            _currentPrefabObject.GetComponent<SubPrefab>()?.Refresh();
        }

        public void Select()
        {
            Renderer.materials = SelectedMaterials;
        }

        public void Deselect()
        {
            Renderer.materials = DefaultMaterials;
        }

        public void Painting()
        {
            Renderer.materials = PaintingMaterials;
        }

        public void StopPainting()
        {
            if (SelectionManager.Instance.Objects.Contains(this))
            {
                Select();
            }
            else
            {
                Deselect();
            }
        }
    }
}
