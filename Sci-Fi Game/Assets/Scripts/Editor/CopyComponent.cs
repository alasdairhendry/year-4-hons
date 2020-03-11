using UnityEditor;
using UnityEngine;

public static class CopyComponent
{
    public static Vector3 pos;
    public static Vector3 rot;
    public static Vector3 scale;

    [MenuItem ( "Tools/Copy Transform" , priority = 9999)]
    public static void CopyTransform ()
    {
        if (Selection.objects[0] as TransformData != null)
        {
            pos = (Selection.objects[0] as TransformData).position;
            rot = (Selection.objects[0] as TransformData).eulerAngles;
            scale = (Selection.objects[0] as TransformData).localScale;
            Debug.Log ( "Copied Transform Data" );
        }
        else if (Selection.objects[0] as WeaponData != null)
        {
            pos = (Selection.objects[0] as WeaponData).offsetPosition;
            rot = (Selection.objects[0] as WeaponData).offsetRotation;
            scale = (Selection.objects[0] as WeaponData).localScale;
            Debug.Log ( "Copied Weapon IK Data" );
        }
        else if (Selection.objects[0] as GearData != null)
        {
            pos = (Selection.objects[0] as GearData).offsetPosition;
            rot = (Selection.objects[0] as GearData).offsetRotation;
            scale = (Selection.objects[0] as GearData).localScale;
            Debug.Log ( "Copied Gear Equip Data" );
        }
        else
        {
            pos = Selection.gameObjects[0].GetComponent<Transform> ().localPosition;
            rot = Selection.gameObjects[0].GetComponent<Transform> ().localEulerAngles;
            scale = Selection.gameObjects[0].GetComponent<Transform> ().localScale;
            Debug.Log ( "Copied Transform Component" );
        }
    }

    [MenuItem ( "Tools/Paste Weapon Transform" , priority = 10000)]
    public static void PasteTransform ()
    {
        WeaponData weaponData = Selection.objects[0] as WeaponData;
        GearData gearData = Selection.objects[0] as GearData;

        if(weaponData != null)
        {
            ((WeaponData)Selection.objects[0]).offsetPosition = pos;
            ((WeaponData)Selection.objects[0]).offsetRotation = rot;
            ((WeaponData)Selection.objects[0]).localScale = scale;
            EditorUtility.SetDirty ( Selection.objects[0] );
            AssetDatabase.SaveAssets ();
            Debug.Log ( "Pasted weapon IK data" );
        }
        else if(gearData != null)
        {
            ((GearData)Selection.objects[0]).offsetPosition = pos;
            ((GearData)Selection.objects[0]).offsetRotation = rot;
            ((GearData)Selection.objects[0]).localScale = scale;
            EditorUtility.SetDirty ( Selection.objects[0] );
            AssetDatabase.SaveAssets ();
            Debug.Log ( "Pasted transform data to gear" );
        }
        else
        {
            TransformData transformData = Selection.objects[0] as TransformData;

            if (transformData == null)
            {
                Debug.LogError ( "No weapon or transform data found" );
            }
            else
            {
                ((TransformData)Selection.objects[0]).position = pos;
                ((TransformData)Selection.objects[0]).eulerAngles = rot;
                ((TransformData)Selection.objects[0]).localScale = scale;
                EditorUtility.SetDirty ( Selection.objects[0] );
                AssetDatabase.SaveAssets ();
                Debug.Log ( "Pasted transform data to transform data" );
            }
        }
    }
}