using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    [SerializeField] private List<SkinnedMeshRenderer> skinnedMeshRenderers = new List<SkinnedMeshRenderer> ();
    [SerializeField] private List<Material> materials = new List<Material> ();

    [SerializeField] private new Camera camera;

    private void Update ()
    {
        //if (Input.GetKeyDown ( KeyCode.Space ))
        //{
        //    StartCoroutine ( TakeScreenshots () );
        //}
        
    }

    private IEnumerator TakeScreenshots ()
    {
        for (int i = 0; i < GetComponentsInChildren<Animator>().Length; i++)
        {
            GetComponentsInChildren<Animator> ()[i].enabled = false;
        }

        camera.clearFlags = CameraClearFlags.Depth;

        int mrCount = 0;
        int matCount = 0;

        Debug.Log ( mrCount );
        Debug.Log ( skinnedMeshRenderers.Count );

        for (int i = 0; i < skinnedMeshRenderers.Count; i++)
        {
            skinnedMeshRenderers[i].gameObject.SetActive ( false );
        }

        Debug.Log ( "Start" );

        while (mrCount < skinnedMeshRenderers.Count)
        {
            skinnedMeshRenderers[mrCount].gameObject.SetActive ( true );

            while (matCount < materials.Count)
            {
                skinnedMeshRenderers[mrCount].material = materials[matCount];
                TakeScreenshot ( string.Format ( "{0}/Resources/ActorSprites/{1} ({2}).png", Application.dataPath, skinnedMeshRenderers[mrCount].sharedMesh.name, materials[matCount].name ) );
                GL.Clear ( true, true, Color.black );
                matCount++;
                //yield break;
                yield return new WaitForEndOfFrame ();
            }

            skinnedMeshRenderers[mrCount].gameObject.SetActive ( false );
            matCount = 0;
            mrCount++;
            yield return null;
        }

        Debug.Log ( "Finished" );
    }

    public int resWidth = 256;
    public int resHeight = 256;

    //public static string ScreenShotName (int width, int height)
    //{
    //    return string.Format ( "{0}/screenshots/screen_{1}x{2}_{3}.png",
    //                         Application.dataPath,
    //                         width, height,
    //                         System.DateTime.Now.ToString ( "yyyy-MM-dd_HH-mm-ss" ) );
    //}

    private void TakeScreenshot (string filename)
    {
        RenderTexture rt = new RenderTexture ( resWidth, resHeight, 24 );
        rt.antiAliasing = 12;
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D ( resWidth, resHeight, TextureFormat.RGBA32, false );
        for (int x = 0; x < resWidth; x++)
        {
            for (int y = 0; y < resHeight; y++)
            {
                screenShot.SetPixel ( x, y, new Color ( 0.0f, 0.0f, 0.0f, 0.0f ) );
            }
        }

        screenShot.Apply ();


        camera.Render ();
        RenderTexture.active = rt;
        screenShot.ReadPixels ( new Rect ( 0, 0, resWidth, resHeight ), 0, 0 );

       

        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy ( rt );
        byte[] bytes = screenShot.EncodeToPNG ();
        System.IO.File.WriteAllBytes ( filename, bytes );
        Debug.Log ( string.Format ( "Took screenshot to: {0}", filename ) );
    }
}
