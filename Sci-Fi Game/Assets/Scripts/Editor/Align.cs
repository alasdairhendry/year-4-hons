using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Align
{
    public enum Axis { X, Y, Z }

    [MenuItem ( "Tools/Align By/1x" )]
    public static void AlignBy1X ()
    {
        AlignBy ( Axis.X, 1.0f );
    }

    [MenuItem ( "Tools/Align By/2x" )]
    public static void AlignBy2X ()
    {
        AlignBy ( Axis.X, 2.0f );
    }

    [MenuItem ( "Tools/Align By/5x" )]
    public static void AlignBy5X ()
    {
        AlignBy ( Axis.X, 5.0f );
    }

    private static void AlignBy (Axis axis, float increment)
    {
        GameObject[] objects = Selection.gameObjects;

        float index = 0;

        for (int i = 0; i < objects.Length; i++)
        {
            if (i == 0.0f) index = objects[i].transform.position.x;

            Undo.RecordObject ( objects[i], "Align multiple by 1x" );

            switch (axis)
            {
                case Axis.X:
                    objects[i].transform.position = new Vector3 ( index, objects[i].transform.position.y, objects[i].transform.position.z );
                    break;
                case Axis.Y:
                    objects[i].transform.position = new Vector3 ( objects[i].transform.position.x, index, objects[i].transform.position.z );
                    break;
                case Axis.Z:
                    objects[i].transform.position = new Vector3 ( objects[i].transform.position.x, objects[i].transform.position.y, index );
                    break;
                default:
                    objects[i].transform.position = new Vector3 ( index, objects[i].transform.position.y, objects[i].transform.position.z );
                    break;
            }

            EditorUtility.SetDirty ( objects[i] );

            index += increment;
        }
    }
}
