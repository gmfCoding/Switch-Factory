using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class TileEntity
{
    public GameObject obj;
    public Vector2Int pos;
    private Vector2Int direction;
    TileInfo           info;

    public Vector2Int Direction
    {
        get => direction;
        set
        {
            // TODO: Restore
            //if (direction != value) 
            {
                direction = value;
                RegenerateModel();
            }
            direction = value;
        }
    }

    public TileInfo Info { get => info; }

    public TileEntity(TileInfo info)
    {
        this.info = info;
    }

    public virtual void RegenerateModel()
    {
        TileEntityVisual.RegenerateModel(this, obj?.transform, true);
    }

    public virtual void OnEntityDestroyed()
    { 
        if (obj != null)
            GameObject.Destroy(obj);
    }

    public abstract bool IsWalkable();

    public void OnEntityCreate()
    {
        obj = GameObject.Instantiate(this.info.Model);
        TileEntityVisual.SetModelPostion(this.Info, obj.transform, pos);
    }
}
