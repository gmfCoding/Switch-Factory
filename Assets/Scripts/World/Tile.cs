using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum TileBase
{
    Invalid, Floor, Wall, Lava
}

[System.Serializable]
public struct Tile
{
    public static Tile Empty = new Tile { type = TileBase.Invalid };
    public static Tile Floor = new Tile { type = TileBase.Floor };
    public static Tile Lava = new Tile { type = TileBase.Lava };

    public short groundTexture;

    public ResourceInstance resource;

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
        return HashCode.Combine(resource.info, tile, type, entity);
    }

    public override string ToString()
    {
        return $"type:{type}\n" +
            $"resource:{(resource.info != null ? resource.info.Name : "none")}\n" +
            $"entity:{(entity != null ? entity.Info.Name : "none")}\n" +
            $"tile:{(tile != null ? tile.Name : "none")}\n";
    }
}
