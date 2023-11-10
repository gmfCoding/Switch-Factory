using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum ResourceGatherType
{ 
    Manual,
    Machinable,
    Taskable
}

public enum ResourceSpawnType
{
    Single,
    Patch
}

[Serializable]
public struct ResourceSpawnInfo
{
    public ResourceSpawnType type;
    [Range(0, 1000)]
    public float size;
    public float offsetX;
    public float offsetY;
    public float chance;
}

[CreateAssetMenu(fileName = "resource.asset", menuName = "Game/Resource", order = 0)]
public class ResourceInfo : AssetInfo
{
    public bool randomise;
    [Range(0, 8000)]
    public short min;
    [Range(0, 8000)]
    public short max;

    public static List<ResourceInfo> data = new List<ResourceInfo>();
    public static Dictionary<ResourceInfo, byte> id = new Dictionary<ResourceInfo, byte>();

    public ResourceGatherType gatherModes;
    public ResourceSpawnInfo spawnInfo;

    public GameObject Model;
}
