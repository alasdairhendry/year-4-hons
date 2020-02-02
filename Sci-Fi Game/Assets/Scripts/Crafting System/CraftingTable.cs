using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = ("Crafting System/Table") )]
public class CraftingTable : ScriptableObject
{
    public List<CraftingRecipe> recipes = new List<CraftingRecipe> ();
}
