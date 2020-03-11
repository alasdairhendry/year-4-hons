using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelpCanvas : UIPanel
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private ScrollRect scrollRect;
    [Space]
    [SerializeField] private Button buttonGeneral;
    [SerializeField] private GameObject panelGeneral;
    [Space]
    [SerializeField] private Button buttonQuesting;
    [SerializeField] private GameObject panelQuesting;
    [Space]
    [SerializeField] private Button buttonCombat;
    [SerializeField] private GameObject panelCombat;
    [Space]
    [SerializeField] private Button buttonInteraction;
    [SerializeField] private GameObject panelInteraction;
    [Space]
    [SerializeField] private Button buttonDialogue;
    [SerializeField] private GameObject panelDialogue;
    [Space]
    [SerializeField] private Button buttonCrafting;
    [SerializeField] private GameObject panelCrafting;
    [Space]
    [SerializeField] private Button buttonGathering;
    [SerializeField] private GameObject panelGathering;
    [Space]
    [SerializeField] private Button buttonSkilling;
    [SerializeField] private GameObject panelSkilling;
    [Space]
    [SerializeField] private Button buttonTalents;
    [SerializeField] private GameObject panelTalents;

    private void Awake ()
    {
        SetButtons ();
        DisplayPanel ( panelGeneral, "General" );
        Open ();
    }

    private void SetButtons ()
    {
        SetButton ( buttonGeneral, panelGeneral, "General" );
        SetButton ( buttonQuesting, panelQuesting, "Questing" );
        SetButton ( buttonCombat, panelCombat, "Combat" );
        SetButton ( buttonDialogue, panelDialogue, "Dialogue" );
        SetButton ( buttonInteraction, panelInteraction, "Interaction" );
        SetButton ( buttonCrafting, panelCrafting, "Crafting" );
        SetButton ( buttonGathering, panelGathering, "Gathering" );
        SetButton ( buttonSkilling, panelSkilling, "Skills" );
        SetButton ( buttonTalents, panelTalents, "Talent Points" );
    }

    private void SetButton (Button button, GameObject panel, string header)
    {
        button.onClick.AddListener ( () =>
        {
            DisplayPanel ( panel, header );
        } );
    }

    private void HideAllPanels ()
    {
        panelGeneral.SetActive ( false );
        panelQuesting.SetActive ( false );
        panelCombat.SetActive ( false );
        panelInteraction.SetActive ( false );
        panelDialogue.SetActive ( false );
        panelGathering.SetActive ( false );
        panelCrafting.SetActive ( false );
        panelSkilling.SetActive ( false );
        panelCrafting.SetActive ( false );
    }

    private void DisplayPanel(GameObject panel, string header)
    {
        HideAllPanels ();
        panel.SetActive ( true );
        headerText.text = "Help - " + header;
        scrollRect.DOVerticalNormalizedPos ( 1, 0.25f );
    }

    public override void Open ()
    {
        base.Open ();
        isOpened = true;
        mainPanel.SetActive ( true );
        UIPanelController.instance.OnPanelOpened ( this );
        DisplayPanel ( panelGeneral, "General" );
    }

    public override void Close (bool bypassClosedCheck = false)
    {
        if (isOpened == false && !bypassClosedCheck) return;

        base.Close ( bypassClosedCheck );
        isOpened = false;
        mainPanel.SetActive ( false );
        UIPanelController.instance.OnPanelClosed ( this );
    }
}
