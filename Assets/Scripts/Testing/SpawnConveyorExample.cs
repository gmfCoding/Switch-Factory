using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnConveyorExample : MonoBehaviour, IItemTransport
{
    World world;
    List<Conveyor> conveyors = new List<Conveyor>();
    public bool CanAcceptFrom(IItemTransport from)
    {
        return true;
    }

    public void Give(Item item, IItemTransport target, out int taken)
    {
        taken = 1;
    }

    public Item Take()
    {
        return new Item(Game.instance.GetAsset<ItemInfo>("iron"), 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        world = Game.instance.world;
        conveyors.Add(world.CreateTileEntity<Conveyor>(new Vector2Int(2, 0)));
        conveyors[0].direction = Vector2Int.right;
        var send = new Item(Game.instance.GetAsset<ItemInfo>("iron"), 1);
        conveyors[0].Give(send, this, out int taken);  
        conveyors.Add(world.CreateTileEntity<Conveyor>(new Vector2Int(3, 0)));
        conveyors[1].direction = Vector2Int.right;
    }

    // Update is called once per frame
    void Update()
    {
    //    foreach (var conv in conveyors)
    //    {
    //        foreach (var item in conv.items)
    //        {
    //            Debug.Log(item.Key + ":" + item.Value.ToString());  
    //        }
    //    }
    }
}
