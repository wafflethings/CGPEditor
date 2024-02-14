using UnityEngine;

namespace CgpEditor.LevelEditor
{
    public class EditorObject : MonoBehaviour, IClickable
    {
        public Vector2Int GridPosition;
        public Material[] DefaultMaterials;
        public Material[] SelectedMaterials;
        public MeshRenderer Renderer;
        
        public bool IsSelected => Selection.Instance.Objects.Contains(this);
        
        public void Select()
        {
            Renderer.materials = SelectedMaterials;
        }

        public void Deselect()
        {
            Renderer.materials = DefaultMaterials;
        }

        public void Clicked()
        {
            Selection.Instance.SelectObject(this, true);
        }
    }
}