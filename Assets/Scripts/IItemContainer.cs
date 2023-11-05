using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemContainer
{
    void Remove(Item item);
    void Add(Item item);

    void Clear();
}
