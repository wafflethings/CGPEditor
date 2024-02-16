using CgpEditor.LevelEditor.Selection;
using UnityEngine;

namespace CgpEditor.LevelEditor
{
    public class EditorObject : MonoBehaviour
    {
        public Vector2Int GridPosition;
        public Material[] DefaultMaterials;
        public Material[] SelectedMaterials;
        public MeshRenderer Renderer;
        
        public bool IsSelected => SelectionManager.Instance.Objects.Contains(this);
        
        public void Select()
        {
            Renderer.materials = SelectedMaterials;
        }

        public void Deselect()
        {
            Renderer.materials = DefaultMaterials;
        }
    }
}