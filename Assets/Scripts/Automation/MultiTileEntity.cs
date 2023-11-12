using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MultiTileEntity : TileEntity
{
    public MultiTileEntity parent;

    protected MultiTileEntity(TileInfo info) : base(info)
    {
    }

    public override void OnEntityDestroyed()
    {
        if (destroyed)
            return;
        if (this == parent)
        {
            this.destroyed = true;
            var span = this.Info.span;
            for (int y = 0; y < span.y; y++)
            {
                for (int x = 0; x < span.x; x++)
                {
                    Game.instance.world.ClearTile(pos + Direction.Transform(new Vector2Int(x, y)));
                }
            }
        }
        else
        {
            if (parent != null && !parent.destroyed)
                parent.OnEntityDestroyed();
        }
    }
}
