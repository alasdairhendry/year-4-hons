using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalentEntryUIPanel : MonoBehaviour
{
    [SerializeField] private TalentType targetTalent = TalentType.BigGulp;
    [SerializeField] private Button button;
    [SerializeField] private Image icon;
    [SerializeField] private TooltipItemUI tooltip;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start ()
    {
        TalentData talentData = TalentManager.instance.GetTalent ( targetTalent ).talentData;
        icon.sprite = talentData.talentSprite;
        SetLevelText ();

        tooltip.SetTooltipAction ( () =>
        {
            if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == FactionType.Wallon)
            {
                return talentData.talentName + "\n" +

                ColourHelper.TagColour ( ColourHelper.TagSize (

                    talentData.talentDescription + "\n" +
                    "Level " + TalentManager.instance.GetTalent ( targetTalent ).currentLevel +"\n" +
                    "Click to Upgrade\n" +
                    "Shift + Click to Downgrade",

                    80 ), ColourDescription.OffWhiteText );
            }
            else
            {
                return talentData.talentName + "\n" +

                ColourHelper.TagColour ( ColourHelper.TagSize (

                    talentData.talentDescription + "\n" +
                    "Level " + TalentManager.instance.GetTalent ( targetTalent ).currentLevel + "\n" +
                    "Click to Upgrade",

                    80 ), ColourDescription.OffWhiteText );
            }
        } );

        button.onClick.AddListener ( () => {
            if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == FactionType.Wallon && Input.GetKey ( KeyCode.LeftShift ))
            {
                TalentManager.instance.DowngradeTalent ( targetTalent );
            }
            else
            {
                TalentManager.instance.UpgradeTalent ( targetTalent );
            }

            SetLevelText ();
        } );
    }

    private void SetLevelText ()
    {
        levelText.text = TalentManager.instance.GetTalent ( targetTalent ).currentLevel.ToString ( "0" );
    }
}
