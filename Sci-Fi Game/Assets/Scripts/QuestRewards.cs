using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestRewards : MonoBehaviour
{
    public static QuestRewards instance;
    [SerializeField] private List<QuestReward> questRewards = new List<QuestReward> ();
    [SerializeField] private CanvasGroup questRewardPanel;

    [SerializeField] private List<Image> itemPanels = new List<Image> ();
    [SerializeField] private List<Image> itemIcons = new List<Image> ();
    [SerializeField] private List<TextMeshProUGUI> itemAmounts = new List<TextMeshProUGUI> ();

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }

    public void GiveQuestReward (string questID)
    {
        Debug.Log ( "Giving quest reward " + questID );

        QuestReward reward = questRewards.FirstOrDefault ( x => x.ID == questID );
        if (reward == null)
        {
            Debug.LogError ( questID + " does not have a quest reward" );
            return;
        }

        for (int i = 0; i < itemPanels.Count; i++)
        {
            if (i >= reward.rewards.Count)
            {
                itemPanels[i].gameObject.SetActive ( false );
            }
            else
            {
                itemPanels[i].gameObject.SetActive ( true );
                itemIcons[i].sprite = ItemDatabase.GetItem ( reward.rewards[i].ID ).Sprite;
                itemAmounts[i].text = reward.rewards[i].Amount.ToString ();
            }
        }

        questRewardPanel.alpha = 1;
        questRewardPanel.blocksRaycasts = true;

        bool someSentToBank = false;
        bool allSentToBank = true;

        for (int i = 0; i < reward.rewards.Count; i++)
        {
            if (EntityManager.instance.PlayerInventory.CheckCanRecieveItem ( reward.rewards[i].ID, reward.rewards[i].Amount ))
            {
                Debug.Log ( "Player can recieve item " + ItemDatabase.GetItem ( reward.rewards[i].ID ).Name );
                int returned = EntityManager.instance.PlayerInventory.AddItem ( reward.rewards[i].ID, reward.rewards[i].Amount );
                Debug.Log ( "Returned " + returned );
                allSentToBank = false;
            }
            else
            {
                Debug.Log ( "Item sent to inventory" );
                EntityManager.instance.PlayerBankInventory.AddItem ( reward.rewards[i].ID, reward.rewards[i].Amount );
                someSentToBank = true;
            }
        }

        if (allSentToBank)
        {
            MessageBox.AddMessage ( "Your rewards have been sent to your bank" );
        }
        else if (someSentToBank)
        {
            MessageBox.AddMessage ( "Some of your rewards have been sent to your bank" );
        }
    }

    [System.Serializable]
    public class QuestReward
    {
        public string ID;
        public List<Inventory.ItemStack> rewards = new List<Inventory.ItemStack> ();
    }
}
