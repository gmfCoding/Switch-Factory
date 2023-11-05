using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "recipe.asset", menuName = "Game/Recipe", order = 1)]
public class ItemRecipeInfo : ScriptableObject
{
    public new string name;
    public string description;

    public List<Item> input;
    public List<Item> output;
}
