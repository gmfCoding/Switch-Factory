using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;  

public class Splitter : MultiTileEntity, IItemContainer
{
    Vector2Int pair;

    public List<ConveyorItem> queue = new List<ConveyorItem>();

    public Splitter(TileInfo info) : base(info)
    {
    }

    public IItemContainer GetTarget(int index)
    {
        index = index % this.Info.span.x;
        var side = Direction.Side();
        return Game.instance.world.GetTileEntity(pos + side * index) as IItemContainer;
    }

    public void Tick()
    {
        int target = 0;
        int count = queue.Count;
        var stop = Conveyor.capacity;
        for (int i = 0; i < count; i++)
        {
            if (queue[i].pos > 1.0f)
            {
                target++;
                if (GetTarget(target) == null)
                    target = (target + 1) % 2;
                if (GetTarget(target) != null && GetTarget(target).CanAdd(queue[i].item))
                {
                    if (GetTarget(target).TryAdd(queue[i].item, this, out int taken, out int remaining))
                    {
                        queue[i].item = queue[i].item.Split(taken);
                        if (queue[i].item == null) // All of the item was transfered
                            stop++;
                    }
                }
            }
            if (queue[i].pos <= (1.0 / Conveyor.capacity * stop))
                queue[i].pos += Conveyor.speed;
            stop--;
        }
        queue.RemoveAll(x => x == null);
    }

    public override void OnEntityDestroyed()
    {
        base.OnEntityDestroyed();
        Game.instance.world.ClearTile(pair);
    }

    public ItemStack Remove(ItemFilter filter, int amount)
    {
        return filter.TryDefaultRemove<ConveyorItem>(queue, amount);
    }

    public ItemStack Remove(ItemStack item)
    {
        var slot = queue.Where(x => x.item == item).FirstOrDefault();
        if (slot == null)
            return null;
        queue.Remove(slot);
        return item;
    }

    public bool TryAdd(ItemStack item, IItemContainer target, out int taken, out int remaining)
    {
        if (!IItemContainer.TryAddDefault(this, item, target, out taken, out remaining))
            return false;
        queue.Add(new ConveyorItem() { item = item, pos = 0f, from = Vector2Int.zero });
        taken = item.amount;
        remaining = 0;
        return true;
    }

    public override bool IsWalkable()
    {
        return true;
    }

    public bool CanAdd(ItemStack stack)
    {
        return stack != null && queue.Count < Conveyor.capacity;
    }

    public bool CanAcceptFrom(IItemContainer from)
    {
        return true;
    }

    public IEnumerable<ItemStack> GetAvailableItems()
    {
        return queue.Select(x => x.item);
    }
}
