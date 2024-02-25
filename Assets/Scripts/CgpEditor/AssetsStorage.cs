using CgpEditor.LevelEditor;
using UnityEngine;

namespace CgpEditor
{
    public class AssetsStorage : MonoSingleton<AssetsStorage>
    {
        public GameObject Grid;
        public GameObject MeleePrefab;
        public GameObject ProjectilePrefab;
        public GameObject MassPrefab;
        public GameObject StairPrefab;
        public GameObject JumpPadPrefab;

        public GameObject GetPrefab(CGPrefabType type)
        {
            switch (type)
            {
                case CGPrefabType.Melee:
                    return MeleePrefab;
                case CGPrefabType.Projectile:
                    return ProjectilePrefab;
                case CGPrefabType.Mass:
                    return MassPrefab;
                case CGPrefabType.Stairs:
                    return StairPrefab;
                case CGPrefabType.JumpPad:
                    return JumpPadPrefab;
                default:
                    return null;
            }
        } 
    }
}