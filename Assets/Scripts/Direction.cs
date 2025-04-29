using UnityEngine;

public enum Direction
{
    // rotates clockwise, starting with 'Up'.
    Up = 0, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft,
}

public class DirectionHandler
{
    /// <summary>
    /// Returns direction vector of length 1.
    /// </summary>
    public static Vector2 GetDirectionVector(Direction direction)
    {
        float dg = Mathf.Sqrt(2) / 2f;
        switch (direction)
        {
            case Direction.Up: return Vector2.up;
            case Direction.Down: return Vector2.down;
            case Direction.Left: return Vector2.left;
            case Direction.Right: return Vector2.right;

            case Direction.UpRight: return new Vector2(dg, dg);
            case Direction.UpLeft: return new Vector2(-dg, dg);
            case Direction.DownRight: return new Vector2(dg, -dg);
            case Direction.DownLeft: return new Vector2(-dg, -dg);

            default: return Vector2.up;
        }
    }

    /// <summary>
    /// Returns angle between direction vector and Vector2.right, counted counter-clockwise.
    /// </summary>
    public static float GetDirectionAngle(Direction direction)
    {
        float[] angles = { 0f, -45f, -90f, -135f, 180f, 135f, 90f, 45f};
        return angles[(int)direction];
    }

    public static Vector2 AngleToVector2(float angle, float length = 1f)
    {
        angle *= Mathf.Deg2Rad;
        float x = -Mathf.Sin(angle);
        float y = Mathf.Cos(angle);
        return new Vector2(x, y) * length;
    }
    public static float Vector2ToAngle(Vector2 dir)
    {
        if(dir == Vector2.zero)
        {
            return 0f;
        }
        return Vector2.SignedAngle(Vector2.up, dir);
    }
}