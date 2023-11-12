using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ConveyorItem : IProvider<ItemStack>
{
    public ItemStack item;
    public float pos;
    public Vector2Int from;

    public ItemStack Get()
    {
        return item;
    }

    public override int GetHashCode()
    {
        return item.GetHashCode();
    }
}

public class Conveyor : TileEntity, IItemContainer, ITickable
{
    public const float speed = 0.028f;
    public const int capacity = 2;

    //public Dictionary<Item, float> items = new Dictionary<Item, float>();
    public List<ConveyorItem> queue = new List<ConveyorItem>();

    public Conveyor(TileInfo info) : base(info)
    {
        ConveyorVis.instance.conveyors.Add(this);
    }

    public virtual void Tick()
    {
        int dequeue = 0;
        int stop = capacity;
        for (int i = 0; i < queue.Count; i++)
        {
            if (queue[i].pos > 1.0f)
                dequeue++;
            if (queue[i].pos <= (1.0 / capacity * stop))
                queue[i].pos += speed;
            stop--;
        }
        TileEntity neighbour = Game.instance.world.GetTileEntity(pos + Direction);
        if (neighbour is IItemContainer target)
        {
            if (!target.CanAcceptFrom(this))
                return;
            for (int i = 0; i < dequeue; i++)
            {
                var took = target.TryAdd(queue[i].item, this, out int taken, out int remaining);
                if (took && remaining == 0)
                {
                    var rem = queue.ElementAt(0);
                    queue.Remove(rem);
                }
                else if (took)
                    queue[i].item = queue[i].item.Split(taken);
            }
        }
    }

    public bool TryAdd(ItemStack given, IItemContainer from, out int taken, out int remaining)
    {
        if (!IItemContainer.TryAddDefault(this, given, from, out taken, out remaining))
            return false;
        if (queue.Count >= capacity)
            return false;
        taken = 1;
        ConveyorItem item = new ConveyorItem() { item = given, pos = 0.0f, from = this.Direction };
        if (from is TileEntity entity)
        {
            item.from = this.pos - entity.pos;
            queue.Add(item);
            taken = 1;
        }
        else
        {
            float newPos = 0.5f;
            if (queue.Count > 0)
                newPos = queue.Last().pos - 1.0f / capacity;
            newPos = Mathf.Max(newPos, 0);
            queue.Insert(queue.Count, item);
            taken = 1;
        }
        return true;
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
    public override void OnEntityDestroyed()
    {
        base.OnEntityDestroyed();
        foreach (var item in queue)
        {
            GameObject.Destroy(item.item.transform.gameObject);
        }
        ConveyorVis.instance.conveyors.Remove(this);
        foreach (var item in queue)
            item.item.Virtualise();
        queue.Clear();
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