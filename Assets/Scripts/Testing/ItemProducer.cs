using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProducer : TileEntity, ITickable, IItemTransport
{
    public string itemName = "iron";
    bool enabled = true;

    public static World world = Game.instance.world;

    public float rate = 30;

    public int tick;

    public ItemProducer(TileInfo info) : base(info)
    {

    }

    public void Tick()
    {
        if (enabled)
            tick++;
        if ( tick >= rate)
        {
            tick = 0;
            if (world.GetTile(pos + Direction).type == TileBase.Invalid)
                return;
            if (world.GetTileEntity(pos + Direction) is IItemTransport trans)
            {
                trans.Give(Take(), this, out int taken); // Unconvention use of Take
                if (taken > 0)
                    Debug.Log("Spawning Item");
            }
        }
    }

    public Item Take()
    {
        return new Item(Game.instance.GetAsset<ItemInfo>(itemName), 1);
    }

    public void Give(Item item, IItemTransport target, out int taken)
    {
        taken = 0;
    }

    public bool CanAcceptFrom(IItemTransport from)
    {
        return false;
    }

    public override bool IsWalkable()
    {
        return false;
    }
}
