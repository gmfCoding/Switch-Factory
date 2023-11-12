using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public static Vector2Int Side(this Vector2Int v)
    {
        return new (v.y, v.x);
    }

    public static Vector2Int Transform(this Vector2Int dir, Vector2Int v)
    {
        if (dir.y == 1)
            return v;
        else if (dir.x == 1)
            return new(v.y, v.x);
        else if (dir.y == -1)
            return new(-v.x, -v.y);
        else if (dir.x == -1)
            return new(-v.y, -v.x);
        throw new Exception($"Vector2Int.Transform: cannot transform {v} by {dir}");
    }

    public static float Cross(Vector2Int a, Vector2Int b)
    {
        return (a.x * b.y - a.y * b.x);
    }


    public static Vector3 XYtoXZ(this Vector3 xy)
    {
        return new Vector3(xy.x, 0, xy.y);
    }

    public static Vector2Int WorldToTile(this Vector3 v)
    {
        return new Vector2Int(Mathf.FloorToInt(v.x + World.Width / 2), Mathf.FloorToInt(v.z + World.Height / 2));
    }

    public static Vector3 TileToWorld(this Vector3 v)
    {
        return v - new Vector3(World.Width / 2, 0, World.Height / 2);
    }

    public static Vector3 TileToWorld(this Vector2Int v)
    {
        return (v - new Vector2Int(World.Width / 2, World.Height / 2)).toVec3XZ();
    }
}

