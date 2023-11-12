    using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnConveyorExample : MonoBehaviour
{
    World world;
    List<Conveyor> conveyors = new List<Conveyor>();

    public Vector2Int offset;
    void CreateConveyor(Vector2Int pos, Vector2Int dir)
    { 
        var conv = world.SetTileInfo(TileInfo.GetTile("conv_straight"),pos);
        conveyors.Add(conv as Conveyor);
        conv.Direction = dir;
    }

    public void TestAssembler()
    {
        offset += Vector2Int.up * 5;
        // Iron
        var prod = world.SetTileInfo(TileInfo.GetTile("item_producer"), new Vector2Int(offset.x, offset.y)) as ItemProducer;
        prod.itemName = "item_iron_plate";
        prod.Direction = Vector2Int.right;
        CreateConveyor(new Vector2Int(offset.x + 1, offset.y), Vector2Int.right);
        var inserter = world.SetTileInfo(TileInfo.GetTile("inserter"), new Vector2Int(offset.x + 2, offset.y)) as Inserter;
        CreateConveyor(new Vector2Int(offset.x + 6, offset.y), Vector2Int.right);

        // Bone
        var inserter2 = world.SetTileInfo(TileInfo.GetTile("inserter"), new Vector2Int(offset.x + 3, offset.y - 1)) as Inserter;
        inserter2.Direction = Vector2Int.up;
        CreateConveyor(new Vector2Int(offset.x + 3, offset.y - 2), Vector2Int.up);
        var prod2 = world.SetTileInfo(TileInfo.GetTile("item_producer"), new Vector2Int(offset.x + 3, offset.y - 3)) as ItemProducer;
        prod2.itemName = "item_bone";
        prod2.Direction = Vector2Int.up;

        //Assembler
        var assembler = world.SetTileInfo(TileInfo.GetTile("machine_assembler"), new Vector2Int(offset.x + 3, offset.y)) as ItemProducer;
        var inserter3 = world.SetTileInfo(TileInfo.GetTile("inserter"), new Vector2Int(offset.x + 4, offset.y)) as Inserter;
        CreateConveyor(new Vector2Int(offset.x + 5, offset.y), Vector2Int.right);
        var consumer = world.SetTileInfo(TileInfo.GetTile("item_consumer"), new Vector2Int(offset.x + 6, offset.y)) as ItemProducer;

    }
    public void TestSmelter()
    {
        offset += Vector2Int.up * 5;
        var prod = world.SetTileInfo(TileInfo.GetTile("item_producer"), new Vector2Int(offset.x + 1, offset.y)) as ItemProducer;
        prod.itemName = "item_crushed_ore";
        prod.Direction = Vector2Int.right;
        CreateConveyor(new Vector2Int(offset.x + 2, offset.y), Vector2Int.right);
        var inserter = world.SetTileInfo(TileInfo.GetTile("inserter"), new Vector2Int(offset.x + 3, offset.y)) as Inserter;
        inserter.Direction = Vector2Int.right;
        var smelter = world.SetTileInfo(TileInfo.GetTile("smelter"), new Vector2Int(offset.x + 4, offset.y)) as ItemProducer;
        var inserter2 = world.SetTileInfo(TileInfo.GetTile("inserter"), new Vector2Int(offset.x + 5, offset.y)) as Inserter;
        inserter.Direction = Vector2Int.right;
        CreateConveyor(new Vector2Int(offset.x + 6, offset.y), Vector2Int.right);
        var consumer = world.SetTileInfo(TileInfo.GetTile("item_consumer"), new Vector2Int(offset.x + 7, offset.y)) as ItemProducer;

    }

    public void DefaultSetup()
    {
        var prod = world.SetTileInfo(TileInfo.GetTile("importer"), new Vector2Int(offset.x++, offset.y)) as ItemProducer;
        //var prod = world.SetTileInfo(TileInfo.GetTile("item_producer"), new Vector2Int(offset.x++, offset.y)) as ItemProducer;

        //prod.itemName = "item_iron_ore";
        //prod.Direction = Vector2Int.right;

        CreateConveyor(new Vector2Int(offset.x++, offset.y), Vector2Int.up);
        var _1 = world.SetTileInfo(TileInfo.GetTile("inserter"), new Vector2Int(offset.x++, offset.y)) as Inserter;
        var _2 = world.SetTileInfo(TileInfo.GetTile("machine_crusher"), new Vector2Int(offset.x++, offset.y)) as MachineBase;
        var _3 = world.SetTileInfo(TileInfo.GetTile("inserter"), new Vector2Int(offset.x++, offset.y)) as Inserter;

        var _4 = world.SetTileInfo(TileInfo.GetTile("smelter"), new Vector2Int(offset.x++, offset.y)) as MachineBase;
        var _5 = world.SetTileInfo(TileInfo.GetTile("inserter"), new Vector2Int(offset.x++, offset.y)) as Inserter;
        CreateConveyor(new Vector2Int(offset.x++, offset.y), Vector2Int.right);
        var _6 = world.SetTileInfo(TileInfo.GetTile("machine_press"), new Vector2Int(offset.x++, offset.y)) as MachineBase;
        CreateConveyor(new Vector2Int(offset.x++, offset.y), Vector2Int.right);
        var _7 = world.SetTileInfo(TileInfo.GetTile("inserter"), new Vector2Int(offset.x++, offset.y)) as Inserter;
        var _8 = world.SetTileInfo(TileInfo.GetTile("machine_assembler"), new Vector2Int(offset.x++, offset.y)) as MachineBase;
        var _i5 = world.SetTileInfo(TileInfo.GetTile("inserter"), new Vector2Int(offset.x, offset.y)) as Inserter;
        var _11 = world.SetTileInfo(TileInfo.GetTile("exporter"), new Vector2Int(offset.x + 1, offset.y)) as MachineBase;

        offset.y--;
        offset.x--;
        // Bone
        var _9 = world.SetTileInfo(TileInfo.GetTile("inserter"), new Vector2Int(offset.x,  offset.y--)) as Inserter;
        _9.Direction = Vector2Int.up;
        CreateConveyor(new Vector2Int(offset.x, offset.y--), Vector2Int.up);
        var _10 = world.SetTileInfo(TileInfo.GetTile("item_producer"), new Vector2Int(offset.x, offset.y--)) as ItemProducer;
        _10.itemName = "item_bone";
        _10.Direction = Vector2Int.up;
        offset += Vector2Int.up * 10;
    }

    // Start is called before the first frame update
    void Start()
    {
        world = Game.instance.world;
        //CreateConveyor(new Vector2Int(offset.x++, offset.y), Vector2Int.left);

        DefaultSetup();
        //TestSmelter();
        //TestAssembler();
        /*s
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
