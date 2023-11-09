using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor.Search;
using UnityEngine;

public class ItemFilter
{
    private HashSet<int> itemCodes = new HashSet<int>();

    public void Add(ItemInfo info)
    {
        itemCodes.Add(info.GetHashCode());
    }

    public bool Contains(object info)
    {
        return itemCodes.Contains(info.GetHashCode());
    }

    public ItemStack TryDefaultRemove<T>(ICollection<T> collection, int amount) where T : IProvider<ItemStack>
    {
        bool found = false;
        T conv = default;
        ItemStack item = null;
        foreach(var next in collection)
        {
            if (Contains(next))
            {
                found = true;
                conv = next;
                item = next.Get().Split(amount);
                break;
            }
        }
        if (found == false)
            return null;
        // Whole stack was split
        if (conv.Get() == item)
            collection.Remove(conv);
        return item;
    }
}