using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public class Conveyor : TileEntity, IItemTransport, ITickable
{
    public Vector2Int direction;
    const float speed = 0.014f;
    const int capacity = 8;

    public Dictionary<Item, float> items = new Dictionary<Item, float>();
    public List<Item> queue = new List<Item>();

    public Conveyor()
    {
        ConveyorVis.instance.conveyors.Add(this);
    }

    public void Tick()
    {
        int dequeue = 0;
        int stop = capacity;
        foreach (var key in queue)
        {
            if (items[key] > 1.0f)
                dequeue++;
            if (items[key] <= (1.0 / capacity * stop))
                items[key] += speed;
            stop--;
        }
        TileEntity neighbour = Game.instance.world.GetTileEntity(pos + direction);
        if (neighbour is IItemTransport target)
        {
            if (!target.CanAcceptFrom(this))
                return;
            for (int i = 0; i < dequeue; i++)
            {
                target.Give(queue[i], this, out int taken);
                if (taken >= queue[i].amount)
                {
                    var rem = queue.ElementAt(0);
                    queue.Remove(rem);
                    items.Remove(rem);
                }
                else
                    queue[i].amount -= taken;
            }
        }
    }

    public override bool IsWalkable()
    {
        return true;
    }

    public Item Take()
    {
        Item item = null;
        foreach (var key in queue)
        {
            if (items[key] < 0.8f)
                item = key;
            else
                break;
        }
        if (item == null)
            return null;
        queue.Remove(item);
        items.Remove(item);
        return item;
    }

    public void Give(Item item, IItemTransport from, out int taken)
    {
        taken = 0;
        if (queue.Count >= capacity)
            return;
        taken = 1;
        if (from is Conveyor)
        {
            queue.Add(item);
            items.Add(item, 0);
            item.amount--;
        }
        else
        {
            float newPos = 0.0f;
            if (items.Count > 0)
                newPos = items.Last().Value - 1.0f / capacity;
            newPos = Mathf.Max(newPos, 0);
            queue.Insert(queue.Count, item);
            items.Add(item, 0);
            item.amount--;
        }
    }

    public bool CanAcceptFrom(IItemTransport from)
    {
        return true;
    }
}