using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "inserter.asset", menuName = "Game/Tiles/Inserter", order = 0)]
public class InserterInfo : BuildableTileInfo
{
    [Header("Inserter")]
    [Tooltip("Time in ticks it takes for it to rotate to it's target.")]
    public int cycle;
    [Tooltip("The amount of items it can hold at once.")]
    public int capacity;
}
