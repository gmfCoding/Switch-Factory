using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetInfo : ScriptableObject
{
    [Header("Asset Info:")]
    [SerializeField]
    private new string name;
    [SerializeField]
    [TextArea]
    private string description;

    public string Name { get => name; }
    public string Description { get => description; }
}
