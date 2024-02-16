using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CgpEditor.LevelEditor.Selection
{
    public static class SelectionModification
    {
        public static CGGridCube[][] GetSelectionBounds()
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;
            
            foreach (EditorObject eo in SelectionManager.Instance.Objects)
            {
                if (eo.GridPosition.x < minX)
                {
                    minX = eo.GridPosition.x;
                }
                
                if (eo.GridPosition.x > maxX)
                {
                    maxX = eo.GridPosition.x;
                }
                
                if (eo.GridPosition.y < minY)
                {
                    minY = eo.GridPosition.y;
                }
                
                if (eo.GridPosition.y > maxY)
                {
                    maxY = eo.GridPosition.y;
                }
            }

            int length = (maxX - minX) + 1;
            int height = (maxY - minY) + 1;
            
            CGGridCube[][] result = new CGGridCube[length][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new CGGridCube[height];
            }

            foreach (CGGridCube gc in CGGrid.CurrentCgGrid.Cubes)
            {
                if (FitsInBounds(gc, minX, maxX, minY, maxY))
                {
                    result[gc.Object.GridPosition.x - minX][gc.Object.GridPosition.y - minY] = gc;
                }
            }

            return result;
        }
        
        private static bool FitsInBounds(CGGridCube cube, int minX, int maxX, int minY, int maxY)
        {
            return (cube.Object.GridPosition.x >= minX && cube.Object.GridPosition.x <= maxX && cube.Object.GridPosition.y >= minY && cube.Object.GridPosition.y <= maxY);
        }
    }
}