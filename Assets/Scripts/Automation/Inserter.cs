using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inserter : TileEntity, ITickable
{
    private Vector2Int src;
    private Vector2Int dst;

    internal int tick;

    public List<ItemStack> filter;

    public new InserterInfo Info => base.Info as InserterInfo;
    public Vector2Int Source { get => src; set => src = value; }
    public Vector2Int Target { get => dst; set => dst = value; }

    ItemStack item;

    public Inserter(TileInfo info) : base(info)
    {

    }

    public void Tick()
    {
        src = this.Direction;
        dst = -this.Direction;
        var srcTile = Game.instance.world.GetTileEntity(this.pos + src) as IItemContainer;
        var dstTile = Game.instance.world.GetTileEntity(this.pos + dst) as IItemContainer;
        if (srcTile == null || dstTile == null)
            return;
        tick++;
        if (tick == Info.cycle / 2 && item == null)
        {
            var take = srcTile.GetAvailableItems().Where(x => dstTile.CanAdd(x)).FirstOrDefault();
            if (take != null)
                item = srcTile.Remove(take);
        }
        else if (tick > Info.cycle && item != null)
        {
            tick = 0;
            if (dstTile.TryAdd(item, srcTile, out int taken, out int remaining))
            {
                item = item.Clone();
                item.Amount = remaining;
            }
            if (item.Amount <= 0)
                item = null;
        }
        else if (tick > Info.cycle)
        {
            tick = 0;
        }
    }

    public override bool IsWalkable()
    {
        return true;
    }
}
