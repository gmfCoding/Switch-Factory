using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tile.asset", menuName = "Game/Tiles/BuildableTile", order = 0)]
public class BuildableTileInfo : TileInfo
{
    [SerializeField]
    private RecipeCost cost;

    public RecipeCost Cost { get => cost; }
}
