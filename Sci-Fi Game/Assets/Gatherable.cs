using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : MonoBehaviour
{
    [SerializeField] [ItemID] private int itemIDGiven;
    [SerializeField] private Vector2 itemGivenRangeAmount = new Vector2 ();
    [Space]
    [SerializeField] private bool growsBack = true;
    [SerializeField] private bool startsGrown;
    [Space]
    [SerializeField] private GameObject harvestedGraphics;
    [SerializeField] private GameObject grownGraphics;
    [Space]
    [NaughtyAttributes.ShowIf ( nameof ( growsBack ) )]
    [SerializeField] private float growTime = 2.5f;

    [NaughtyAttributes.ShowIf ( nameof ( growsBack ) )]
    [SerializeField] private bool canBeNurtured = false;

    [NaughtyAttributes.ShowIf ( nameof ( canBeNurtured ) )]
    [SerializeField] [ItemID] private int nurtureItemRequired = 1;

    [SerializeField] private Interactable interactable;

    private bool hasBeenNurtured = false;
    private bool currentState = false;
    private float currentGrowTime = 0.0f;
    private float timeUntilReady = 0.0f;

    private void Start ()
    {
        if (startsGrown)
        {
            SetState ( true );
        }
    }

    public void Interact ()
    {
        if (currentState)
        {
            int randomAmount = Random.Range ( (int)itemGivenRangeAmount.x, (int)(itemGivenRangeAmount.y + 1) );
            if (itemGivenRangeAmount.x == itemGivenRangeAmount.y) randomAmount = (int)itemGivenRangeAmount.x;

            if (hasBeenNurtured)
            {
                int added = 0;

                for (int i = 0; i < randomAmount; i++)
                {
                    if (Random.value > 0.5f)
                    {
                        added++;
                    }
                }

                randomAmount += added;
                if (added > 0)
                    MessageBox.AddMessage ( "You recieved an extra " + added.ToString ( "0" ) + " resources because this item was cultivated.", MessageBox.Type.Info );
            }

            int x = EntityManager.instance.PlayerInventory.AddItem ( itemIDGiven, randomAmount );

            if(x != 0)
            {
                // Player inventory too full
                EntityManager.instance.PlayerInventory.RemoveCoins ( randomAmount - x );
                return;
            }

            SetState ( false );

            if (canBeNurtured)
            {
                interactable.SetInteractType ( "Cultivate" );
            }

            if (!growsBack)
            {
                Destroy ( this.gameObject );
            }
        }
        else
        {
            if (canBeNurtured && !hasBeenNurtured)
            {
                int returned = EntityManager.instance.PlayerInventory.RemoveItem ( nurtureItemRequired, 1 );

                if(returned == 1)
                {
                    return;
                }
                else
                {
                    hasBeenNurtured = true;
                }
            }
        }
    }

    private void Update ()
    {
        if (!growsBack) return;

        if (!currentState)
        {
            currentGrowTime += Time.deltaTime * (hasBeenNurtured ? 3.5f : 1.0f);
            timeUntilReady = (growTime - currentGrowTime) / (hasBeenNurtured ? 3.5f : 1.0f);

            interactable.SetInteractName ( interactable.initialInteractableName + " " + "[" + timeUntilReady.ToString ( "0.0" ) + "]" );

            if(currentGrowTime >= growTime)
            {
                currentGrowTime = 0.0f;
                SetState ( true );
            }
        }
    }

    private void SetState (bool state)
    {
        currentState = state;

        if (currentState)
        {
            interactable.ResetInteractType ();
            interactable.ResetInteractName ();
            grownGraphics.SetActive ( true );
            harvestedGraphics.SetActive ( false );
        }
        else
        {
            grownGraphics.SetActive ( false );
            harvestedGraphics.SetActive ( true );
            currentGrowTime = 0.0f;
            hasBeenNurtured = false;
        }
    }

}
