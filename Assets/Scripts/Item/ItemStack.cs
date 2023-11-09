using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public ItemInfo item;
    public int amount;

    public Transform transform;

    public int Amount
    {
        get => amount; set
        {
            if (value == 0)
                Virtualise();
            amount = value;
        }
    }

    public ItemStack(ItemInfo info, int count = 1)
    {
        item = info;
        Amount = count;
    }

    public ItemStack(ItemStack item)
    {
        this.item = item.item;
        this.Amount = item.Amount;
    }

    public ItemStack Clone()
    {
        return new ItemStack(this);
    }

    ~ItemStack()
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

    public void Virtualise(bool hide = false)
    {
        if (transform != null)
            GameObject.Destroy(transform.gameObject);
    }

    public void SetInvisible()
    {
        if (transform != null)
            transform.gameObject.SetActive(false);
    }

    public void SetVisible()
    {
        if (transform != null)
            transform.gameObject.SetActive(true);
    }

    public ItemStack Split(int amount)
    {
        if (amount > this.Amount)
            amount = this.Amount;
        if (amount > 0 && this.Amount > amount)
        {
            var item = new ItemStack(this.item, this.Amount - amount);
            this.Amount -= amount;
            return item;
        }
        return this;
    }

    public bool TryMerge(ItemStack item, out int taken, out int remaining)
    {
        taken = 0;
        remaining = 0;
        int can = CheckMerge(item);
        if (can == 0)
            return false;
        taken = can;
        remaining = item.Amount - taken;
        item.Amount -= can;
        this.Amount += can;
        return true;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The amount that could be merged</returns>
    public int CheckMerge(ItemStack item)
    {
        if (item.item != this.item)
            return 0;
        if (this.Amount + item.Amount <= this.item.maxStack)
            return item.Amount;
        else
            return item.Amount - this.Amount;
    }
}
