using UnityEngine;

namespace CgpEditor.LevelEditor
{
    public class SubPrefab : MonoBehaviour
    {
        [HideInInspector] public CGGridCube Cube;

        public virtual void Refresh()
        {
            Cube = GetComponentInParent<CGGridCube>();
        }

        private void Update()
        {
            Vector3 targetPosition = new Vector3(0, CGGridCube.YOffset, 0);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition,
                ((transform.localPosition - targetPosition).sqrMagnitude * 6 + 3) * Time.deltaTime);
        }
    }
}
