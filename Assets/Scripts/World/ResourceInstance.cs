using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public struct ResourceInstance
{
    public GameObject obj;
    public ResourceInfo info;
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
