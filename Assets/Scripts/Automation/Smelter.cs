using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Smelter : TileEntity, ITickable, IItemContainer
{

    IItemContainer dfl => (IItemContainer)this;
    ItemStack input;
    ItemStack output;

    SmelterRecipeInfo Recipe => GetCurrentRecipe();

    public Smelter(TileInfo info) : base(info)
    {
    }

    public override bool IsWalkable()
    {
        return false;
    }

    int ticks;
    public void Tick()
    {
        var recipe = this.Recipe;
        var give = recipe?.Output.First();
        var need = recipe?.Input.First();
        if (recipe == null || (output != null && output.item != give.item))
            return;
        if (input.item != need.item || input.Amount < need.Amount)
            return;
        if (output != null && output.CheckMerge(give) < give.Amount)
            return;
        ticks++;
        if (ticks >= recipe.time)
        {
            ticks = 0;
            if (output == null)
                output = new ItemStack(give);
            else
                output.Amount += give.Amount;
        }
    }

    public ItemStack Remove(ItemFilter filter, int amount)
    {
        if (output == null)
            return null;
        if (filter != null && !filter.Contains(output.item))
            return null;
        var item = output.Split(amount);
        if (item == output)
            output = null;
        return item;
    }

    public bool TryAdd(ItemStack item, IItemContainer target, out int taken, out int remaining)
    {
        if (!IItemContainer.TryAddDefault(this, item, target, out taken, out remaining))
            return false;
        var recipe = SmelterRecipeInfo.GetRecipeFor(input?.item);
        if (recipe == null)
            recipe = SmelterRecipeInfo.GetRecipeFor(item?.item);
        if (recipe == null)
            return false;
        if (input == null)
            taken = item.Amount;
        else if (item.Amount < Recipe.Input.First().Amount * 2)
        {
            taken = input.CheckMerge(item);
            taken = Mathf.Min(taken, Recipe.Input.First().Amount);
        }
        else
            taken = 0;
        remaining = item.Amount - taken;
        if (taken > 0)
        {
            input = item;
            input.Virtualise();
            input.Amount = taken;
            return true;
        }
        return false;
    }

    public SmelterRecipeInfo GetCurrentRecipe() => SmelterRecipeInfo.GetRecipeFor(input?.item);

    public bool CanAdd(ItemStack item)
    {
        if (item?.item == null)
            return false;
        var recipe = SmelterRecipeInfo.GetRecipeFor(item.item);
        if ((input == null || item.item == input.item) && recipe != null)
            return true;
        return false;
    }

    public ItemStack Remove(ItemStack item)
    {
        var get = output;
        output = null;
        return get;
    }

    public bool CanAcceptFrom(IItemContainer from)
    {
        return from is Inserter;
    }

    public IEnumerable<ItemStack> GetAvailableItems()
    {
        return new List<ItemStack>() { output };
    }
}
