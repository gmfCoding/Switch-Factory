using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
enum ResourceGatherType
{ 
    Manual,
    Machinable,
    Taskable
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

    ResourceGatherType gatherModes;

    public GameObject Model;
}
