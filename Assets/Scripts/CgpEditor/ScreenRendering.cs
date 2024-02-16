using UnityEngine;

namespace CgpEditor
{
    public static class ScreenRendering
    {
        public static void DrawRectBetween2Points(Vector2 p1, Vector2 p2, Color colour)
        {
            Rect rect = new Rect()
            {
                xMin = p2.x,
                xMax = p1.x,
                yMin = p2.y,
                yMax = p1.y
            };
            
            DrawRect(rect, colour);
        }
        
        public static void DrawRect(Rect rect, Color colour)
        {
            GUI.color = colour;
            GUI.DrawTexture(rect, new Texture2D(1,1));
        }
    }
}