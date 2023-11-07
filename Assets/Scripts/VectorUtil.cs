using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtil
{
    public static Vector3 toVec3XZ(this Vector2Int direction)
    {
        return new Vector3(direction.x, 0, direction.y);
    }
    public static Vector3 toVec3XY(this Vector2Int direction)
    {
        return new Vector3(direction.x, direction.y, 0);
    }
    
    public static float Cross(Vector2Int a, Vector2Int b)
    {
        return (a.x * b.y - a.y * b.x);
    }
}

