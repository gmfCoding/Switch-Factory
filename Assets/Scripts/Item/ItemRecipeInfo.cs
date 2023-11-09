using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "recipe.asset", menuName = "Game/Recipe", order = 1)]
public class ItemRecipeInfo : AssetInfo
{
    [SerializeField]
    private List<ItemStack> input;
    [SerializeField]
    private List<ItemStack> output;

    public List<ItemStack> Input { get => input; }
    public List<ItemStack> Output { get => output; }

    public int time;
}
