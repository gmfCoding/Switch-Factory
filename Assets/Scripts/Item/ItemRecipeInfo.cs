using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "recipe.asset", menuName = "Game/Recipe", order = 1)]
public class ItemRecipeInfo : AssetInfo
{
    [SerializeField]
    private List<Item> input;
    [SerializeField]
    private List<Item> output;

    public List<Item> Input { get => input; }
    public List<Item> Output { get => output; }
}
