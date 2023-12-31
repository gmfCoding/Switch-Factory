using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ResourceInstance
{
    public GameObject obj;
    public ResourceNodeInfo info;
    public int remaining;
    public int size;

    public void OnDestroy()
    {
        if (obj != null)
            GameObject.Destroy(obj);
        remaining = 0;
        size = 0;
        info = null;
        obj = null;
    }
}
