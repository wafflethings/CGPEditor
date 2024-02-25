using CgpEditor.LevelEditor;
using CgpEditor.LevelEditor.Selection;
using UnityEngine;
using UnityEngine.UI;

namespace CgpEditor.Ux.MainPanel
{
    public class PrefabPanel : MonoBehaviour
    {
        public Button None;
        public Button Melee;
        public Button Projectile;
        public Button Mass;
        public Button Stairs;
        public Button JumpPad;

        private void Start()
        {
            None.onClick.AddListener(() => SelectionManager.Instance.FillSelectionWithPrefab(CGPrefabType.None));
            Melee.onClick.AddListener(() => SelectionManager.Instance.FillSelectionWithPrefab(CGPrefabType.Melee));
            Projectile.onClick.AddListener(() => SelectionManager.Instance.FillSelectionWithPrefab(CGPrefabType.Projectile));
            Mass.onClick.AddListener(() => SelectionManager.Instance.FillSelectionWithPrefab(CGPrefabType.Mass));
            Stairs.onClick.AddListener(() => SelectionManager.Instance.FillSelectionWithPrefab(CGPrefabType.Stairs));
            JumpPad.onClick.AddListener(() => SelectionManager.Instance.FillSelectionWithPrefab(CGPrefabType.JumpPad));
        }
    }
}
