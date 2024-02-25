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
        public Button ClearBadButton;

        private void Start()
        {
            None.onClick.AddListener(() => SelectionManager.Instance.FillSelectionWithPrefab(CGPrefabType.None));
            Melee.onClick.AddListener(() => SelectionManager.Instance.FillSelectionWithPrefab(CGPrefabType.Melee));
            Projectile.onClick.AddListener(() => SelectionManager.Instance.FillSelectionWithPrefab(CGPrefabType.Projectile));
            Mass.onClick.AddListener(() => SelectionManager.Instance.FillSelectionWithPrefab(CGPrefabType.Mass));
            Stairs.onClick.AddListener(() => SelectionManager.Instance.FillSelectionWithPrefab(CGPrefabType.Stairs));
            JumpPad.onClick.AddListener(() => SelectionManager.Instance.FillSelectionWithPrefab(CGPrefabType.JumpPad));
            ClearBadButton.onClick.AddListener(ClearBadStairs);
        }

        private void ClearBadStairs()
        {
            foreach (CGGridCube cube in CGGrid.CurrentCgGrid.Cubes)
            {
                Stairs stairs = cube.GetComponentInChildren<Stairs>();
                if ((bool)stairs)
                {
                    if ((stairs.PrimaryErrors && stairs.SecondaryErrors) || !stairs.HasDirection)
                    {
                        CGGrid.CurrentCgGrid.SetPrefab(CGPrefabType.None, cube.GridPosition.x, cube.GridPosition.y);
                    }
                }
            }
        }
    }
}
