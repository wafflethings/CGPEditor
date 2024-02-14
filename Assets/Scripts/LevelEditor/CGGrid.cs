﻿using System.Collections.Generic;
using CgpEditor.IO;
using UnityEngine;

namespace CgpEditor.LevelEditor
{
    public class CGGrid : MonoBehaviour
    {
        public static CGGrid CurrentCgGrid;
        
        [Header("Templates")]
        public GameObject CubeTemplate;
        public GameObject StairTemplate;
        public GameObject JumpPadTemplate;
        [Header("Data")]
        public int Length;
        public int[,] Heights;
        public CGPrefabType[,] Prefabs;
        public CGGridCube[,] Cubes;
        
        public static CGGrid CreateGrid(int length)
        {
            if (CurrentCgGrid != null)
            {
                Selection.Instance.Objects.Clear();
                Destroy(CurrentCgGrid.gameObject);
            }

            CGGrid cgGrid = Instantiate(AssetsStorage.Instance.Grid).GetComponent<CGGrid>();
            CurrentCgGrid = cgGrid;
            cgGrid.Heights = new int[length,length];
            cgGrid.Prefabs = new CGPrefabType[length, length];
            for (int x = 0; x < length; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    cgGrid.Prefabs[x, z] = CGPrefabType.None;
                }
            }
            cgGrid.Length = length;
            cgGrid.Build();
            return cgGrid;
        }



        private void Build()
        {
            Cubes = new CGGridCube[Length, Length];
            for (int x = 0; x < Length; x++)
            {
                for (int z = 0; z < Length; z++)
                {
                    GameObject newCube = Instantiate(CubeTemplate, new Vector3(x, Heights[x, z], z) * CGGridCube.OneCubeSize, Quaternion.identity);
                    newCube.transform.parent = transform;
                    newCube.GetComponent<EditorObject>().GridPosition = new Vector2Int(x, z);
                    Cubes[x, z] = newCube.GetComponent<CGGridCube>();
                }
            }

            transform.position -= CGGridCube.OneCubeSize * Length * new Vector3(0.5f, 0, 0.5f);
        }
    }
}
