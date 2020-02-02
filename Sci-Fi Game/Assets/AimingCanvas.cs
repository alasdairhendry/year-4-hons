using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReticleType { Off, CanFire, CantFire }

public class AimingCanvas : MonoBehaviour
{
    public static AimingCanvas instance;

    public ReticleType ReticleType { get; protected set; }

    [SerializeField] private GameObject canFireReticle;
    [SerializeField] private GameObject cantFireReticle;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy ( this.gameObject );
            return;
        }

        canFireReticle.SetActive ( false );
        cantFireReticle.SetActive ( false );
        ReticleType = ReticleType.Off;
    }

    public void SetReticle(ReticleType reticleType)
    {
        if (ReticleType == reticleType) return;
        ReticleType = reticleType;

        switch (ReticleType)
        {
            case ReticleType.CanFire:
                canFireReticle.SetActive ( true );
                cantFireReticle.SetActive ( false );
                break;
            case ReticleType.CantFire:
                canFireReticle.SetActive ( false );
                cantFireReticle.SetActive ( true );
                break;
            case ReticleType.Off:
                canFireReticle.SetActive ( false );
                cantFireReticle.SetActive ( false );
                break;
        }

    }
}
