using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum ResourceGatherType
{ 
    Manual = 1,
    Machinable = 2,
    Taskable = 4
}

public enum ResourceGroup
{
    Single,
    Patch
}

[Serializable]
public struct ResourceSpawnInfo
{
    public ResourceGroup type;
    [Range(0, 1000)]
    public float size;
    public float offsetX;
    public float offsetY;
    public float chance;
}

[CreateAssetMenu(fileName = "resource.asset", menuName = "Game/Resource", order = 0)]
public class ResourceNodeInfo : AssetInfo
{
    public bool randomise;
    [Range(0, 8000)]
    public short min;
    [Range(0, 8000)]
    public short max;

    public static List<ResourceNodeInfo> data = new List<ResourceNodeInfo>();
    public static Dictionary<ResourceNodeInfo, byte> id = new Dictionary<ResourceNodeInfo, byte>();

    public ResourceGatherType gatherModes;
    public ResourceSpawnInfo spawnInfo;

    public ResourceNodeInstance Model;
}
