using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MachineItem : IProvider<ItemStack>
{
    public ItemStack item;

    public ItemStack Get()
    {
        return item;
    }

    public override int GetHashCode()
    {
        return item.GetHashCode();
    }
}

public class MachineBase : TileEntity, IItemContainer, ITickable
{
    public const float speed = 0.028f;
    public const int capacity = 2;
    ItemRecipeInfo recipe;

    //public Dictionary<Item, float> items = new Dictionary<Item, float>();
    public List<ItemStack> queue = new List<ItemStack>();
    public List<ItemStack> outBuffer = new List<ItemStack>();

    public MachineBase(TileInfo info) : base(info)
    {
        recipe = info.recipes.FirstOrDefault();
    }

    public void CompressInput()
    {
        queue = queue
        .GroupBy(item => item.item)
        .Select(group => new ItemStack(group.Key,group.Sum(item => item.Amount)))
        .ToList();
    }

    public virtual void Tick()
    {
        CompressInput();
        if (recipe != null && queue.Count > 0 && outBuffer.Count == 0)
        {
            if (recipe.TryMake(queue, out ItemStack made))
                outBuffer.Add(made);
        }
        List<ItemStack> remove = new List<ItemStack>();
        foreach (var made in outBuffer)
        {
            TileEntity neighbour = Game.instance.world.GetTileEntity(pos + Direction);
            if (Info.canConveyorOutput && neighbour is IItemContainer target)
            {
                if (!target.CanAcceptFrom(this))
                    return;
                if (target.TryAdd(made, this, out int _, out int remaining2))
                    remove.Add(made);
            }
        }
        foreach (var item in remove)
            outBuffer.Remove(item);
    }

    public bool TryAdd(ItemStack item, IItemContainer from, out int taken, out int remaining)
    {
        if (!IItemContainer.TryAddDefault(this, item, from, out taken, out remaining))
            return false;
        if (queue.Count >= capacity)
            return false;
        item.Virtualise();
        taken = 1;
        queue.Add(item);
        return true;
    }

    public ItemStack Remove(ItemFilter filter, int amount)
    {
        return filter.TryDefaultRemove(outBuffer, amount);
    }

    public ItemStack Remove(ItemStack item)
    {
        var slot = outBuffer.Where(x => x.item == item.item).FirstOrDefault();
        if (slot == null)
            return null;
        outBuffer.Remove(slot);
        return item;
    }

    public override void OnEntityDestroyed()
    {
        base.OnEntityDestroyed();
        foreach (var item in queue)
            item.Virtualise();
        queue.Clear();
        outBuffer.Clear();
    }

    public override bool IsWalkable()
    {
        return false;
    }

    public bool CanAdd(ItemStack stack)
    {
        if (recipe == null)
            return false;
        return stack != null && queue.Count <= recipe.Input.Count;
    }

    public bool CanAcceptFrom(IItemContainer from)
    {
        if (from is Conveyor)
            return Info.canConveyorInput;
        return true;
    }

    public IEnumerable<ItemStack> GetAvailableItems()
    {
        return outBuffer;
    }
}