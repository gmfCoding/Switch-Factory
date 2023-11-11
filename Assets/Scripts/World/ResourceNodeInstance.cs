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

    public void OnCreate()
    {
        if (!info.protection)
            return;
        var collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = info.protectionRadius;
        for (int i = 0; i < info.protectionDetail; i++)
        {
            float rR = Random.Range(-info.protectionRadius, info.protectionRadius);
            float rA = Random.Range(0, 359);
            float x = Mathf.Sin(rA) * rR;
            float y = Mathf.Cos(rA) * rR;
            var prot = Instantiate(info.protector);
            //prot.node = this; // TODO: Create enemy class
            prot.transform.position = new Vector3(x, 0 /* Use world height */, y) + transform.position;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        { 
            // Scare away player
        }
    }
}
