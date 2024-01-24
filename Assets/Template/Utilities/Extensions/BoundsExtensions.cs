using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsExtensions
{
    public static Vector2 BottomLeft(this Bounds bounds)
    {
        return new Vector2(bounds.min.x, bounds.min.y);
    }

    public static Vector2 UpperLeft(this Bounds bounds)
    {
        return new Vector2(bounds.min.x, bounds.max.y);
    }

    public static Vector2 BottomRight(this Bounds bounds)
    {
        return new Vector2(bounds.max.x, bounds.min.y);
    }

    public static Vector2 UpperRight(this Bounds bounds)
    {
        return new Vector2(bounds.max.x, bounds.max.y);
    }
}