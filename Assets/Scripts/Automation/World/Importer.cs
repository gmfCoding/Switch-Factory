using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Importer : TileEntity, IItemContainer, ITickable
{
    private ImportRegion collectionRegion;

    public List<ItemStack> items = new List<ItemStack>();

    public ImportRegion CollectionRegion { 
        get 
        {
            if (collectionRegion == null)
                collectionRegion = this.obj.GetComponent<ImportRegion>();
            return collectionRegion; 
        }
    }

    public Importer(TileInfo info) : base(info)
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
        foreach (var item in CollectionRegion.grabables)
        {
            items.Add(item);
        }
        CollectionRegion.grabables.Clear();
        return items;
    }

    public override bool IsWalkable()
    {
        return false;
    }

    public ItemStack Remove(ItemFilter filter, int amount)
    {
        GetAvailableItems();
        return filter.TryDefaultRemove<ItemStack>(items, amount);
    }

    public ItemStack Remove(ItemStack item)
    {
        GetAvailableItems();
        if (items.Remove(item))
            return item;
        return null;
    }

    public bool TryAdd(ItemStack item, IItemContainer target, out int taken, out int remaining)
    {
        taken = 0;
        remaining = 0;
        return false;
    }

    public void Tick()
    {
        if (Game.instance.world.GetTileEntity(Direction + pos) is IItemContainer trans)
        {
            if (!trans.CanAcceptFrom(this))
                return;
            GetAvailableItems();
            List<ItemStack> remove = new List<ItemStack>();
            for (int i = 0; i < items.Count; i++)
            {
                var took = trans.TryAdd(items[i], this, out int taken, out int remaining);
                if (took && remaining == 0)
                {
                    remove.Add(items[i]);
                }
                else if (took)
                    items[i] = items[i].Split(taken);
            }
            foreach (var item in remove)
                items.Remove(item);
        }
    }
}
