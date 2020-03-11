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
    public float currentEnergy { get; protected set; } = 0.0f;
    public float currentEnergyNormalised { get { return currentEnergy / maxEnergy; } }

    public CraftingRecipe currentRecipeBeingCrafted { get; protected set; } = null;
    public float currentCraftingTime { get; protected set; } = 0.0f;
    public Inventory resultsInventory { get; protected set; } = new Inventory ( 12, true, false );
    public string CraftingBenchName { get => craftingBenchName; protected set => craftingBenchName = value; }

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
                currentCraftingTime -= Time.deltaTime * SkillModifiers.CraftingRecipeSpeedModifier;

                if(currentCraftingTime <= 0)
                {
                    OnRecipeCrafted ();
                }
            }
        }
    }

    private void OnRecipeCrafted ()
    {
        if (Random.value < SkillModifiers.CraftingSuccessChance)
        {
            for (int i = 0; i < currentRecipeBeingCrafted.resultingItems.Count; i++)
            {
                resultsInventory.AddItem ( currentRecipeBeingCrafted.resultingItems[i].ID, currentRecipeBeingCrafted.resultingItems[i].Amount, true );
            }

            MessageBox.AddMessage ( "Recipe " + currentRecipeBeingCrafted.recipeName + " has finished being crafted at " + craftingBenchName, MessageBox.Type.Info );

            currentCraftingTime = 0.0f;
        }
        else
        {
            MessageBox.AddMessage ( "Failed to craft recipe " + currentRecipeBeingCrafted.recipeName + " at " + craftingBenchName, MessageBox.Type.Error );
            currentCraftingTime = 0.0f;
            currentRecipeBeingCrafted = null;
        }
    }

    public virtual void Interact ()
    {
        CraftingCanvas.instance.SetCraftingTable ( table, this );
        CraftingCanvas.instance.Open ();
    }

    public virtual void Claim ()
    {
        if (currentCraftingTime > 0)
        {
            MessageBox.AddMessage ( "The recipe is not finished yet.", MessageBox.Type.Error );
            return;
        }

        if (resultsInventory.IsEmpty)
        {
            MessageBox.AddMessage ( "There's nothing in here.", MessageBox.Type.Error );
            return;
        }

        int stacks = resultsInventory.stacks.Count;

        for (int i = 0; i < stacks; i++)
        {
            ItemDatabase.SendTo ( resultsInventory, EntityManager.instance.PlayerInventory, resultsInventory.stacks[0].ID, resultsInventory.stacks[0].Amount, false );
        }

        SoundEffectManager.Play ( AudioClipAsset.InventoryUpdated, AudioMixerGroup.SFX );

        if (resultsInventory.IsEmpty)
        {
            currentRecipeBeingCrafted = null;
        }
        else
        {
            MessageBox.AddMessage ( "Your inventory couldn't hold everything.", MessageBox.Type.Warning );
        }
    }

    public virtual void Create(CraftingRecipe recipe)
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

        if (CheckCanCreateRecipe(recipe))
        {
            for (int i = 0; i < recipe.ingredientsRequired.Count; i++)
            {
                if (Random.value <= TalentManager.instance.GetTalentModifier ( TalentType.MaterialDesign ))
                {
                    MessageBox.AddMessage ( "Your " + TalentManager.instance.GetTalent ( TalentType.MaterialDesign ).talentData.talentName + " talent saves " + recipe.ingredientsRequired[i].Amount + " " + ItemDatabase.GetItem ( recipe.ingredientsRequired[i].ID ).Name + " from being consumed." );
                }
                else
                {
                    EntityManager.instance.PlayerInventory.RemoveItem ( recipe.ingredientsRequired[i].ID, recipe.ingredientsRequired[i].Amount );
                }
            }

            currentRecipeBeingCrafted = recipe;
            currentEnergy -= recipe.tableResourceUsage;

            if (Random.value <= TalentManager.instance.GetTalentModifier ( TalentType.Demand ))
            {
                MessageBox.AddMessage ( "Your " + TalentManager.instance.GetTalent ( TalentType.Demand ).talentData.talentName + " talent halves the crafting time for " + currentRecipeBeingCrafted.recipeName );
                currentCraftingTime = recipe.timeToCraft * 0.5f;
            }
            else
            {
                currentCraftingTime = recipe.timeToCraft;
            }

            SkillManager.instance.AddXpToSkill ( SkillType.Crafting, recipe.timeToCraft * 10.0f );
        }
        else
        {
            MessageBox.AddMessage ( "Not enough ingredients to create item.", MessageBox.Type.Warning );
        }
    }

    protected bool CheckCanCreateRecipe (CraftingRecipe recipe)
    {
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

        return canCreate;
    }
}
