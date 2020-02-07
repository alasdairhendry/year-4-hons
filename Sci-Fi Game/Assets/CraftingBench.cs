using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBench : MonoBehaviour
{
    [SerializeField] private CraftingTable table;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float maxRestorationRate;
    private float currentEnergy = 0.0f;

    private void Start ()
    {
        currentEnergy = maxEnergy;
    }

    private void Update ()
    {
        RestoreEnergy ();
    }

    public void Interact ()
    {
        CraftingCanvas.instance.SetCraftingTable ( table );
        CraftingCanvas.instance.Open ();
    }

    private void RestoreEnergy ()
    {
        if (currentEnergy <= maxEnergy)
        {
            currentEnergy += Time.deltaTime;

            if (currentEnergy >= maxEnergy)
            {
                currentEnergy = maxEnergy;
            }
        }
    }
}
