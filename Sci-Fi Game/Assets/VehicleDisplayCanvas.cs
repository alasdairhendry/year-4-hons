using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDisplayCanvas : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject switchText;

    private void Update ()
    {
        if (EntityManager.instance)
        {
            if (EntityManager.instance.PlayerCharacter)
            {
                if (EntityManager.instance.PlayerCharacter.currentVehicle)
                {
                    mainPanel.SetActive ( true );

                    if (EntityManager.instance.PlayerCharacter.currentVehicle is VehicleBaseMulti)
                    {
                        switchText.SetActive ( true );
                    }
                    else
                    {
                        switchText.SetActive ( false );
                    }

                    return;
                }
            }
        }

        mainPanel.SetActive ( false );
    }
}
