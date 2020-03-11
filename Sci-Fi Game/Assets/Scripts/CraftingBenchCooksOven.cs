using QuestFlow.QuestEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBenchCooksOven : CraftingBench
{
    public override void Interact ()
    {
        if(QuestManager.instance.GetQuestDataByID("cooksassistant").currentQuestState == QuestFlow.QuestState.Completed)
        {
            base.Interact ();
        }
        else if(QuestManager.instance.GetQuestDataByID ( "cooksassistant" ).currentQuestState == QuestFlow.QuestState.InProgress)
        {
            MessageBox.AddMessage ( "The cook is too busy to let other people use his oven just now.", MessageBox.Type.Error );
        }
        else
        {
            MessageBox.AddMessage ( "I should probably ask the cook if I can use his oven first..", MessageBox.Type.Warning );
        }
    }

    public override void Create (CraftingRecipe recipe)
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

        if (CheckCanCreateRecipe ( recipe ))
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
            currentEnergy -= recipe.tableResourceUsage * 0.5f;

            if (Random.value <= TalentManager.instance.GetTalentModifier ( TalentType.Demand ))
            {
                MessageBox.AddMessage ( "Your " + TalentManager.instance.GetTalent ( TalentType.Demand ).talentData.talentName + " talent halves the crafting time for " + currentRecipeBeingCrafted.recipeName );
                currentCraftingTime = recipe.timeToCraft * 0.5f;
            }
            else
            {
                currentCraftingTime = recipe.timeToCraft;
            }

            MessageBox.AddMessage ( "The cook's oven is so advanced that your recipe is made in half the time." );
            currentCraftingTime *= 0.5f;
            SkillManager.instance.AddXpToSkill ( SkillType.Crafting, recipe.timeToCraft * 10.0f );
        }
        else
        {
            MessageBox.AddMessage ( "Not enough ingredients to create item.", MessageBox.Type.Warning );
        }
    }
}
