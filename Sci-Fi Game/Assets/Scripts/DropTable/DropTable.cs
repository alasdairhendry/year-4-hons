using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class DropTable : ScriptableObject
{
    public List<Loot> loot = new List<Loot> ();

    [NaughtyAttributes.Button]
    public void Roll1000Times ()
    {
        List<Inventory.ItemStack> overallRolls = new List<Inventory.ItemStack> ();

        for (int i = 0; i < 1000; i++)
        {
            List<Inventory.ItemStack> rolls = new List<Inventory.ItemStack> ();
            bool wasFactionRoll = false;

            if (RollTable ( out rolls, out wasFactionRoll ))
            {
                for (int x = 0; x < rolls.Count; x++)
                {
                    if (overallRolls.Exists ( roll => roll.ID == rolls[x].ID ))
                    {
                        overallRolls[overallRolls.IndexOf ( overallRolls.First ( first => first.ID == rolls[x].ID ) )].Amount += rolls[x].Amount;
                    }
                    else
                    {
                        overallRolls.Add ( rolls[x] );

                    }
                }
            }
        }

        Debug.Log ( "Rolled" );

        for (int i = 0; i < overallRolls.Count; i++)
        {
            Debug.Log ( string.Format ( "-{0} {1}", overallRolls[i].Amount, ItemDatabase.GetItem ( overallRolls[i].ID ).Name ) );
        }
    }

    public int GetOverallWeighting ()
    {
        int maxWeighting = 0;

        for (int i = 0; i < loot.Count; i++)
        {
            maxWeighting += loot[i].weight;
        }

        return maxWeighting;
    }

    public int GetOverallWeighting (List<Loot> loot)
    {
        int maxWeighting = 0;

        for (int i = 0; i < loot.Count; i++)
        {
            maxWeighting += loot[i].weight;
        }

        return maxWeighting;
    }

    public bool RollTable (out List<Inventory.ItemStack> rolls, out bool wasFactionRoll)
    {
        rolls = new List<Inventory.ItemStack> ();
        wasFactionRoll = false;

        int maxWeighting = GetOverallWeighting (loot);
        int random = Random.Range ( 0, maxWeighting + 1 );

        bool addedUniqueRoll = false;

        for (int i = 0; i < loot.Count; i++)
        {
            if (loot[i].weight == 0)
            {
                if (ItemDatabase.ItemExists ( loot[i].itemID ))
                {
                    AddItemToRoll ( ref rolls, loot[i] );
                    continue;
                }
            }

            if (random <= loot[i].weight && !addedUniqueRoll)
            {
                if (ItemDatabase.ItemExists ( loot[i].itemID ))
                {
                    AddItemToRoll ( ref rolls, loot[i] );
                    addedUniqueRoll = true;
                }
            }
            else
            {
                random -= loot[i].weight;
            }           
        }

        if (!addedUniqueRoll)
        {
            if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == FactionType.Kyrish)
            {
                int factionRandom = Mathf.RoundToInt ( random * 10.0f );
                factionRandom = Mathf.Clamp ( factionRandom, 0, maxWeighting );
                if (factionRandom < 0) random = 0;

                for (int i = 0; i < loot.Count; i++)
                {
                    if (factionRandom <= loot[i].weight && !addedUniqueRoll)
                    {
                        if (ItemDatabase.ItemExists ( loot[i].itemID ))
                        {
                            AddItemToRoll ( ref rolls, loot[i] );
                            addedUniqueRoll = true;
                            wasFactionRoll = true;
                            break;
                        }
                    }
                    else
                    {
                        factionRandom -= loot[i].weight;
                    }
                }
            }
        }

        if (rolls.Count <= 0)
            return false;
        else return true;
    }

    public bool RollTableGuaranteed (out List<Inventory.ItemStack> rolls, out bool wasFactionRoll)
    {
        Debug.Log ( "RollTableGuaranteed " );
        List <Loot> guaranteedLoot = new List<Loot> ( loot );

        for (int i = guaranteedLoot.Count - 1; i >= 0; i--)
        {
            if (ItemDatabase.ItemExists ( guaranteedLoot[i].itemID ) == false)
            {
                guaranteedLoot.RemoveAt ( i );
            }
        }

        rolls = new List<Inventory.ItemStack> ();
        wasFactionRoll = false;

        if (guaranteedLoot.Count <= 0)
        {
            Debug.LogError ( "Loot table did not contain any items after removing empty rolls" );
            return false;
        }

        int maxWeighting = GetOverallWeighting (guaranteedLoot);
        int random = Random.Range ( 0, maxWeighting + 1 );

        bool addedUniqueRoll = false;

        for (int i = 0; i < guaranteedLoot.Count; i++)
        {
            Debug.Log ( "Check Loot Index " + i );
            if (guaranteedLoot[i].weight == 0)
            {
                if (ItemDatabase.ItemExists ( guaranteedLoot[i].itemID ))
                {
                    AddItemToRoll ( ref rolls, guaranteedLoot[i] );
                    Debug.Log ( "Item was required" );
                    continue;
                }
            }

            if (random <= guaranteedLoot[i].weight && !addedUniqueRoll)
            {
                Debug.Log ( "Roll hit " + guaranteedLoot[i].itemID );
                if (ItemDatabase.ItemExists ( guaranteedLoot[i].itemID ))
                {
                Debug.Log ( "Item Exists " + guaranteedLoot[i].itemID );
                    AddItemToRoll ( ref rolls, guaranteedLoot[i] );
                    addedUniqueRoll = true;
                }
            }
            else
            {
                Debug.Log ( "Roll missed" );
                random -= guaranteedLoot[i].weight;
            }
        }

        if (!addedUniqueRoll)
        {
            if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == FactionType.Kyrish)
            {
                int factionRandom = Mathf.RoundToInt ( random * 10.0f );
                factionRandom = Mathf.Clamp ( factionRandom, 0, maxWeighting );
                if (factionRandom < 0) random = 0;

                for (int i = 0; i < guaranteedLoot.Count; i++)
                {
                    if (factionRandom <= guaranteedLoot[i].weight && !addedUniqueRoll)
                    {
                        if (ItemDatabase.ItemExists ( guaranteedLoot[i].itemID ))
                        {
                            AddItemToRoll ( ref rolls, guaranteedLoot[i] );
                            addedUniqueRoll = true;
                            wasFactionRoll = true;
                            break;
                        }
                    }
                    else
                    {
                        factionRandom -= guaranteedLoot[i].weight;
                    }
                }
            }
        }

        if (rolls.Count <= 0)
            return false;
        else return true;
    }

    private void AddItemToRoll(ref List<Inventory.ItemStack> rolls, Loot loot)
    {
        int amount = loot.amount;

        if (loot.range)
        {
            amount = Random.Range ( loot.amountRange.x, loot.amountRange.y + 1 );
        }

        rolls.Add ( new Inventory.ItemStack () { ID = loot.itemID, Amount = amount } );
    }
}

[System.Serializable]
public class Loot
{
    [ItemIDAttribute] public int itemID = -1;
    public int amount = 1;
    public bool range = false;
    public Vector2Int amountRange = new Vector2Int ();
    public int weight = 1;
    public bool foldout;
}
