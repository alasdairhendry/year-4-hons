using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Crafting System/Recipe"))]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public string recipeDescription
    {
        get
        {
            if (resultingItems.Count == 1)
            {
                return ItemDatabase.GetItem ( resultingItems[0].ID ).Description;
            }
            else if (resultingItems.Count == 0)
            {
                Debug.LogError ( "No items returned" );
                return "";
            }
            else
            {
                Debug.LogError ( "Need to update this to handle multiple items oops" );
                return ItemDatabase.GetItem ( resultingItems[0].ID ).Description;
            }
        }
    }
    public float timeToCraft = 1.0f;
    public float tableResourceUsage = 1.0f;
    public List<Inventory.ItemStack> ingredientsRequired = new List<Inventory.ItemStack> ();
    public List<Inventory.ItemStack> toolsRequired = new List<Inventory.ItemStack> ();
    public List<Inventory.ItemStack> resultingItems = new List<Inventory.ItemStack> ();
}