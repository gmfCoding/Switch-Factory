using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "smelter_recipe.asset", menuName = "Game/Recipe/Smelter", order = 1)]

public class SmelterRecipeInfo : ItemRecipeInfo
{
    public static HashSet<SmelterRecipeInfo> recipes = new HashSet<SmelterRecipeInfo>();

    public static SmelterRecipeInfo GetRecipeFor(ItemInfo item)
    {
        if (item == null)
            return null;
        return SmelterRecipeInfo.recipes.Where(x => x.Input.All(x => x.item == item)).FirstOrDefault();
    }
}
