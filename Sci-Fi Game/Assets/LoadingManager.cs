using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy ( this.gameObject );
            return;
        }

        DontDestroyOnLoad ( this.gameObject );
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene (int index)
    {
        canvasGroup.DOFade ( 1.0f, 0.5f ).OnComplete ( () => { SceneManager.LoadScene ( index ); } );
        canvasGroup.blocksRaycasts = true;
    }

    private void OnSceneLoaded (Scene arg0, LoadSceneMode arg1)
    {
        canvasGroup.DOFade ( 0.0f, 0.5f );
        canvasGroup.blocksRaycasts = false;
    }
}
