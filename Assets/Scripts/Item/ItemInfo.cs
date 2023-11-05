using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "item.asset", menuName = "Game/Item", order = 0)]
public class ItemInfo : AssetInfo
{
    public GameObject model;

    public static Dictionary<string, ItemInfo> itemDatabase = new Dictionary<string, ItemInfo>();
    static void LoadItems()
    {
        ItemInfo[] items = Resources.LoadAll<ItemInfo>("Items");
        foreach (var item in items)
        {
            itemDatabase.Add(item.name, item);
        }
    }

    ItemInfo Get(string name)
    {
        if (itemDatabase.ContainsKey(name))
            return itemDatabase[name];
        return null;
    }
}
