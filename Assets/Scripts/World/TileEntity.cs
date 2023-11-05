using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileEntity
{
    public GameObject obj;
    public Vector2Int pos;
    public abstract bool IsWalkable();
}
