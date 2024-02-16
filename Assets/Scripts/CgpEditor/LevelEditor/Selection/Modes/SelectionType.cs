using UnityEngine;
using UnityEngine.EventSystems;

namespace CgpEditor.LevelEditor.Selection.Modes
{
    public class SelectionType : MonoBehaviour
    {
        public bool IsActive => SelectionManager.Instance.CurrentSelection == this && !Gizmo.Instance.Dragging;
        public bool OnUi => EventSystem.current.IsPointerOverGameObject();
        
        public virtual void SelectionUpdate()
        {
            
        }
        
        private void Update()
        {
            if (!IsActive)
            {
                enabled = false;
            }
        }
    }
}