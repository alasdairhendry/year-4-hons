using QuestFlow.QuestEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public class ActionGiveRecipeIngredients : Action
    {
        [SerializeField] CraftingRecipe recipe;

        public override void DoAction ()
        {
            for (int i = 0; i < recipe.ingredientsRequired.Count; i++)
            {
                EntityManager.instance.PlayerInventory.AddItem ( recipe.ingredientsRequired[i].ID, recipe.ingredientsRequired[i].Amount );
            }
        }
    }
}
