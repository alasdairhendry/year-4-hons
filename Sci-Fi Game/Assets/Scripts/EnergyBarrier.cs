using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBarrier : MonoBehaviour
{
    [SerializeField] private int tollCost = 10;
    [SerializeField] private List<GameObject> barrierPoles = new List<GameObject> ();
    [SerializeField] private GameObject barrierPrefab;

    private List<GameObject> barriers = new List<GameObject> ();
    private float disabledCounter = 0.0f;

    private void Start ()
    {
        for (int i = 0; i < barrierPoles.Count - 1; i++)
        {
            GameObject go = Instantiate ( barrierPrefab );
            go.transform.position = barrierPoles[i].transform.position;

            Vector3 dir = barrierPoles[i + 1].transform.position - barrierPoles[i].transform.position;
            dir.y = 0.0f;

            go.transform.rotation = Quaternion.LookRotation ( dir );

            float dist = Vector3.Distance ( barrierPoles[i + 1].transform.position, barrierPoles[i].transform.position );
            go.transform.localScale = new Vector3 ( 1.0f, 1.0f, dist );
            barriers.Add ( go );
        }
    }

    private void Update ()
    {
        if(disabledCounter > 0.0f)
        {
            disabledCounter -= Time.deltaTime;

            if(disabledCounter < 0.0f)
            {
                disabledCounter = 0.0f;

                for (int i = 0; i < barriers.Count; i++)
                {
                    barriers[i].SetActive ( true );
                }
            }
        }
    }

    public void Pay ()
    {
        if (disabledCounter > 0)
        {
            MessageBox.AddMessage ( "The toll has already been paid.", MessageBox.Type.Warning );
            return;
        }

        if(EntityManager.instance.PlayerInventory.CheckHasItemQuantity(3, tollCost ))
        {
            MessageBox.AddMessage ( "The pay the toll of 10 crowns to pass through.", MessageBox.Type.Info );
            EntityManager.instance.PlayerInventory.RemoveCoins ( tollCost );

            for (int i = 0; i < barriers.Count; i++)
            {
                barriers[i].SetActive ( false );
                disabledCounter = 5.0f;
            }
        }
        else
        {
            MessageBox.AddMessage ( "The toll costs 10 crowns to pass.", MessageBox.Type.Warning );
        }
    }
}
