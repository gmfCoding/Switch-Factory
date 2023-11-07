using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int amount;
    public ItemInfo item;
    public Transform transform;

    public Item(ItemInfo info, int count = 1)
    {
        item = info;
        amount = count;
    }

    public Item(Item item)
    {
        this.item = item.item;
        this.amount = item.amount;
    }

    ~Item()
    { 
        if (transform != null)
        {
            Game.instance.Delete(transform.gameObject);
        }
    }

    public bool ExistInWorld()
    {
        return transform != null && !(transform is RectTransform);
    }

    public bool ExistInUI()
    {
        return transform != null && transform is RectTransform;
    }

    
    public void LinkTransform(Transform trans)
    {
        this.transform = trans;
    }

    public void Virtualise()
    {
        if (transform != null)
            GameObject.Destroy(transform.gameObject);
    }
}
