using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "item.asset", menuName = "Game/Item", order = 0)]
public class ItemInfo : AssetInfo
{
    [Header("Item Info:")]
    [SerializeField]
    private GameObject model;

    public int maxStack = 100;

    public GameObject Model { get => model; }
}