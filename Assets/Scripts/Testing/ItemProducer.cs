using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemProducer : TileEntity, ITickable, IItemContainer
{
    public string itemName = "iron_ore";
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
            if (world.GetTileEntity(pos + Direction) is IItemContainer trans)
            {
                trans.TryAdd(Remove(null, 1), this, out _, out _); // Unconvention use of Take
            }
        }
    }

    public void TryAdd(ItemStack item, IItemContainer target, out int taken)
    {
        taken = 0;
    }

    public bool CanAcceptFrom(IItemContainer from)
    {
        return false;
    }

    public override bool IsWalkable()
    {
        return false;
    }

    public ItemStack Remove(ItemFilter filter, int amount)
    {
        var item = Game.instance.GetAsset<ItemInfo>(itemName);
        if (filter == null || filter.Contains(item))
            return Remove(null);
        return null;
    }

    public ItemStack Remove(ItemStack item)
    {
        return new ItemStack(Game.instance.GetAsset<ItemInfo>(itemName), 1);
    }

    public bool TryAdd(ItemStack item, IItemContainer target, out int taken, out int remaining)
    {
        taken = 0;
        remaining = item != null ? item.Amount : 0;
        return false;
    }

    public bool CanAdd(ItemStack stack)
    {
        return false;
    }

    public IEnumerable<ItemStack> GetAvailableItems()
    {
        return new List<ItemStack>() { Remove(null, 1) };
    }
}
