using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float lifetime = 1.0f;
    [SerializeField] private bool playOnAwake = true;
    public System.Action onDestroy;

    public void Initialise (float lifetime, bool playOnAwake, System.Action onDestroy = null)
    {
        this.lifetime = lifetime;
        this.playOnAwake = playOnAwake;
        this.onDestroy += onDestroy;
    }

    private void Update ()
    {
        lifetime -= Time.deltaTime;

        if(lifetime<= 0.0f)
        {
            Destroy ();
        }
    }

    private void Destroy ()
    {
        onDestroy?.Invoke ();
        Destroy ( this.gameObject );
    }
}
