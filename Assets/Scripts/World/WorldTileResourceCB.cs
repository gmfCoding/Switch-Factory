using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTileResourceCB : TileCallback
{

    public override void OnCreated(World world, Vector2Int pos)
    {
        ref var res = ref world.GetResourceInstanceReference(pos);
    }
}
