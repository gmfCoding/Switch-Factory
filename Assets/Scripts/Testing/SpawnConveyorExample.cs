using JetBrains.Annotations;
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

    void CreateConveyor(Vector2Int pos, Vector2Int dir)
    { 
        var conv = world.SetTile(TileInfo.GetTile("conv_straight"),pos);
        conveyors.Add(conv as Conveyor);
        conv.Direction = dir;
    }

    // Start is called before the first frame update
    void Start()
    {
        world = Game.instance.world;
        var prod = world.SetTile(TileInfo.GetTile("item_producer"), new Vector2Int(1, 2));
        prod.Direction = Vector2Int.right;
        CreateConveyor(new Vector2Int(2, 2), Vector2Int.right);
        CreateConveyor(new Vector2Int(3, 2), Vector2Int.right);
        CreateConveyor(new Vector2Int(4, 2), Vector2Int.up);
        CreateConveyor(new Vector2Int(4, 3), Vector2Int.left);
        CreateConveyor(new Vector2Int(3, 3), Vector2Int.up);
        CreateConveyor(new Vector2Int(3, 4), Vector2Int.up);
        var cons = world.SetTile(TileInfo.GetTile("item_consumer"), new Vector2Int(3, 5));
        cons.Direction = Vector2Int.down;

        /*
        conveyors.Add(world.CreateTileEntity<Conveyor>(new Vector2Int(3, 0)));
        conveyors[1].direction = Vector2Int.up;
        conveyors.Add(world.CreateTileEntity<Conveyor>(new Vector2Int(3, 1)));
        conveyors[2].direction = Vector2Int.up;
        conveyors.Add(world.CreateTileEntity<Conveyor>(new Vector2Int(3, 2)));
        conveyors[3].direction = Vector2Int.left;
        conveyors.Add(world.CreateTileEntity<Conveyor>(new Vector2Int(2, 2)));
        conveyors[4].direction = Vector2Int.left;*/
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
