using System.Collections;
using System.Collections.Generic;
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

    public void SpawnIndicator (Transform targetTransform, float maxDistance, string text, FloatingTextType type)
    {
        GameObject go = Instantiate ( damageIndicatorPrefab );
        go.transform.SetParent ( this.transform );
        go.transform.position = targetTransform.position + (Random.insideUnitSphere * maxDistance);
        go.transform.localScale = Vector3.one * 0.01f;

        float parse = 0.0f;

        if(float.TryParse(text, out parse ))
        {
            if (parse < 1)
                go.GetComponentInChildren<TextMeshProUGUI> ().text = parse.ToString ( "0.0" );
            else
                go.GetComponentInChildren<TextMeshProUGUI> ().text = Mathf.Floor ( parse ).ToString ( "0" );
        }
        else
        {
            go.GetComponentInChildren<TextMeshProUGUI> ().text = text;

        }
    }
}
