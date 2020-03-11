using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInformationPanel : UIPanel
{
    public static ItemInformationPanel instance;

    [SerializeField] [ItemID] private List<int> itemsToIgnore = new List<int> ();

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private Transform buttonsParent;
    [Space]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI informationText;
    [SerializeField] private ScrollRect buttonScrollRect;
    [SerializeField] private ScrollRect informationScrollRect;
    [SerializeField] private Button closeButton;
    [Space]
    [SerializeField] private GameObject itemButtonGameObject;
    [Space]
    [SerializeField] private List<DropTable> dropTables = new List<DropTable> ();
    [SerializeField] private List<NPCData> npcs = new List<NPCData> ();
    [SerializeField] private List<CraftingTable> craftingTables = new List<CraftingTable> ();
    [SerializeField] private List<NPCShopkeeper> shopkeepers = new List<NPCShopkeeper> ();

    private Dictionary<int, ItemInformation> dictionary = new Dictionary<int, ItemInformation> ();

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
        Close ( true );
    }

    private void Start ()
    {
        CreateButtons ();
    }

    public override void Open ()
    {
        base.Open ();
        mainPanel.SetActive ( true );
        isOpened = true;
        UIPanelController.instance.OnPanelOpened ( this );
        CloseInformationPanel ();
    }

    public override void Close (bool bypassCloseCheck = false)
    {
        if (isOpened == false && !bypassCloseCheck) return;

        base.Close ();
        mainPanel.SetActive ( false );
        isOpened = false;
        UIPanelController.instance.OnPanelClosed ( this );
    }

    private void CreateButtons ()
    {
        List<ItemBaseData> items = new List<ItemBaseData> ();

        for (int i = 0; i < 1000; i++)
        {
            ItemBaseData item = null;

            if (ItemDatabase.GetItem ( i, out item ))
            {
                if (itemsToIgnore.Contains ( i )) continue;

                items.Add ( item );
            }
        }

        items = items.OrderBy ( x => x.Name ).ToList ();

        for (int i = 0; i < items.Count; i++)
        {
            ItemBaseData item = items[i];
            int itemID = item.ID;

            GameObject newButton = Instantiate ( itemButtonGameObject );
            newButton.transform.SetParent ( buttonsParent );
            newButton.GetComponentInChildren<TextMeshProUGUI> ().text = item.Name;
            newButton.GetComponentInChildren<TextMeshProUGUI> ().transform.GetChild ( 0 ).GetComponent<Image> ().sprite = item.Sprite;

            newButton.GetComponentInChildren<Button> ().onClick.AddListener ( () =>
            {
                OpenInformationPanel ( itemID );
            } );

            ItemInformation itemInformation = new ItemInformation ( itemID, npcs, craftingTables, shopkeepers );
            dictionary.Add ( itemID, itemInformation );
        }

        itemButtonGameObject.SetActive ( false );
    }

    private void OpenInformationPanel(int itemID)
    {
        informationPanel.gameObject.SetActive ( true );
        buttonPanel.gameObject.SetActive ( false );

        ItemInformation itemInformation = dictionary[itemID];
        string text = "";

        if (itemInformation.npcs.Count > 0)
        {
            text = "This item can be dropped by the following NPCs\n";

            for (int i = 0; i < itemInformation.npcs.Count; i++)
            {
                text += "-"+ itemInformation.npcs[i].NpcName + "\n";
            }

            text += "\n";
        }

        if (itemInformation.craftingTables.Count > 0)
        {
            text += "This item can be crafted at the following crafting tables\n";

            for (int i = 0; i < itemInformation.craftingTables.Count; i++)
            {
                text += "-" + itemInformation.craftingTables[i].craftingTableName + "\n";
            }

            text += "\n";
        }

        if (itemInformation.shopkeepers.Count > 0)
        {
            text += "This item can be purchased from the following shops\n";

            for (int i = 0; i < itemInformation.shopkeepers.Count; i++)
            {
                text += "-" + itemInformation.shopkeepers[i].Npc.NpcData.NpcName + "\n";
            }

            text += "\n";
        }

        informationText.text = text;

        closeButton.onClick.RemoveAllListeners ();
        closeButton.onClick.AddListener ( () => { CloseInformationPanel (); } );
    }

    private void CloseInformationPanel ()
    {
        informationPanel.gameObject.SetActive ( false );
        buttonPanel.gameObject.SetActive ( true );

        closeButton.onClick.RemoveAllListeners ();
        closeButton.onClick.AddListener ( () => { Close (); } );
    }

    public class ItemInformation
    {
        public int itemID = -1;
        public List<NPCData> npcs = new List<NPCData> ();
        public List<CraftingTable> craftingTables = new List<CraftingTable> ();
        public List<NPCShopkeeper> shopkeepers = new List<NPCShopkeeper> ();

        public ItemInformation (int itemID, List<NPCData> _npcs, List<CraftingTable> _craftingTables, List<NPCShopkeeper> _shopkeepers)
        {
            this.itemID = itemID;
            GetInformation ( _npcs, _craftingTables, _shopkeepers );

        }

        private void GetInformation (List<NPCData> _npcs, List<CraftingTable> _craftingTables, List<NPCShopkeeper> _shopkeepers)
        {
            for (int i = 0; i < _npcs.Count; i++)
            {
                NPCData npc = _npcs[i];

                if (npc.UniqueDropTable != null)
                {
                    if(npc.UniqueDropTable.loot.Exists(x=>x.itemID == itemID ))
                    {
                        npcs.Add ( npc );
                        continue;
                    }
                }

                if (npc.AccessToCoinsDropTable)
                {
                    if (DropTableManager.instance.CoinsDropTable.loot.Exists ( x => x.itemID == itemID ))
                    {
                        npcs.Add ( npc );
                        continue;
                    }
                }

                if (npc.AccessToGunDropTable)
                {
                    if (DropTableManager.instance.GunDropTable.loot.Exists ( x => x.itemID == itemID ))
                    {
                        npcs.Add ( npc );
                        continue;
                    }
                }

                if (npc.AccessToIngredientsDropTable)
                {
                    if (DropTableManager.instance.IngredientsDropTable.loot.Exists ( x => x.itemID == itemID ))
                    {
                        npcs.Add ( npc );
                        continue;
                    }
                }

                if (npc.AccessToMaskTable)
                {
                    if (DropTableManager.instance.MaskDropTable.loot.Exists ( x => x.itemID == itemID ))
                    {
                        npcs.Add ( npc );
                        continue;
                    }
                }

                if (npc.AccessToMeleeDropTable)
                {
                    if (DropTableManager.instance.MeleeDropTable.loot.Exists ( x => x.itemID == itemID ))
                    {
                        npcs.Add ( npc );
                        continue;
                    }
                }

                if (npc.AccessToPartyHatTable)
                {
                    if (DropTableManager.instance.PartyHatDropTable.loot.Exists ( x => x.itemID == itemID ))
                    {
                        npcs.Add ( npc );
                        continue;
                    }
                }
            }

            for (int i = 0; i < _craftingTables.Count; i++)
            {
                for (int y = 0; y < _craftingTables[i].recipes.Count; y++)
                {
                    CraftingRecipe recipe = _craftingTables[i].recipes[y];

                    if (recipe.resultingItems.Exists ( x => x.ID == itemID ))
                    {
                        craftingTables.Add ( _craftingTables[i] );
                        continue;
                    }
                }
            }

            for (int i = 0; i < _shopkeepers.Count; i++)
            {
                if(_shopkeepers[i].BaseInventory.Exists(x=>x.itemID == itemID ))
                {
                    shopkeepers.Add ( _shopkeepers[i] );
                }
            }
        }
    }
}
