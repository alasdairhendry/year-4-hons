using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpCanvas : MonoBehaviour
{
    public static LevelUpCanvas instance;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI text;
    [Space]
    [SerializeField] private float animationLength = 4.5f;

    private float counter = 0.0f;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }

    private void Update ()
    {
        if (counter > 0)
        {
            counter -= Time.deltaTime;

            if (counter < 0) counter = 0;
        }
    }

    public void Show(string levelName)
    {
        if (counter > 0) return;

        text.text = levelName;
        animator.SetTrigger ( "Show" );
        counter = animationLength;
    }
}
