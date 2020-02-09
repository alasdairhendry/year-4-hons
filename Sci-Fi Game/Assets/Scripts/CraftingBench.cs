using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBench : MonoBehaviour
{
    [SerializeField] private string craftingBenchName;
    [SerializeField] private CraftingTable table;
    [Space]
    [SerializeField] private float maxEnergy;
    [SerializeField] private float maxRestorationRate;
    private float currentEnergy = 0.0f;

    private CraftingRecipe currentRecipeBeingCrafted = null;
    private float currentCraftingTime = 0.0f;
    private Inventory resultsInventory = new Inventory ( 12, false, false );

    private void Start ()
    {
        currentEnergy = maxEnergy;
    }

    private void Update ()
    {
        RestoreEnergy ();
        CheckCurrentRecipe ();
    }

    private void RestoreEnergy ()
    {
        if (currentCraftingTime > 0) return;

        if (currentEnergy <= maxEnergy)
        {
            currentEnergy += Time.deltaTime;

            if (currentEnergy >= maxEnergy)
            {
                currentEnergy = maxEnergy;
            }
        }
    }

    private void CheckCurrentRecipe ()
    {
        if(currentRecipeBeingCrafted != null)
        {
            if(currentCraftingTime > 0)
            {
                currentCraftingTime -= Time.deltaTime;

                if(currentCraftingTime <= 0)
                {
                    OnRecipeCrafted ();
                }
            }
        }
    }

    private void OnRecipeCrafted ()
    {
        for (int i = 0; i < currentRecipeBeingCrafted.resultingItems.Count; i++)
        {
           resultsInventory.AddItem ( currentRecipeBeingCrafted.resultingItems[i].ID, currentRecipeBeingCrafted.resultingItems[i].Amount );
        }

        MessageBox.AddMessage ( "Recipe " + currentRecipeBeingCrafted.recipeName + " has finished being crafted at " + craftingBenchName, MessageBox.Type.Info );

        currentCraftingTime = 0.0f;
        currentRecipeBeingCrafted = null;
    }

    public void Interact ()
    {
        CraftingCanvas.instance.SetCraftingTable ( table, this );
        CraftingCanvas.instance.Open ();
    }

    public void Create(CraftingRecipe recipe)
    {
        if (currentCraftingTime > 0)
        {
            MessageBox.AddMessage ( "Something is already being crafted here.", MessageBox.Type.Error );
            return;
        }

        if (currentEnergy < recipe.tableResourceUsage)
        {
            MessageBox.AddMessage ( "There is not enough energy to craft this item.", MessageBox.Type.Error );
            return;
        }

        bool canCreate = true;

        for (int i = 0; i < recipe.ingredientsRequired.Count; i++)
        {
            if (!EntityManager.instance.PlayerInventory.CheckHasItemQuantity ( recipe.ingredientsRequired[i].ID, recipe.ingredientsRequired[i].Amount ))
            {
                canCreate = false;
            }
        }

        for (int i = 0; i < recipe.toolsRequired.Count; i++)
        {
            if (!EntityManager.instance.PlayerInventory.CheckHasItemQuantity ( recipe.toolsRequired[i].ID, recipe.toolsRequired[i].Amount ))
            {
                canCreate = false;
            }
        }

        if (canCreate)
        {
            for (int i = 0; i < recipe.ingredientsRequired.Count; i++)
            {
                EntityManager.instance.PlayerInventory.RemoveItem ( recipe.ingredientsRequired[i].ID, recipe.ingredientsRequired[i].Amount );
            }

            currentCraftingTime = recipe.timeToCraft;
            currentEnergy -= recipe.tableResourceUsage;

            //UpdateRecipePanel ();
        }
        else
        {
            MessageBox.AddMessage ( "Not enough ingredients to create item.", MessageBox.Type.Warning );
        }
    }

}
