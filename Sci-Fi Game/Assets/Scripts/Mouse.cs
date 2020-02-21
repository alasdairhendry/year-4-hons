using QuestFlow;
using QuestFlow.DialogueEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mouse : MonoBehaviour
{
    public enum CursorType { Default, Interact, Talk, Attack, Open, Loot }

    [SerializeField] private List<CursorTypeData> cursorData = new List<CursorTypeData> ();

    [System.Serializable]
    private class CursorTypeData
    {
        public CursorType cursorType = CursorType.Default;
        public Texture2D texture;
        public Texture2D disabledTexture;
        public Vector2 cursorHotspot = new Vector2 ();
    }

    private static Mouse instance;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        SetCursor ( CursorType.Default );
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private static Vector3 leftMousePosition = new Vector3 ();
    private static Vector3 rightMousePosition = new Vector3 ();
    private static Vector3 middleMousePosition = new Vector3 ();
    private static Vector3 previousMousePosition = new Vector3 ();
    private List<KeyCode> hotkeyNumbers = new List<KeyCode> { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };

    private void Update ()
    {
        if (Input.GetMouseButtonDown ( 0 ))
        {
            leftMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown ( 1 ))
        {
            rightMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown ( 2 ))
        {
            middleMousePosition = Input.mousePosition;
        }

        CheckNumberedHotkeys ();
        CheckLetteredHotkeys ();
    }

    private void CheckLetteredHotkeys ()
    {
        if (Input.GetKeyDown ( KeyCode.G ))
            GearCanvas.instance.Trigger ();

        if (Input.GetKeyDown ( KeyCode.I ))
            EntityManager.instance.InventoryCanvas.Trigger ();

        if (Input.GetKeyDown ( KeyCode.J ))
            QuestLogCanvas.instance.Trigger ();

        if (Input.GetKeyDown ( KeyCode.K ))
            SkillsCanvas.instance.Trigger ();

        if (Input.GetKeyDown ( KeyCode.C ))
            MessageBox.instance.Trigger ();

        if (Input.GetKeyDown ( KeyCode.M ))
            WorldMapCanvas.instance.Trigger ();

        if (Input.GetKeyDown ( KeyCode.N ))
            TalentCanvas.instance.Trigger ();

        //if (Input.GetKeyDown ( KeyCode.H ))
        //    GearCanvas.instance.Trigger ();

        //if (Input.GetKeyDown ( KeyCode.P ))
        //    GearCanvas.instance.Trigger ();
    }

    private void CheckNumberedHotkeys ()
    {
        if (DialogueManager.instance.ConversationIsActive)
        {
            for (int i = 0; i < hotkeyNumbers.Count; i++)
            {
                if (Input.GetKeyDown ( hotkeyNumbers[i] ))
                {
                    DialogueManager.instance.OnHotkeyPressed ( hotkeyNumbers[i], Input.GetKey ( KeyCode.LeftShift ), Input.GetKey ( KeyCode.LeftControl ), Input.GetKey ( KeyCode.LeftAlt ) );
                }
            }

            if (Input.GetKeyDown ( KeyCode.Space ))
            {
                DialogueManager.instance.OnHotkeyPressed ( KeyCode.Space, Input.GetKey ( KeyCode.LeftShift ), Input.GetKey ( KeyCode.LeftControl ), Input.GetKey ( KeyCode.LeftAlt ) );
            }
        }
        else if (TeleportCanvas.instance.isOpened)
        {
            for (int i = 0; i < hotkeyNumbers.Count; i++)
            {
                if (Input.GetKeyDown ( hotkeyNumbers[i] ))
                {
                    TeleportCanvas.instance.OnHotkeyPressed ( hotkeyNumbers[i], Input.GetKey ( KeyCode.LeftShift ), Input.GetKey ( KeyCode.LeftControl ), Input.GetKey ( KeyCode.LeftAlt ) );
                }
            }
        }
        else
        {
            for (int i = 0; i < hotkeyNumbers.Count; i++)
            {
                if (Input.GetKeyDown ( hotkeyNumbers[i] ))
                {
                    HotbarCanvas.instance.OnHotkeyPressed ( hotkeyNumbers[i], Input.GetKey ( KeyCode.LeftShift ), Input.GetKey ( KeyCode.LeftControl ), Input.GetKey ( KeyCode.LeftAlt ) );
                }
            }
        }
    }

    private void LateUpdate ()
    {
        previousMousePosition = Input.mousePosition;
    }

    public static bool Moved ()
    {
        return previousMousePosition != Input.mousePosition;
    }

    public static bool Click (int index, float threshold = 32.0f)
    {
        if (instance == null) return false;

        Vector3 targetPosition = leftMousePosition;

        if (index == 1)
        {
            targetPosition = rightMousePosition;
        }
        else if (index == 2)
        {
            targetPosition = middleMousePosition;
        }

        if (Up ( index ))
        {
            if(Extensions.SquaredDistance(targetPosition, Input.mousePosition) > threshold)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    public static bool Up (int index)
    {
        if (instance == null) return false;
        return Input.GetMouseButtonUp ( index );
    }

    public static bool Down (int index, bool canBeOverUI = true)
    {
        if (instance == null) return false;
        if (!canBeOverUI && EventSystem.current.IsPointerOverGameObject ()) return false;
        return Input.GetMouseButtonDown ( index );
    }

    public static bool DownRepeating (int index, bool canBeOverUI = true)
    {
        if (instance == null) return false;
        if (!canBeOverUI && EventSystem.current.IsPointerOverGameObject ()) return false;
        return Input.GetMouseButton ( index );
    }

    public static void SetCursor(CursorType cursorType, bool disabled = false)
    {
        if (EventSystem.current.IsPointerOverGameObject ())
        {
            CursorTypeData data = instance.cursorData.FirstOrDefault ( x => x.cursorType == CursorType.Default );
            Cursor.SetCursor ( data.texture, data.cursorHotspot, CursorMode.Auto );
        }
        else
        {
            CursorTypeData data = instance.cursorData.FirstOrDefault ( x => x.cursorType == cursorType );
            Cursor.SetCursor ( disabled ? data.disabledTexture : data.texture, data.cursorHotspot, CursorMode.Auto );
        }
    }

    public static void SetCursorState (CursorLockMode mode, bool visible)
    {
        Cursor.lockState = mode;
        Cursor.visible = visible;
    }
}
