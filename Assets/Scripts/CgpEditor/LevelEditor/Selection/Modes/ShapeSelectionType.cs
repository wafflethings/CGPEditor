using System.Collections.Generic;
using UnityEngine;

namespace CgpEditor.LevelEditor.Selection.Modes
{
    public class ShapeSelectionType : SelectionType
    {
        protected List<CGGridCube> _points = new List<CGGridCube>();

        public override void SelectionUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _points.Clear();
            }
        }
    }
}