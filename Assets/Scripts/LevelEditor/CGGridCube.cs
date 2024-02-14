using UnityEngine;

namespace CgpEditor.LevelEditor
{
    public class CGGridCube : MonoBehaviour, IClickable
    {
        public const int OneCubeSize = 5;
        public const int YOffset = 5 * OneCubeSize;
        public EditorObject Object { get; private set; }

        public int Height
        {
            get => CGGrid.CurrentCgGrid.Heights[Object.GridPosition.x, Object.GridPosition.y];
            set => CGGrid.CurrentCgGrid.Heights[Object.GridPosition.x, Object.GridPosition.y] = value;
        }

        private void Start()
        {
            Object = GetComponent<EditorObject>();
            RefreshPosition();
        }
        
        public void Clicked()
        {
            
        }
        
        public void SetHeight(int targetY)
        {
            Height = targetY;
            RefreshPosition();
        }

        public void RefreshPosition()
        {
            transform.position = new Vector3(transform.position.x, Height * OneCubeSize, transform.position.z);
        }
    }
}