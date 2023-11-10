using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeInstance : MonoBehaviour
{
    [SerializeField]
    private Vector2Int direction;
    [SerializeField]
    private Vector2Int position;

    public ResourceNodeInfo info;

    public Vector2Int Direction { get => direction; 
        set
        {
            direction = value;
            this.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
    }

    public Vector2Int Position
    {
        get => position;
        set
        {
            position = value;
            this.transform.position = TileUtil.TileToWorldspace(position, TileUtil.TileTransform.Centric);
        }
    }
}
