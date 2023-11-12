using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerEntity : TileEntity, IItemContainer
{
    ResourceNodeInfo resource;

    public MinerEntity(TileInfo info) : base(info)
    {
    }

    public bool CanAcceptFrom(IItemContainer from)
    {
        return false;
    }

    public bool CanAdd(ItemStack stack)
    {
        return false;
    }

    public IEnumerable<ItemStack> GetAvailableItems()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsWalkable()
    {
        throw new System.NotImplementedException();
    }

    public ItemStack Remove(ItemFilter filter, int amount)
    {
        throw new System.NotImplementedException();
    }

    public ItemStack Remove(ItemStack item)
    {
        throw new System.NotImplementedException();
    }

    public bool TryAdd(ItemStack item, IItemContainer target, out int taken, out int remaining)
    {
        throw new System.NotImplementedException();
    }
}
