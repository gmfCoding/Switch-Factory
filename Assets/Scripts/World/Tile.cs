using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileBase
{
    Invalid, Floor, Wall, Lava
}

public struct Tile
{
    public static Tile Empty = new Tile { type = TileBase.Invalid };
    public static Tile Floor = new Tile { type = TileBase.Floor };
    public static Tile Lava = new Tile { type = TileBase.Lava };

    public short groundTexture;

    public short resourceID;
    public short resources;

    public TileInfo tile;

    public TileBase type;

    public TileEntity entity;

    public bool IsWalkable()
    {
        if (type == TileBase.Wall || type == TileBase.Lava)
            return true;
        if (entity == null)
            return false;
        return entity.IsWalkable();
    }

    public static bool operator ==(Tile lhs, Tile rhs)
    {
        if (rhs.type == TileBase.Invalid)
            return lhs.type == TileBase.Invalid;
        return lhs.tile == rhs.tile;
    }

    public static bool operator !=(Tile lhs, Tile rhs)
    {
        return !(lhs == rhs);
    }

    public bool IsEmpty()
    {
        return entity == null;
    }

    public bool IsValid()
    {
        return this != Tile.Empty;
    }

    public override bool Equals(object obj)
    {
        if (obj is Tile tile)
        {
            if (tile.type == TileBase.Invalid && this.type == TileBase.Invalid)
                return true;
            return type == tile.type &&
               groundTexture == tile.groundTexture &&
               EqualityComparer<TileInfo>.Default.Equals(this.tile, tile.tile) &&
               EqualityComparer<TileEntity>.Default.Equals(entity, tile.entity);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(resourceID, tile, type, entity);
    }
}
