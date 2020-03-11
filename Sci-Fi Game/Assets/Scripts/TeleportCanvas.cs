using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeleportCanvas : UIPanel
{
    public static TeleportCanvas instance;

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject teleportButtonPrefab;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private TextMeshProUGUI titleText;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        Close ( true );
    }

    public override void Open ()
    {
        base.Open ();
        mainPanel.SetActive ( true );
        isOpened = true;
        UIPanelController.instance.OnPanelOpened ( this );
    }

    public override void Close (bool bypassCloseCheck = false)
    {
        if (isOpened == false && !bypassCloseCheck) return;

        base.Close ();
        hotkeyActions.Clear ();
        mainPanel.SetActive ( false );
        isOpened = false;
        UIPanelController.instance.OnPanelClosed ( this );
    }

    //public void SetDestinations (System.Action<TeleportDestination> onClickDestination, params TeleportDestination[] teleportDestinations)
    //{
    //    hotkeyActions.Clear ();

    //    for (int i = 0; i < contentPanel.transform.childCount; i++)
    //    {
    //        Destroy ( contentPanel.transform.GetChild ( i ).gameObject );
    //    }

    //    for (int i = 0; i < teleportDestinations.Length; i++)
    //    {
    //        GameObject button = Instantiate ( teleportButtonPrefab );
    //        button.transform.SetParent ( contentPanel );

    //        button.GetComponentInChildren<TextMeshProUGUI> ().text = teleportDestinations[i].name;

    //        int x = i;
    //        Button b = button.GetComponentInChildren<Button> ();
    //        b.onClick.AddListener ( () => { onClickDestination?.Invoke ( teleportDestinations[x] ); } );
    //    }
    //}

    //public void SetDestinations (System.Action<TeleportDestination> onClickDestination, List<TeleportDestination> teleportDestinations)
    //{
    //    hotkeyActions.Clear ();

    //    for (int i = 0; i < contentPanel.transform.childCount; i++)
    //    {
    //        Destroy ( contentPanel.transform.GetChild ( i ).gameObject );
    //    }

    //    for (int i = 0; i < teleportDestinations.Count; i++)
    //    {
    //        GameObject button = Instantiate ( teleportButtonPrefab );
    //        button.transform.SetParent ( contentPanel );

    //        button.GetComponentInChildren<TextMeshProUGUI> ().text = teleportDestinations[i].name;

    //        int x = i;
    //        Button b = button.GetComponentInChildren<Button> ();
    //        b.onClick.AddListener ( () => { onClickDestination?.Invoke ( teleportDestinations[x] ); } );
    //    }
    //}

    private List<System.Action> hotkeyActions = new List<System.Action> ();

    public void SetButtons(string title, List<string> buttonData, System.Action<int> onClickButton)
    {
        hotkeyActions.Clear ();
        titleText.text = title;

        for (int i = 0; i < contentPanel.transform.childCount; i++)
        {
            Destroy ( contentPanel.transform.GetChild ( i ).gameObject );
        }

        for (int i = 0; i < buttonData.Count; i++)
        {
            GameObject button = Instantiate ( teleportButtonPrefab );
            button.transform.SetParent ( contentPanel );

            if (i < 9)
            {
                button.GetComponentInChildren<TextMeshProUGUI> ().text = "<align=left>[" + (i + 1) + "]</align><line-height=0.0001>";
                button.GetComponentInChildren<TextMeshProUGUI> ().text += "\n<align=center>" + buttonData[i].ToProperCase () + "</align>";
            }
            else
            {
                button.GetComponentInChildren<TextMeshProUGUI> ().text = buttonData[i].ToProperCase ();
            }

            int x = i;
            Button b = button.GetComponentInChildren<Button> ();
            b.onClick.AddListener ( () => { onClickButton?.Invoke ( x ); Close (); } );

            if (i < 9)
            {
                hotkeyActions.Add ( () => { onClickButton?.Invoke ( x ); Close (); } );
            }
        }
    }

    public void SetDestinations (System.Action<TeleportationBeam> onClickDestination, List<TeleportationBeam> teleportBeams)
    {
        hotkeyActions.Clear ();
        titleText.text = "Choose Destination";

        for (int i = 0; i < contentPanel.transform.childCount; i++)
        {
            Destroy ( contentPanel.transform.GetChild ( i ).gameObject );
        }

        for (int i = 0; i < teleportBeams.Count; i++)
        {
            GameObject button = Instantiate ( teleportButtonPrefab );
            button.transform.SetParent ( contentPanel );

            if (i < 9)
            {
                button.GetComponentInChildren<TextMeshProUGUI> ().text = "<align=left>[" + (i + 1) + "]</align><line-height=0.0001>";
                button.GetComponentInChildren<TextMeshProUGUI> ().text += "\n<align=center>" + teleportBeams[i].Region.ToString ().ToProperCase () + "</align>";
            }
            else
            {
                button.GetComponentInChildren<TextMeshProUGUI> ().text = teleportBeams[i].Region.ToString ().ToProperCase ();
            }

            int x = i;
            Button b = button.GetComponentInChildren<Button> ();
            b.onClick.AddListener ( () => { onClickDestination?.Invoke ( teleportBeams[x] ); Close (); } );

            if (i < 9)
            {
                hotkeyActions.Add ( () => { onClickDestination?.Invoke ( teleportBeams[x] ); Close (); } );
            }
        }
    }

    public void OnHotkeyPressed (KeyCode keyCode, bool isShift, bool isControl, bool isAlt)
    {
        if (keyCode == KeyCode.Alpha1)
        {
            if (0 < hotkeyActions.Count)
                hotkeyActions[0].Invoke ();
        }

        if (keyCode == KeyCode.Alpha2)
        {
            if (1 < hotkeyActions.Count)
                hotkeyActions[1].Invoke ();
        }

        if (keyCode == KeyCode.Alpha3)
        {
            if (2 < hotkeyActions.Count)
                hotkeyActions[2].Invoke ();
        }

        if (keyCode == KeyCode.Alpha4)
        {
            if (3 < hotkeyActions.Count)
                hotkeyActions[3].Invoke ();
        }

        if (keyCode == KeyCode.Alpha5)
        {
            if (4 < hotkeyActions.Count)
                hotkeyActions[4].Invoke ();
        }

        if (keyCode == KeyCode.Alpha6)
        {
            if (5 < hotkeyActions.Count)
                hotkeyActions[5].Invoke ();
        }

        if (keyCode == KeyCode.Alpha7)
        {
            if (6 < hotkeyActions.Count)
                hotkeyActions[6].Invoke ();
        }

        if (keyCode == KeyCode.Alpha8)
        {
            if (7 < hotkeyActions.Count)
                hotkeyActions[7].Invoke ();
        }

        if (keyCode == KeyCode.Alpha9)
        {
            if (8 < hotkeyActions.Count)
                hotkeyActions[8].Invoke ();
        }
    }
}

//public class TeleportDestination
//{
//    public string name;
//    public Vector3 worldPosition;

//    public TeleportDestination (string name, Vector3 worldPosition)
//    {
//        this.name = name;
//        this.worldPosition = worldPosition;
//    }
//}
