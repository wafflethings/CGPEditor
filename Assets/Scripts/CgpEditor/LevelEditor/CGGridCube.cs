using UnityEngine;

namespace CgpEditor.LevelEditor
{
    public class CGGridCube : MonoBehaviour
    {
        public const int OneCubeSize = 5;
        public const int YOffset = 5 * OneCubeSize;

        public EditorObject Object
        {
            get
            {
                if (!(bool)_object)
                {
                    _object = GetComponent<EditorObject>();
                }

                return _object;
            }
        }
        private EditorObject _object;

        public int Height
        {
            get => CGGrid.CurrentCgGrid.Heights[Object.GridPosition.x, Object.GridPosition.y];
            set => CGGrid.CurrentCgGrid.Heights[Object.GridPosition.x, Object.GridPosition.y] = value;
        }

        private void Start()
        {
            RefreshPosition();
        }
        
        public void SetHeight(int targetY)
        {
            Height = targetY;
            RefreshPosition();
        }

        public void RefreshPosition()
        {
            if (Object == null)
            {
                Debug.LogWarning($"Somehow CGGridCube {gameObject.name} has a null EditorObject; destroying??");
                Destroy(gameObject);
                return;
            }
            
            transform.position = new Vector3(transform.position.x, Height * OneCubeSize, transform.position.z);
        }
    }
}