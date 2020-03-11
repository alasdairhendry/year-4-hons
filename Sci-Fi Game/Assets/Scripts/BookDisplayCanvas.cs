using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookDisplayCanvas : UIPanel
{
    public static BookDisplayCanvas instance;

    [SerializeField] private CanvasGroup cGroup;
    [SerializeField] private TextMeshProUGUI bookTitle;
    [SerializeField] private TextMeshProUGUI bookText;
    [SerializeField] private TextMeshProUGUI pageText;
    [SerializeField] private ScrollRect scrollRect;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        Close ( true );
    }

    public void SetBook (string bookTitle, string bookText)
    {
        this.bookTitle.text = bookTitle;
        this.bookText.text = bookText;
    }

    public override void Open ()
    {
        base.Open ();
        isOpened = true;

        scrollRect.verticalNormalizedPosition = 1;

        cGroup.alpha = 1;
        cGroup.blocksRaycasts = true;
        pageText.text = "Page 1";
        this.bookText.pageToDisplay = 1;
        UIPanelController.instance.OnPanelOpened ( this );
    }

    public override void Close (bool bypassCloseCheck = false)
    {
        if (isOpened == false && !bypassCloseCheck) return;

        base.Close ();
        isOpened = true;
        this.bookTitle.text = "";
        this.bookText.text = "";
        cGroup.alpha = 0;
        cGroup.blocksRaycasts = false;
        UIPanelController.instance.OnPanelClosed ( this );
    }

    public void OnClickPreviousPage ()
    {
        int x = Mathf.Clamp ( bookText.pageToDisplay - 1, 1, bookText.textInfo.pageCount );
        pageText.text = "Page " + x;
        bookText.pageToDisplay = x;
    }

    public void OnClickNextPage ()
    {
        int x = Mathf.Clamp ( bookText.pageToDisplay + 1, 1, bookText.textInfo.pageCount );
        pageText.text = "Page " + x;
        bookText.pageToDisplay = x;
    }

}
