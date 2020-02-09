using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XPTrackerUIPrefab : MonoBehaviour
{
    public Animator animator;
    public SkillManager.SkillType skillType;

    private float delay = 2.5f;
    private float currentTime = 0.0f;
    private bool isActive = false;

    [SerializeField] private RectTransform fillImage;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillLevelText;

    public Skill targetSkill
    {
        get
        {
            return SkillManager.instance.GetSkill ( skillType );
        }
    }

    public XPTrackerUIPrefab Initialise(SkillManager.SkillType skillType)
    {
        this.skillType = skillType;
        return this;
    }

    public void OnXpAdded ()
    {
        currentTime = delay;
        SetState ( true );
    }

    private void Update ()
    {
        CheckUI ();
        CheckShouldBeActive ();
    }

    private void CheckUI ()
    {
        if (!isActive) return;

        //float localXScale = Mathf.InverseLerp ( 0.0f, Skill.XPRequiredForLevel ( targetSkill.currentLevel ), targetSkill.currentXP );
        //float damp = (fillImage.localScale.x > localXScale) ? 20.0f : 5.0f;
        //fillImage.localScale = Vector3.Slerp ( fillImage.localScale, new Vector3 ( localXScale, 1.0f, 1.0f ), Time.deltaTime * damp );
        //skillLevelText.text = targetSkill.currentLevel.ToString ();
        //skillNameText.text = targetSkill.skillName;
    }

    private void CheckShouldBeActive ()
    {
        if (currentTime > 0.0f)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0.0f)
            {
                currentTime = 0.0f;
                SetState ( false );
            }
        }
    }

    private void SetState(bool state)
    {
        if (isActive == state) return;

        isActive = state;
        animator.SetBool ( "opened", state );
    }
}
