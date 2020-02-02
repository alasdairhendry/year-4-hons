using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DropTable : ScriptableObject
{
    public List<Loot> loot = new List<Loot> ();

    public int GetOverallWeighting ()
    {
        int maxWeighting = 0;

        for (int i = 0; i < loot.Count; i++)
        {
            maxWeighting += loot[i].weight;
        }

        return maxWeighting;
    }

    public Inventory.ItemStack RollTable ()
    {
        int maxWeighting = GetOverallWeighting ();

        int random = Random.Range ( 0, maxWeighting + 1 );

        for (int i = 0; i < loot.Count; i++)
        {
            if(random <= loot[i].weight)
            {
                if(loot[i].itemID == -1)
                {
                    return null;
                }
                else
                {
                    int amount = loot[i].amount;

                    if (loot[i].range)
                    {
                        amount = Random.Range ( loot[i].amountRange.x, loot[i].amountRange.y + 1 );
                    }

                    return new Inventory.ItemStack () { ID = loot[i].itemID, Amount = amount };
                }
            }
            else
            {
                random -= loot[i].weight;
            }
        }

        return null;
    }
}

[System.Serializable]
public class Loot
{
    [ItemIDAttribute] public int itemID = -1;
    public int amount = 1;
    public bool range = false;
    public Vector2Int amountRange = new Vector2Int ();
    //public int minRange = 1;
    //public int maxRange = 10;
    public int weight = 1;
    public bool foldout;
}
