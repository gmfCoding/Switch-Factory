using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemTransport
{
    public Item Take();

    /// <summary>
    /// Attempts to send an item.
    /// </summary>
    /// <param name="item">The item instance to send</param>
    /// <param name="target">The transport capable that sent it</param>
    /// <param name="taken">The amount the transport capable took</param>
    public void Give(Item item, IItemTransport target, out int taken);
    bool CanAcceptFrom(IItemTransport from);
}
