using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum TileType
{
    Floor, Wall, Lava
}

public struct Tile
{
    public short texture;

    public TileType type;

    public TileEntity entity;

    public bool IsWalkable()
    {
        if (type == TileType.Wall || type == TileType.Lava)
            return true;
        if (entity == null)
            return false;
        return entity.IsWalkable();
    }
}
