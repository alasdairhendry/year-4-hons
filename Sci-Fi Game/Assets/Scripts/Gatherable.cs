using UnityEngine;

public class Gatherable : MonoBehaviour
{
    [SerializeField] [ItemID] private int itemIDGiven;
    //[SerializeField] private Vector2 itemGivenRangeAmount = new Vector2 ();
    [SerializeField] private int baseAmountGiven = 1;
    [SerializeField] private int maxAmountWithLevelModifier = 10;
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
                        if (Random.value <= SkillModifiers.GatheringCompostChance)
                        {
                            OnNurtureSuccess ();
                        }
                        else
                        {
                            OnNurtureFail ();                            
                        }
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
        int amountGathered = baseAmountGiven;
        int levelBonusAmount = 0;
        int compostBonusAmount = 0;

        float levelBonusRandom = Random.value;

        while(levelBonusRandom < SkillModifiers.GatheringYieldChance && amountGathered + levelBonusAmount < maxAmountWithLevelModifier)
        {
            levelBonusAmount++;
            levelBonusRandom = Random.value;
        }

        if (hasBeenNurtured)
        {
            float compostBonusRandom = Random.value;

            while(compostBonusRandom < 0.33f)
            {
                compostBonusAmount++;
                compostBonusRandom = Random.value;
            }
        }

        int totalAmountGathered = amountGathered + levelBonusAmount + compostBonusAmount;

        int x = EntityManager.instance.PlayerInventory.AddItem ( itemIDGiven, totalAmountGathered );

        if (x != 0)
        {
            // Player inventory too full
            EntityManager.instance.PlayerInventory.RemoveItem ( totalAmountGathered - x );
            return;
        }

        SkillManager.instance.AddXpToSkill ( SkillType.Gathering, totalAmountGathered * 15.0f );

        if (levelBonusAmount > 0)
            MessageBox.AddMessage ( "You recieved an extra " + levelBonusAmount.ToString ( "0" ) + " resources because of your gathering level.", MessageBox.Type.Info );

        if (compostBonusAmount > 0)
            MessageBox.AddMessage ( "You recieved an extra " + compostBonusAmount.ToString ( "0" ) + " resources because this item was cultivated.", MessageBox.Type.Info );

        SetState ( false );

        grownInteractable.IsInteractable = false;

        if (canBeNurtured)
        harvestableInteractable.SetInteractType ( "Cultivate" );

        if (!growsBack)
        {
            Destroy ( this.gameObject );
        }
    }

    private void OnNurtureSuccess ()
    {
        hasBeenNurtured = true;
        harvestableInteractable.SetInteractType ( "" );
        SkillManager.instance.AddXpToSkill ( SkillType.Gathering, 30.0f );
        MessageBox.AddMessage ( "You cultivate the resources, allowing it to grow back sooner.", MessageBox.Type.Info );

        if (Random.value <= TalentManager.instance.GetTalentModifier ( TalentType.GreenFingers ))
        {
            EntityManager.instance.PlayerInventory.AddItem ( nurtureItemRequired, 1 );
            MessageBox.AddMessage ( "Your " + TalentManager.instance.GetTalent ( TalentType.GreenFingers ).talentData.talentName + " talent saves the " + ItemDatabase.GetItem ( nurtureItemRequired ).Name + " from being consumed" );
        }
    }

    private void OnNurtureFail ()
    {
        int randomMessage = Random.Range ( 0, 3 );

        if (randomMessage == 0)
            MessageBox.AddMessage ( "Your hand slips and the compost falls everywhere.", MessageBox.Type.Warning );
        else if (randomMessage == 1)
            MessageBox.AddMessage ( "You accidentally place the compost on the ground.", MessageBox.Type.Warning );
        else if (randomMessage == 2)
        {
            MessageBox.AddMessage ( "The compost chemically burns your hand and you drop it.", MessageBox.Type.Warning );
            EntityManager.instance.PlayerCharacter.Health.RemoveHealth ( EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.05f, DamageType.CompostDamage );
        }
    }

    private void OnGrownBack ()
    {
        if (hasBeenNurtured)
        {
            SkillManager.instance.AddXpToSkill ( SkillType.Gathering, 15.0f );
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
            currentGrowTime += Time.deltaTime * (hasBeenNurtured ? 3.5f : 1.0f) * SkillModifiers.GatheringGrowthSpeedModifier;
            timeUntilReady = (growTime - currentGrowTime) / (hasBeenNurtured ? 3.5f : 1.0f) / SkillModifiers.GatheringGrowthSpeedModifier;

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

            if (growsBack)
            {
                if (Random.value <= TalentManager.instance.GetTalentModifier ( TalentType.Supply ))
                {
                    currentGrowTime = growTime * 0.5f;
                    MessageBox.AddMessage ( "Your " + TalentManager.instance.GetTalent ( TalentType.Supply ).talentData.talentName + " talent halves the growth time for " + ItemDatabase.GetItem ( itemIDGiven ).Name );
                }
            }
        }
    }

}
