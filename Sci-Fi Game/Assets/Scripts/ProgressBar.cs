using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [Space]
    [SerializeField] private float damping = 5.0f;
    [SerializeField] private float minPosition = 0.0f;
    [SerializeField] private float maxPosition = 100.0f;

    [SerializeField] [Range ( 0.0f, 1.0f )] private float currentValue = 0.0f;
    private float targetValue = 0.0f;

    public void SetValue(float value)
    {
        targetValue = value;
    }

    private void Update ()
    {
        if (currentValue < targetValue)
        {
            currentValue += Time.deltaTime * damping * Mathf.Abs(targetValue - currentValue);

            if (currentValue >= targetValue)
            {
                currentValue = targetValue;
            }

            SetPosition ();
        }
        else if (currentValue > targetValue)
        {
            currentValue -= Time.deltaTime * damping * Mathf.Abs ( targetValue - currentValue );

            if (currentValue <= targetValue)
            {
                currentValue = targetValue;
            }

            SetPosition ();
        }
    }

    private void SetPosition ()
    {
        float targetX = Mathf.Lerp ( minPosition, maxPosition, currentValue );

        Vector3 targetPosition = target.anchoredPosition3D;
        targetPosition.x = targetX;

        target.anchoredPosition3D = targetPosition;
    }

    private void OnValidate ()
    {
        if(target == null)
        {
            target = transform.GetChild ( 0 ).GetComponent<RectTransform> ();

            if(target == null)
            {
                Debug.LogError ( "No target found" );
            }
        }

        SetPosition ();
    }
}
