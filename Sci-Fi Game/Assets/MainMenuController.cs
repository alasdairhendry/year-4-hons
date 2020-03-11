using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private SkinnedMeshRenderer characterRenderer;
    [SerializeField] private NPCNavMesh characterNavMesh;
    [SerializeField] private Transform walkInToSceneTransform;
    [SerializeField] private Transform walkOutOfSceneTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private GameObject handMask;
    [SerializeField] private GameObject headMask;
    [SerializeField] private float postHolsterDelay = 0.0f;
    [SerializeField] private float handActivateDelay = 0.0f;
    [SerializeField] private float headActivateDelay = 0.0f;
    [SerializeField] private float startWalkingDelay = 0.0f;
    [Space]
    [SerializeField] private CanvasGroup canvasGroupButtons;
    [SerializeField] private CanvasGroup canvasGroupFaction;
    [SerializeField] private CanvasGroup canvasGroupSettings;
    [Header ( "Faction" )]
    [SerializeField] private List<GameObject> factionButtons = new List<GameObject> ();
    [SerializeField] private List<Faction> factions = new List<Faction> ();
    [Space]
    [SerializeField] private TextMeshProUGUI factionName;
    [SerializeField] private TextMeshProUGUI factionDescription;
    [SerializeField] private TextMeshProUGUI factionSpecialisationName;
    [SerializeField] private TextMeshProUGUI factionSpecialisationDescription;
    [SerializeField] private Image factionIcon;
    [Header ( "Settings" )]
    [SerializeField] private GameObject settingsEntryTemplate;

    private Faction selectedFaction = null;

    private void Awake ()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        canvasGroupButtons.alpha = 1;
        canvasGroupButtons.blocksRaycasts = true;

        canvasGroupFaction.alpha = 0;
        canvasGroupFaction.blocksRaycasts = false;

        canvasGroupSettings.alpha = 0;
        canvasGroupSettings.blocksRaycasts = false;

        CreateFactionButtons ();
        SelectFaction ( factions[0] );

        CreateSettingsSliders ();
    }

    private void Start ()
    {
        DoCharacterWalkIn ();
        characterNavMesh.onPathComplete += OnWalkInComplete;
    }

    public void OnClick_Play ()
    {
        canvasGroupButtons.DOFade ( 0.0f, 0.2f ).OnComplete ( () => { canvasGroupButtons.blocksRaycasts = false; } );
        canvasGroupFaction.DOFade ( 1.0f, 0.2f ).OnComplete ( () => { canvasGroupFaction.blocksRaycasts = true; } );
    }

    public void OnClick_FactionPlay ()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade ( 0.0f, 0.25f );
        StartCoroutine ( DoCharacterLeave () );
        SoundEffectManager.Play ( AudioClipAsset.LevelUp, AudioMixerGroup.SFX, 1.0f );
    }

    private IEnumerator DoCharacterLeave ()
    {
        SetCharacterHolsterState ( true );
        yield return new WaitForSeconds ( postHolsterDelay );
        characterAnimator.SetTrigger ( "equip-mask" );
        yield return new WaitForSeconds ( handActivateDelay );
        handMask.SetActive ( true );
        yield return new WaitForSeconds ( headActivateDelay);
        handMask.SetActive ( false );
        headMask.SetActive ( true );
        yield return new WaitForSeconds ( startWalkingDelay );

        DoCharacterWalkOut ();
        Invoke ( nameof ( LoadGame ), 2.0f );

        yield return null;
    }

    private void LoadGame ()
    {
        LoadingManager.instance.LoadScene ( 1 );
    }

    public void OnClick_Settings ()
    {
        canvasGroupButtons.DOFade ( 0.0f, 0.2f ).OnComplete ( () => { canvasGroupButtons.blocksRaycasts = false; } );
        canvasGroupSettings.DOFade ( 1.0f, 0.2f ).OnComplete ( () => { canvasGroupSettings.blocksRaycasts = true; } );
    }

    public void OnClick_Back ()
    {
        canvasGroupButtons.DOFade ( 1.0f, 0.2f ).OnComplete ( () => { canvasGroupButtons.blocksRaycasts = true; } );
        canvasGroupFaction.DOFade ( 0.0f, 0.2f ).OnComplete ( () => { canvasGroupFaction.blocksRaycasts = false; } );
        canvasGroupSettings.DOFade ( 0.0f, 0.2f ).OnComplete ( () => { canvasGroupSettings.blocksRaycasts = false; } );
    }  

    public void OnClick_Exit ()
    {

    }

    private void CreateFactionButtons ()
    {
        for (int i = 0; i < factionButtons.Count; i++)
        {
            int x = i;
            factionButtons[i].GetComponentInChildren<TextMeshProUGUI> ().text = factions[i].factionName;
            factionButtons[i].GetComponentInChildren<Button> ().onClick.AddListener ( () => { SelectFaction ( factions[x
] ); } );
        }
    }
    
    private void CreateSettingsSliders ()
    {
        CreateSettingsSlider ( "Master Volume", (f) => { AudioMixerManager.instance.SetVolume ( AudioMixerGroup.Master, f ); }, 1.0f );
        CreateSettingsSlider ( "Music Volume", (f) => { AudioMixerManager.instance.SetVolume ( AudioMixerGroup.Music, f ); }, 1.0f );
        CreateSettingsSlider ( "Effects Volume", (f) => { AudioMixerManager.instance.SetVolume ( AudioMixerGroup.SFX, f ); }, 1.0f );
        CreateSettingsSlider ( "Ambient Volume", (f) => { AudioMixerManager.instance.SetVolume ( AudioMixerGroup.Ambient, f ); }, 1.0f );
        CreateSettingsSlider ( "Interface Volume", (f) => { AudioMixerManager.instance.SetVolume ( AudioMixerGroup.UI, f ); }, 1.0f );
        Destroy ( settingsEntryTemplate );
    }

    private void CreateSettingsSlider (string text, System.Action<float> onValueChanged, float initialValue)
    {
        GameObject go = Instantiate ( settingsEntryTemplate );
        go.transform.SetParent ( settingsEntryTemplate.transform.parent );

        go.GetComponentInChildren<TextMeshProUGUI> ().text = text;
        go.GetComponentInChildren<Slider> ().onValueChanged.AddListener ( (f) => { onValueChanged?.Invoke ( f ); } );
        go.GetComponentInChildren<Slider> ().value = initialValue;
    }

    private void SelectFaction(Faction faction)
    {
        factionName.text = faction.factionName;
        factionDescription.text = faction.factionDescription;
        factionSpecialisationName.text = faction.specialisationName;
        factionSpecialisationDescription.text = faction.specialisationDescription;
        factionIcon.sprite = faction.factionSprite;
        selectedFaction = faction;
        PlayerFactionController.chosenFaction = selectedFaction;
        characterRenderer.sharedMaterial = faction.factionPlayerMaterial;
    }

    public void DoCharacterWalkIn ()
    {
        characterNavMesh.SetDestination ( walkInToSceneTransform.position, false, true );
    }

    private void OnWalkInComplete ()
    {
        characterNavMesh.onPathComplete = OnWalkInComplete;
        SetCharacterHolsterState ( false );
        character.transform.localEulerAngles = new Vector3 ( 0.0f, -150.0f, 0.0f );
        canvasGroup.DOFade ( 1.0f, 1.0f );
        canvasGroup.blocksRaycasts = true;
    }

    public void DoCharacterWalkOut ()
    {
        characterNavMesh.SetDestination ( walkOutOfSceneTransform.position, false, true );
    }

    public void SetCharacterHolsterState(bool state)
    {
        character.cWeapon.SetHolsterState ( state );
    }
}
