using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineRunner : MonoBehaviour
{
    //[SerializeField] private int startingCoins = 100;
    //[SerializeField] private int spinCost = 10;
    //[SerializeField] private int maxSpins = 1000;
    //[SerializeField] private DropTable table;
    //private bool gameOver = false;
    //private int currentCoins = 0;

    //[Space]
    //[SerializeField] private int maxIterations = 100;
    //private int currIterations = 0;

    //private void Start ()
    //{
    //    currentCoins = startingCoins;
    //}

    //private void Update ()
    //{
    //    if (gameOver) return;

    //    bool ranOutOfCoins = false;

    //    for (int i = 0; i < maxSpins; i++)
    //    {
    //        if (currentCoins < spinCost)
    //        {
    //            OnRanOutOfCoins ();
    //            ranOutOfCoins = true;
    //            break;
    //        }

    //        currentCoins -= spinCost;
    //        Inventory.ItemStack roll = table.RollTable ();

    //        if (roll != null)
    //        {
    //            currentCoins += roll.Amount;
    //        }
    //    }

    //    if (!ranOutOfCoins)
    //        OnRanOutOfSpins ();

    //    ranOutOfCoins = false;

    //    currIterations++;

    //    if(currIterations >= maxIterations)
    //    {
    //        gameOver = true;

    //        Debug.Log ( currIterations + " complete" );
    //        Debug.Log ( "Average failure rate " + ((float)failures / (float)currIterations) );
    //        Debug.Log ( "Average success rate " + ((float)successes / (float)currIterations) );
    //        Debug.Log ( "Average profit " + (float)wholeProfit / (float)successes );
    //        Debug.Log ( "Average profit/loss " + (float)wholeProfit / (float)currIterations );
    //        //Debug.Log ( "Max profit " + biggestProfit );
    //    }

    //    //Debug.Log ( "Ran out of spins - Made " + (currentCoins - startingCoins) + " profit" );
    //    //gameOver = true;
    //}

    //private int failures = 0;
    //private int successes = 0;
    //private int wholeProfit = 0;
    //private float biggestProfit = 0;

    //private void OnRanOutOfCoins ()
    //{
    //    failures++;
    //    Replay ();
    //}

    //private void OnRanOutOfSpins ()
    //{
    //    successes++;
    //    wholeProfit += (currentCoins - startingCoins);
    //    if (biggestProfit < (currentCoins - startingCoins)) biggestProfit = (currentCoins - startingCoins);
    //    Replay ();
    //}

    //[NaughtyAttributes.Button]
    //public void Replay ()
    //{
    //    currentCoins = startingCoins;
    //}
}
