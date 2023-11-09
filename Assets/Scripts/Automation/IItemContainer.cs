using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemContainer
{
    public ItemStack Remove(ItemFilter filter, int amount);
    public ItemStack Remove(ItemStack item);

    /// <summary>
    /// Attempts to send an item.
    /// <br/>
    /// If any amount of the item is accepted the ownership of the entire item will belong to the new container.<br/>
    /// Callee should use this to determine if they should use ItemStack.Split(taken) or similar.
    /// </summary>
    /// <param name="item"> The item instance to send.</param>
    /// <param name="target">The sender interface.</param>
    /// <param name="taken">The amount the transport capable took, undefined when <paramref name="return"/> == false.</param>
    /// <param name="remaining">The amount remaining in <paramref name="item"/>, undefined when <paramref name="return"/> == false.</param>
    /// <returns> <c>true</c> if the item was accepted, <c>false</c> if the item was not.</returns>
    /// 
    public bool TryAdd(ItemStack item, IItemContainer target, out int taken, out int remaining);

    public static bool TryAddDefault(IItemContainer destination, ItemStack item, IItemContainer source, out int taken, out int remaining)
    {
        taken = 0;
        remaining = 0;
        if (item != null)
            return true;
        return destination.CanAdd(item);
    }

    bool CanAcceptFrom(IItemContainer from);

    public bool CanAdd(ItemStack stack);

    public IEnumerable<ItemStack> GetAvailableItems();
}
