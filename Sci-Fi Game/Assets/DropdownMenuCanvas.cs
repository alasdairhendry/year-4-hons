using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownMenuCanvas : MonoBehaviour
{
    public static DropdownMenuCanvas instance;

    [SerializeField] private RectTransform panel;
    private List<Transform> dropdownItemPanels = new List<Transform> ();
    private List<Button> dropdownItemPanelButtons = new List<Button> ();
    private List<TextMeshProUGUI> dropdownItemPanelTexts = new List<TextMeshProUGUI> ();

    public bool IsActive { get; protected set; } = false;
    private bool clickToDisable = false;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        for (int i = 0; i < panel.transform.childCount; i++)
        {
            dropdownItemPanels.Add ( panel.transform.GetChild ( i ) );
            dropdownItemPanelButtons.Add ( dropdownItemPanels[i].GetComponent<Button> () );
            dropdownItemPanelTexts.Add ( dropdownItemPanelButtons[i].GetComponentInChildren<TextMeshProUGUI> () );
        }
    }

    private void LateUpdate ()
    {
        if (clickToDisable)
        {
            if (Mouse.Click ( 0 ) || Mouse.Click ( 1 ))
            {
                Hide ();
                clickToDisable = false;
            }
        }

        if (IsActive)
        {
            clickToDisable = true;
        }
    }

    public void Open (List<InventoryInteractionData> list, int itemID, int inventoryIndex)
    {
        ItemBaseData item;

        for (int i = 0; i < dropdownItemPanels.Count; i++)
        {
            int x = i;
            dropdownItemPanelButtons[i].onClick.RemoveAllListeners ();

            if (i >= list.Count)
            {
                dropdownItemPanels[i].gameObject.SetActive ( false );
            }
            else
            {
                item = ItemDatabase.GetItem ( itemID );

                if(item == null)
                {
                    Debug.Log ( "Item ID " + itemID + " does not exist" );
                }

                dropdownItemPanels[i].gameObject.SetActive ( true );
                dropdownItemPanelTexts[i].text = list[i].interactType.ToString () + " " + ColourHelper.TagColour ( item.Name, ColourDescription.OffWhiteText );
                dropdownItemPanelButtons[i].onClick.AddListener ( () => { Hide (); list[x].onInteract ( inventoryIndex ); } );
            }
        }

        DisplayPanel ();
    }

    public void Hide ()
    {
        panel.gameObject.SetActive ( false );
        IsActive = false;
    }

    private void DisplayPanel ()
    {
        IsActive = true;
        TooltipCanvas.instance.HideTooltip ();
        panel.gameObject.SetActive ( true );
        panel.anchoredPosition3D = Input.mousePosition + new Vector3 ( 32.0f, -32.0f, 0.0f );
    }
}
