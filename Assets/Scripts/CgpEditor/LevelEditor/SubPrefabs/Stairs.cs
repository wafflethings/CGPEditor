using UnityEngine;

namespace CgpEditor.LevelEditor
{
    public class Stairs : SubPrefab
    {
        public GameObject PrimaryStairs;
        public GameObject SecondaryStairs;
        public Material NormalMaterial;
        public Material ErroringMaterial;
        private MeshRenderer _primaryRenderer;
        private MeshRenderer _secondaryRenderer;

        public override void Refresh()
        {
            base.Refresh();

            _primaryRenderer = PrimaryStairs.GetComponentInChildren<MeshRenderer>(true);
            _secondaryRenderer = SecondaryStairs.GetComponentInChildren<MeshRenderer>(true);

            PrimaryStairs.SetActive(false);
            SecondaryStairs.SetActive(false);

            int differencePrimary;
            int differenceSecondary;
            bool hasDirection = false;

            if (CheckIfStairsShouldReach(new Vector2Int(0, 1), out differencePrimary))
            {
                PrimaryStairs.SetActive(true);
                PrimaryStairs.transform.forward = transform.forward;
                hasDirection = true;
            }
            else if (CheckIfStairsShouldReach(new Vector2Int(0, -1), out differencePrimary))
            {
                PrimaryStairs.SetActive(true);
                PrimaryStairs.transform.forward = -transform.forward;
                hasDirection = true;
            }

            if (CheckIfStairsShouldReach(new Vector2Int(1, 0), out differenceSecondary))
            {
                SecondaryStairs.SetActive(true);
                SecondaryStairs.transform.forward = transform.right;
                hasDirection = true;
            }
            else if (CheckIfStairsShouldReach(new Vector2Int(-1, 0), out differenceSecondary))
            {
                SecondaryStairs.SetActive(true);
                SecondaryStairs.transform.forward = -transform.right;
                hasDirection = true;
            }

            _primaryRenderer.material = differencePrimary > 2 || differencePrimary < 1f ? ErroringMaterial : NormalMaterial;
            _secondaryRenderer.material = differenceSecondary > 2 || differenceSecondary < 1f ? ErroringMaterial : NormalMaterial;

            if (!hasDirection)
            {
                PrimaryStairs.SetActive(true);
                _primaryRenderer.material = ErroringMaterial;
            }

            if (Mathf.Approximately(1, differencePrimary))
            {
                PrimaryStairs.transform.localScale = Vector3.one;
            }

            if (Mathf.Approximately(2, differencePrimary))
            {
                PrimaryStairs.transform.localScale = new Vector3(1, 2, 1);
            }

            if (Mathf.Approximately(1, differenceSecondary))
            {
                SecondaryStairs.transform.localScale = Vector3.one;
            }

            if (Mathf.Approximately(2, differenceSecondary))
            {
                SecondaryStairs.transform.localScale = new Vector3(1, 2, 1);
            }
        }

        private bool CheckIfStairsShouldReach(Vector2Int direction, out int heightDifference)
        {
            Vector2Int targetLocation = Cube.GridPosition + direction; ;
            if (targetLocation.x < 0 || targetLocation.x > CGGrid.CurrentCgGrid.Length-1 || targetLocation.y < 0 || targetLocation.y > CGGrid.CurrentCgGrid.Length-1)
            {
                heightDifference = 0;
                return false;
            }
            CGGridCube otherCube = CGGrid.CurrentCgGrid.Cubes[targetLocation.x, targetLocation.y];
            heightDifference = otherCube.Height - Cube.Height;
            return otherCube.Height > Cube.Height;
        }
    }
}
