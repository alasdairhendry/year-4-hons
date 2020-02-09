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

    //[SerializeField] private Interactable interactable;
    private Interactable grownInteractable;
    private Interactable harvestableInteractable;

    private bool hasBeenNurtured = false;
    private bool currentState = false;
    private float currentGrowTime = 0.0f;
    private float timeUntilReady = 0.0f;

    private void Start ()
    {
        Interactable[] ints = GetComponentsInChildren<Interactable> ();

        for (int i = 0; i < ints.Length; i++)
        {
            if (ints[i].name.ToLower ().Contains ( "grown" ))
            {
                grownInteractable = ints[i];
            }

            if (ints[i].name.ToLower ().Contains ( "harvestable" ))
            {
                harvestableInteractable = ints[i];
            }
        }

        if(grownInteractable == null || harvestableInteractable == null)
        {
            Debug.LogError ( "Interactable not found - Please ensure grown-interactable and harvestable-interactable are named correctly", this.gameObject );
        }

        if (startsGrown)
        {
            SetState ( true );
        }
    }

    public void Interact ()
    {
        if (currentState)
        {
            OnHarvest ();
        }
        else
        {
            if (canBeNurtured)
            {
                if (!hasBeenNurtured)
                {
                    int returned = EntityManager.instance.PlayerInventory.RemoveItem ( nurtureItemRequired, 1 );

                    if (returned == 1)
                    {
                        MessageBox.AddMessage ( "I need some " + ItemDatabase.GetItem ( nurtureItemRequired ).Name + " to cultivate this resource.", MessageBox.Type.Warning );
                        return;
                    }
                    else
                    {
                        OnNurture ();
                    }
                }
                else
                {
                    MessageBox.AddMessage ( "I've already cultivated this resource!.", MessageBox.Type.Info );
                }
            }
            else
            {
                MessageBox.AddMessage ( "There's nothing I can do to this resource.", MessageBox.Type.Info );
            }
        }
    }

    private void OnHarvest ()
    {
        int amountGathered = Random.Range ( (int)itemGivenRangeAmount.x, (int)(itemGivenRangeAmount.y + 1) );
        if (itemGivenRangeAmount.x == itemGivenRangeAmount.y) amountGathered = (int)itemGivenRangeAmount.x;
        int bonusAmountGathered = 0;

        if (hasBeenNurtured)
        {
            for (int i = 0; i < amountGathered; i++)
            {
                if (Random.value > 0.5f)
                {
                    bonusAmountGathered++;
                }
            }

            amountGathered += bonusAmountGathered;
        }

        int x = EntityManager.instance.PlayerInventory.AddItem ( itemIDGiven, amountGathered );

        if (x != 0)
        {
            // Player inventory too full
            EntityManager.instance.PlayerInventory.RemoveItem ( amountGathered - x );
            return;
        }

        SkillManager.instance.AddXpToSkill ( SkillManager.SkillType.Gathering, amountGathered * 15.0f );

        if (bonusAmountGathered > 0)
            MessageBox.AddMessage ( "You recieved an extra " + bonusAmountGathered.ToString ( "0" ) + " resources because this item was cultivated.", MessageBox.Type.Info );

        SetState ( false );

        grownInteractable.IsInteractable = false;

        if (canBeNurtured)
        harvestableInteractable.SetInteractType ( "Cultivate" );

        if (!growsBack)
        {
            Destroy ( this.gameObject );
        }
    }

    private void OnNurture ()
    {
        hasBeenNurtured = true;
        harvestableInteractable.SetInteractType ( "" );
        SkillManager.instance.AddXpToSkill ( SkillManager.SkillType.Gathering, 30.0f );
        MessageBox.AddMessage ( "You cultivate the resources, allowing it to grow back sooner.", MessageBox.Type.Info );        
    }

    private void OnGrownBack ()
    {
        if (hasBeenNurtured)
        {
            SkillManager.instance.AddXpToSkill ( SkillManager.SkillType.Gathering, 15.0f );
            MessageBox.AddMessage ( "A resource you cultivated has now grown.", MessageBox.Type.Info );
        }

        currentGrowTime = 0.0f;
        SetState ( true );       
    }

    private void Update ()
    {
        if (!growsBack) return;

        if (!currentState)
        {
            currentGrowTime += Time.deltaTime * (hasBeenNurtured ? 3.5f : 1.0f);
            timeUntilReady = (growTime - currentGrowTime) / (hasBeenNurtured ? 3.5f : 1.0f);

            harvestableInteractable.SetInteractName ( harvestableInteractable.initialInteractableName + " " + "[" + timeUntilReady.ToString ( "0.0" ) + "]" );

            if (currentGrowTime >= growTime)
            {
                OnGrownBack ();                
            }
        }
    }

    private void SetState (bool state)
    {
        currentState = state;

        if (currentState)
        {
            grownInteractable.IsInteractable = true;
            harvestableInteractable.ResetInteractType ();
            harvestableInteractable.ResetInteractName ();
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
