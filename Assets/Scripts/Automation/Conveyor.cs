using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConveyorItem
{
    public Item item;
    public float pos;
    public Vector2Int prev_dir;
}

public class Conveyor : TileEntity, IItemTransport, ITickable
{
    const float speed = 0.028f;
    const int capacity = 2;

    //public Dictionary<Item, float> items = new Dictionary<Item, float>();
    public List<ConveyorItem> queue = new List<ConveyorItem>();

    public Conveyor(TileInfo info) : base(info)
    {
        Debug.Log("Created new Conveyor");
        ConveyorVis.instance.conveyors.Add(this);
    }

    public void Tick()
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
        if (neighbour is IItemTransport target)
        {
            if (!target.CanAcceptFrom(this))
                return;
            for (int i = 0; i < dequeue; i++)
            {
                target.Give(queue[i].item, this, out int taken);
                if (taken >= queue[i].item.amount)
                {
                    var rem = queue.ElementAt(0);
                    queue.Remove(rem);
                    queue.Remove(rem);
                }
                else
                    queue[i].item.amount -= taken;
            }
        }
    }

    public override bool IsWalkable()
    {
        return true;
    }

    public Item Take()
    {
        ConveyorItem item = null;
        for (int i = 0; i < queue.Count; i++)
        {
            if (queue[i].pos < 0.8f)
                item = queue[i];
            else
                break;
        }
        if (item == null)
            return null;
        queue.Remove(item);
        queue.Remove(item);
        return item.item;
    }

    public void Give(Item given, IItemTransport from, out int taken)
    {
        taken = 0;
        if (queue.Count >= capacity)
            return;
        taken = 1;
        ConveyorItem item = new ConveyorItem() {  item = given, pos = 0.0f, prev_dir = this.Direction };
        if (from is Conveyor conv)
        {
            item.prev_dir = conv.Direction;
            queue.Add(item);
            taken = 1;
        }
        else
        {
            float newPos = 0.0f;
            if (queue.Count > 0)
                newPos = queue.Last().pos - 1.0f / capacity;
            newPos = Mathf.Max(newPos, 0);
            queue.Insert(queue.Count, item);
            taken = 1;
        }
    }

    public override void OnEntityDestroyed()
    {
        base.OnEntityDestroyed();
        foreach (var item in queue)
        {
            GameObject.Destroy(item.item.transform.gameObject);
        }
        ConveyorVis.instance.conveyors.Remove(this);
        queue.Clear();
    }

    public bool CanAcceptFrom(IItemTransport from)
    {
        return true;
    }
}