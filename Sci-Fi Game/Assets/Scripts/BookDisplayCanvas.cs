using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookDisplayCanvas : MonoBehaviour
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

        Hide ();
    }

    public void Show (string bookTitle, string bookText)
    {
        this.bookTitle.text = bookTitle;
        this.bookText.text = bookText;

        scrollRect.verticalNormalizedPosition = 1;

        cGroup.alpha = 1;
        cGroup.blocksRaycasts = true;
        pageText.text = "Page 1";
        this.bookText.pageToDisplay = 1;
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

    public void Hide ()
    {
        this.bookTitle.text = "";
        this.bookText.text = "";
        cGroup.alpha = 0;
        cGroup.blocksRaycasts = false;
    }
}
