using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorVis : MonoBehaviour
{

    public static ConveyorVis instance;
    public List<Conveyor> conveyors = new List<Conveyor>();

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        foreach (var conv in conveyors)
        {
            foreach (var item in conv.items)
            {
                if (!item.Key.ExistInWorld())
                    item.Key.LinkTransform(Instantiate(item.Key.item.model).transform);
                item.Key.transform.position = conv.pos + new Vector2(conv.direction.x, conv.direction.y) * item.Value;
            }
        }
    }
}
