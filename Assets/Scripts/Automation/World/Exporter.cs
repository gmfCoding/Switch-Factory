using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exporter : TileEntity, IItemContainer
{
    private ExportRegion worldExporter;


    public Exporter(TileInfo info) : base(info)
    {
    }

    public ExportRegion WorldExporter {
        get
        {
            if (worldExporter == null )
                worldExporter = this.obj.GetComponent<ExportRegion>();
            return worldExporter;
        }
    }

    public bool CanAcceptFrom(IItemContainer from)
    {
        return true;
    }

    public bool CanAdd(ItemStack stack)
    {
        return stack != null;
    }

    public IEnumerable<ItemStack> GetAvailableItems()
    {
        return null;
    }

    public override bool IsWalkable()
    {
        return false;
    }

    public ItemStack Remove(ItemFilter filter, int amount)
    {
        return null;
    }

    public ItemStack Remove(ItemStack item)
    {
        return null;
    }

    public bool TryAdd(ItemStack item, IItemContainer target, out int taken, out int remaining)
    {
        if (!IItemContainer.TryAddDefault(this, item, target, out taken, out remaining))
            return false;
        WorldExporter.AddItem(item);
        remaining = 0;
        taken = item.Amount;
        return true;
    }
}
