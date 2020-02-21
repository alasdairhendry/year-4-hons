using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalentCanvas : UIPanel
{
    public static TalentCanvas instance;

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private TextMeshProUGUI talentPointsText;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }

    private void Start ()
    {
        Close ();
    }

    public override void Open ()
    {
        base.Open ();
        isOpened = true;
        mainPanel.SetActive ( true );
    }

    public override void Close ()
    {
        base.Close ();
        isOpened = false;
        mainPanel.SetActive ( false );
    }

    private void Update ()
    {
        if (!isOpened) return;
        talentPointsText.text = "<b>" + TalentManager.instance.CurrentTalentPoints + "</b> Talent Points";
    }
}
