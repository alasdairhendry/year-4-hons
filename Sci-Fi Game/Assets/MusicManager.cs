using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField] private AudioClip initialClip = null;
    [SerializeField] private AudioSource audioSource = null;
    [Space]
    [SerializeField] private List<AudioClip> audioclips = new List<AudioClip> ();

    private float currentCounter = 0.0f;
    bool startFade = false;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy ( this.gameObject );
            return;
        }

        DontDestroyOnLoad ( this.gameObject );
        PlayClip ( initialClip );
    }

    private void Update ()
    {
        currentCounter -= Time.deltaTime;

        if (currentCounter <= 5.0f && startFade == false)
        {
            startFade = true;
            audioSource.DOFade ( 0.0f, 5.0f );
        }

        if (currentCounter <= 0.0f)
        {
            PlayClip ( audioclips.GetRandom () );
        }
    }

    private void PlayClip (AudioClip clip)
    {
        startFade = false;
        currentCounter = clip.length;
        audioSource.clip = clip;
        audioSource.Play ();
        audioSource.DOFade ( 0.05f, 1.5f );
    }
}
