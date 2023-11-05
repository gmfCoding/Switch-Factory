using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tile.assert", menuName = "Game/Tile", order = 0)]
public class TileInfo : AssetInfo
{
    GameObject model;

    TileType placement;
}
