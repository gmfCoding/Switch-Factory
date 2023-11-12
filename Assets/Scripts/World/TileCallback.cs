using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileCallback : MonoBehaviour
{
    public abstract void OnCreated(World world, Vector2Int pos);
}
