using TMPro;
using UnityEngine;

public class DamageCanvas : MonoBehaviour
{
    public static DamageCanvas instance;
    [SerializeField] private GameObject damageIndicatorPrefab;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy ( this.gameObject );
            return;
        }
    }

    public void SpawnDamageIndicator(Transform targetTransform, float maxDistance, float amount, bool isCritical = false, bool isPlayer = false)
    {
        GameObject go = Instantiate ( damageIndicatorPrefab );
        go.transform.SetParent ( this.transform );
        go.transform.position = targetTransform.position + (Random.insideUnitSphere * maxDistance);

        float scaleModifier = 0.01f;

        if (isPlayer)
        {
            scaleModifier *= 0.75f;
        }

        if (isCritical)
        {
            scaleModifier *= 2.0f;
        }

        go.transform.localScale = Vector3.one * scaleModifier;

        go.GetComponentInChildren<TextMeshProUGUI> ().text = Mathf.Floor ( amount ).ToString ( "0" );

        if (!isPlayer)
        {
            if (isCritical)
            {
                go.GetComponentInChildren<TextMeshProUGUI> ().color = Color.yellow;
            }
            else
            {
                go.GetComponentInChildren<TextMeshProUGUI> ().color = new Color ( 0.9f, 0.9f, 0.9f, 1.0f );
            }
        }
    }

    public void SpawnHealIndicator (Transform targetTransform, float maxDistance, float amount, bool isCritical = false, bool isPlayer = false)
    {
        GameObject go = Instantiate ( damageIndicatorPrefab );
        go.transform.SetParent ( this.transform );
        go.transform.position = targetTransform.position + (Random.insideUnitSphere * maxDistance);

        float scaleModifier = 0.01f;

        if (isPlayer)
        {
            scaleModifier *= 0.75f;
        }

        if (isCritical)
        {
            scaleModifier *= 2.0f;
        }

        go.transform.localScale = Vector3.one * scaleModifier;
        go.GetComponentInChildren<TextMeshProUGUI> ().text = "+" + Mathf.Floor ( amount ).ToString ( "0" );

        if (!isPlayer)
        {
            if (isCritical)
            {
                go.GetComponentInChildren<TextMeshProUGUI> ().color = Color.yellow;
            }
            else
            {
                go.GetComponentInChildren<TextMeshProUGUI> ().color = new Color ( 0.9f, 0.9f, 0.9f, 1.0f );
            }
        }
        else
        {
            go.GetComponentInChildren<TextMeshProUGUI> ().color = Color.cyan;
        }
    }

    public void SpawnInfoText (Transform targetTransform, float maxDistance, string text, ColourDescription colourDescription = ColourDescription.None)
    {
        GameObject go = Instantiate ( damageIndicatorPrefab );
        go.transform.SetParent ( this.transform );
        go.transform.position = targetTransform.position + (Random.insideUnitSphere * maxDistance);

        float scaleModifier = 0.01f;

        go.transform.localScale = Vector3.one * scaleModifier;

        go.GetComponentInChildren<TextMeshProUGUI> ().text = text;

        if (colourDescription != ColourDescription.None)
            go.GetComponentInChildren<TextMeshProUGUI> ().color = ColourHelper.GetEditorColour ( colourDescription );
    }


    //public void SpawnIndicator (Transform targetTransform, float maxDistance, string text, FloatingTextType type)
    //{
    //    GameObject go = Instantiate ( damageIndicatorPrefab );
    //    go.transform.SetParent ( this.transform );
    //    go.transform.position = targetTransform.position + (Random.insideUnitSphere * maxDistance);
    //    go.transform.localScale = Vector3.one * 0.01f;

    //    float parse = 0.0f;

    //    if(float.TryParse(text, out parse ))
    //    {
    //        if (parse < 1)
    //            go.GetComponentInChildren<TextMeshProUGUI> ().text = parse.ToString ( "0.0" );
    //        else
    //            go.GetComponentInChildren<TextMeshProUGUI> ().text = Mathf.Floor ( parse ).ToString ( "0" );
    //    }
    //    else
    //    {
    //        go.GetComponentInChildren<TextMeshProUGUI> ().text = text;
    //    }
    //}
}
