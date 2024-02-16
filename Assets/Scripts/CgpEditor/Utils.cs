using UnityEngine;

namespace CgpEditor
{
    public static class Utils
    {
        // https://forum.unity.com/threads/limiting-rotation-with-mathf-clamp.171294/
        public static float ClampAngle(float angle, float min, float max)
        {
            if (min < 0 && max > 0 && (angle > max || angle < min))
            {
                angle -= 360;
                if (angle > max || angle < min)
                {
                    if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                    else return max;
                }
            }
            else if(min > 0 && (angle > max || angle < min))
            {
                angle += 360;
                if (angle > max || angle < min)
                {
                    if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                    else return max;
                }
            }
 
            if (angle < min) return min;
            else if (angle > max) return max;
            else return angle;
        }
        
        
        //https://discussions.unity.com/t/rect-contains-cant-accept-rects-with-a-negative-width-and-height/47282/4
        public static Rect FixNegativeSize (this Rect rectOld) 
        {
            var rect = new Rect(rectOld);

            if (rect.width < 0) {
                rect.x += rect.width;
                rect.width = Mathf.Abs(rect.width);
            }

            if (rect.height < 0) {
                rect.y += rect.height;
                rect.height = Mathf.Abs(rect.height);
            }

            return rect;
        }
    }
}