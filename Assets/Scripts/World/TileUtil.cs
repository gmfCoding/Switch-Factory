using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUtil
{
    private static Vector2Int[] rotations = new Vector2Int[]
    {
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.right,
    };

    public enum TileTransform
    { 
        Pure,
        Centric,
        CentricOffset
    }

    public static Vector2Int IndexTileRotaiton(int index)
    {
        index = (4 - (-index % 4)) % 4;
        return rotations[index];
    }

    public static int TileRotationIndex(Vector2Int rotation)
    {
        for (int i = 0; i < rotations.Length; i++)
        {
            if (rotations[i] == rotation)
                return i;
        }
        return 0;
    }

    public static Vector2Int WorldspaceToTile(Vector3 point)
    {
        return point.WorldToTile();
    }

    public static Vector3 TileToWorldspace(Vector2Int pos, TileTransform mode)
    {
        if (mode != TileTransform.Pure)
            return (pos.toVec3XZ() + new Vector3(.5f, mode == TileTransform.CentricOffset ? 0.5f : 0f, 0.5f)).TileToWorld();
        return pos.toVec3XZ().TileToWorld();
    }
}
