using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConsumer : TileEntity, IItemContainer
{
    public string itemName = "iron";
    bool enabled = true;

    public static World world = Game.instance.world;

    public ItemConsumer(TileInfo info) : base(info)
    {

    }

    public override bool IsWalkable()
    {
        return false;
    }

    public ItemStack Remove(ItemFilter filter, int amount)
    {
        return null;
    }

    public ItemStack Remove(ItemStack item)
    {
        return null;
    }

    public bool CanAcceptFrom(IItemContainer from)
    {
        return true;
    }

    public bool CanAdd(ItemStack stack)
    {
        return true;
    }

    public IEnumerable<ItemStack> GetAvailableItems()
    {
        return null;
    }

    public bool TryAdd(ItemStack item, IItemContainer target, out int taken, out int remaining)
    {
        if (!IItemContainer.TryAddDefault(this, item, target, out taken, out remaining))
            return false;
        taken = item.amount;
        remaining = 0;
        item.Virtualise();
        return true;
    }
}
