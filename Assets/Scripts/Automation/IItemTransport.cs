using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemTransport
{
    public Item Take();
    public void Give(Item item, IItemTransport target, out int taken);
    bool CanAcceptFrom(IItemTransport from);
}
