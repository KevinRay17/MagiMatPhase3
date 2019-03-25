using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFunctions : MonoBehaviour
{
    /// <summary>
    /// Returns a normalized Vector2 based on the int faceDirection used on the player.
    /// 1 = up, 2 = right, 3 = down, 4 = left
    /// </summary>
    /// <param name="faceDirection">Direction</param>
    /// <returns>Normalized Vector2 in one of the cardinal directions</returns>
    public static Vector2 FaceDirectionToVector2(int faceDirection)
    {
        switch (faceDirection)
        {
            case 1:
                return Vector2.up;
            case 2:
                return Vector2.right;
            case 3:
                return Vector2.down;
            case 4:
                return Vector2.left;
            default:
                return Vector2.zero;
        }
    }
    /// <summary>
    /// Returns a float angle between 0 to 360 based on a Vector2 direction.
    /// Angle starts facing upwards at 0 degrees and continues clockwise
    /// </summary>
    /// <param name="direction">Direction</param>
    /// <returns>Float of the angle in degrees</returns>
    public static float Vector2DirectionToAngle(Vector2 direction)
    {
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        angle *= -1;
        if (angle < 0)
        {
            angle += 360;
        }
        return angle;
    }
}
