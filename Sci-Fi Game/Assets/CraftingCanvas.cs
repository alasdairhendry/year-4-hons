using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingCanvas : MonoBehaviour
{
    public static CraftingCanvas instance;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy ( this.gameObject );
            return;
        }

        Close ();
    }

    [SerializeField] private GameObject panel;
    [Space]
    [SerializeField] private List<CraftingRecipeUIButton> recipeButtons = new List<CraftingRecipeUIButton> ();
    [SerializeField] private List<CraftingIngredientUIPanel> ingredientPanels = new List<CraftingIngredientUIPanel> ();
    [SerializeField] private List<CraftingIngredientUIPanel> toolPanels = new List<CraftingIngredientUIPanel> ();
    [Space]
    [SerializeField] private TMPro.TextMeshProUGUI recipeNameText;
    [SerializeField] private TMPro.TextMeshProUGUI recipeDescriptionText;

    private CraftingTable currentTable;
    private CraftingRecipe currentRecipe;

    private void Update ()
    {
        if (EntityManager.instance.PlayerCharacter.cInput.rawInput != Vector2.zero)
        {
            Close ();
        }
    }

    public void Open(CraftingTable table)
    {
        if (table.recipes.Count <= 0)
        {
            Debug.LogError ( "Table does not have recipes" );
            return;
        }

        currentTable = table;
        UpdateRecipeButtons ();
        ShowRecipe ( table.recipes[0] );
        panel.SetActive ( true );
    }

    public void Close ()
    {
        panel.SetActive ( false );
        currentTable = null;
        currentRecipe = null;
    }

    private void UpdateRecipeButtons ()
    {
        for (int i = 0; i < recipeButtons.Count; i++)
        {
            recipeButtons[i].button.onClick.RemoveAllListeners ();

            if (i >= currentTable.recipes.Count)
            {
                recipeButtons[i].gameObject.SetActive ( false );
            }
            else
            {
                recipeButtons[i].gameObject.SetActive ( true );
                recipeButtons[i].text.text = currentTable.recipes[i].recipeName;
                int x = i;
                recipeButtons[i].button.onClick.AddListener ( () => { ShowRecipe ( currentTable.recipes[x] ); } );
            }
        }
    }

    private void UpdateRecipePanel ()
    {
        recipeNameText.text = currentRecipe.recipeName;
        recipeDescriptionText.text = currentRecipe.recipeDescription;

        for (int i = 0; i < ingredientPanels.Count; i++)
        {
            if (i >= currentRecipe.ingredientsRequired.Count)
            {
                ingredientPanels[i].panel.SetActive ( false );
            }
            else
            {
                ingredientPanels[i].panel.SetActive ( true );
                ingredientPanels[i].iconImage.sprite = ItemDatabase.GetItem ( currentRecipe.ingredientsRequired[i].ID ).Sprite;
                ingredientPanels[i].text.text = EntityManager.instance.PlayerInventory.GetQuantityOfItem ( currentRecipe.ingredientsRequired[i].ID ).ToString ( "0" ) + "/" + currentRecipe.ingredientsRequired[i].Amount.ToString ( "0" );
            }
        }

        for (int i = 0; i < toolPanels.Count; i++)
        {
            if (i >= currentRecipe.toolsRequired.Count)
            {
                toolPanels[i].panel.SetActive ( false );
            }
            else
            {
                toolPanels[i].panel.SetActive ( true );
                toolPanels[i].iconImage.sprite = ItemDatabase.GetItem ( currentRecipe.toolsRequired[i].ID ).Sprite;
                toolPanels[i].text.text = EntityManager.instance.PlayerInventory.GetQuantityOfItem ( currentRecipe.toolsRequired[i].ID ).ToString ( "0" ) + "/" + currentRecipe.toolsRequired[i].Amount.ToString ( "0" );               
            }
        }
    }

    private void ShowRecipe(CraftingRecipe recipe)
    {
        currentRecipe = recipe;
        UpdateRecipePanel ();
    }

    public void OnClick_Create ()
    {
        bool canCreate = true;

        for (int i = 0; i < currentRecipe.ingredientsRequired.Count; i++)
        {
            if (!EntityManager.instance.PlayerInventory.CheckHasItemQuantity ( currentRecipe.ingredientsRequired[i].ID, currentRecipe.ingredientsRequired[i].Amount ))
            {
                canCreate = false;
            }
        }

        for (int i = 0; i < currentRecipe.toolsRequired.Count; i++)
        {
            if (!EntityManager.instance.PlayerInventory.CheckHasItemQuantity ( currentRecipe.toolsRequired[i].ID, currentRecipe.toolsRequired[i].Amount ))
            {
                canCreate = false;
            }
        }

        if (canCreate)
        {
            for (int i = 0; i < currentRecipe.ingredientsRequired.Count; i++)
            {
                EntityManager.instance.PlayerInventory.RemoveItem ( currentRecipe.ingredientsRequired[i].ID, currentRecipe.ingredientsRequired[i].Amount );
            }

            for (int i = 0; i < currentRecipe.resultingItems.Count; i++)
            {
                EntityManager.instance.PlayerInventory.AddItem ( currentRecipe.resultingItems[i].ID, currentRecipe.resultingItems[i].Amount );
            }

            UpdateRecipePanel ();
        }
        else
        {
            MessageBox.AddMessage ( "Not enough ingredients to create item.", MessageBox.Type.Warning );
        }
    }
}
