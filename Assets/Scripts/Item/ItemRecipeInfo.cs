using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "recipe.asset", menuName = "Game/Recipes/Base", order = 1)]
public class ItemRecipeInfo : AssetInfo
{
    [SerializeField]
    private List<ItemStack> input;
    [SerializeField]
    private List<ItemStack> output;

    public List<ItemStack> Input { get => input; }
    public List<ItemStack> Output { get => output; }

    public int time;

    public bool Criteria<T>(ICollection<T> items) where T : IProvider<ItemStack>
    {
        foreach (var require in Input)
        {
            bool found = items.Where(x => x.Get().item == require.item && x.Get().Amount >= require.Amount).FirstOrDefault() != null;
            if (!found)
                return false;
        }
        return true;
    }

    public bool TryMake<T>(ICollection<T> items, out ItemStack item) where T : IProvider<ItemStack>
    {
        item = null;
        if (!Criteria(items))
            return false;
        item = new ItemStack(output.FirstOrDefault());
        foreach (var require in Input)
        {

            var found = items.Where(x => x.Get().item == require.item && x.Get().Amount >= require.Amount).FirstOrDefault();
            found.Get().Amount -= require.Get().Amount;
            if (found.Get().Amount <= 0)
                items.Remove(found);
        }
        return true;
    }
}
