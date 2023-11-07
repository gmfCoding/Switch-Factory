using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConsumer : TileEntity, IItemTransport
{
    public string itemName = "iron";
    bool enabled = true;

    public static World world = Game.instance.world;

    public ItemConsumer(TileInfo info) : base(info)
    {

    }

    public Item Take()
    {
        return new Item(Game.instance.GetAsset<ItemInfo>(itemName), 1);
    }

    public void Give(Item item, IItemTransport target, out int taken)
    {
        taken = 1;
        item.Virtualise();
    }

    public bool CanAcceptFrom(IItemTransport from)
    {
        return true;
    }

    public override bool IsWalkable()
    {
        return false;
    }
}
