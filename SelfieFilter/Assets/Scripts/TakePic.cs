using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;



public class TakePic : MonoBehaviour
{
    private string nameToSave = "Test";
    private int photoHeight;
    private int photoWidth;
    private Rect namePicRect;
    private Rect takePicRect;
    private Rect applyFilterRect;
    private Rect activePicRect;
    private Rect finalPicsRect;
    private Rect saveRect;
    private Rect inputRect;
    private Rect quitRect;
    private Texture2D activePic = null;
    private Texture2D finalPics = null;
    private List<Color32> listFilters = new List<Color32>();
    private List<Texture2D> listTextures = new List<Texture2D>();
    public RenderTexture rt;
    public Material mat;
    WebCamTexture webCamTexture;
    public int fileCounter;


    void Start()
    {
        webCamTexture = new WebCamTexture();
        webCamTexture.Play();
        
        Debug.Log(webCamTexture.width + "/" + webCamTexture.height);


        float heightFloat = Screen.height * 0.2f;
        photoHeight = (int)(Screen.height * 0.2f);
        float rate = heightFloat / webCamTexture.height;      //This is to set up that photo taken is not larger than 20% of the screen and it is easily handled by our buttons. 
        photoWidth = (int)(rate * webCamTexture.width);
        Debug.Log(photoHeight + "/" + photoWidth);
        activePicRect = new Rect(Screen.width * 0.5f, 0, photoWidth, photoHeight);

        takePicRect = new Rect(0, 0, Screen.height * 0.2f, Screen.height * 0.2f);
        applyFilterRect = new Rect(0, Screen.height * 0.2f, Screen.height * 0.2f, Screen.height * 0.2f);
        saveRect = new Rect(0, Screen.height * 0.4f, Screen.height * 0.2f, Screen.height * 0.2f);
        quitRect = new Rect(0, Screen.height * 0.6f, Screen.height * 0.2f, Screen.height * 0.2f);


        inputRect = new Rect(Screen.height * 0.2f, Screen.height * 0.4f, Screen.height * 0.2f, Screen.height * 0.2f);
        finalPicsRect = new Rect(Screen.width * 0.5f, photoHeight, photoWidth * 2, photoHeight * 2);

        //Declare filters you want; You may change values, or even check to use Shaders? Enter 0 for colors which dont need to be changed.
        //Scripts is optimized to apply only one color in the if / if else staments. Change to only if should more than one color 
        //would be needed to apply. 

        listFilters.Add(new Color32(255, 0, 0, 0));
        listFilters.Add(new Color32(0, 255, 0, 0));
        listFilters.Add(new Color32(0, 0, 255, 0));
        listFilters.Add(new Color32(0, 0, 0, 255));
    }



    IEnumerator TakePhoto()  // Start this Coroutine on some button click
    {
        listTextures.Clear();
        yield return new WaitForEndOfFrame();

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();
        activePic = photo;
        listTextures.Add(photo);
        Debug.Log("done");
    }




    private void JoinPictures ()
    {
        finalPics = new Texture2D((activePic.width * 2), activePic.height * 2);
        Debug.Log(finalPics.width + "/" + finalPics.height);

        for (int y = 0; y <finalPics.height; y ++)
        {

            for (int x = 0; x < finalPics.width; x++)
            {
                if (y < activePic.height)
                {
                    if (x < activePic.width)
                    {
                        Color32 initialColor = activePic.GetPixel(x, y);
                        Color32 finalColor = new Color32(initialColor.r, initialColor.g, 255, initialColor.a);
                        finalPics.SetPixel(x, y, finalColor);
                    }
                    else
                    {
                        Color32 initialColor = activePic.GetPixel(x - activePic.width, y);
                        Color32 finalColor = new Color32(initialColor.r, 255, initialColor.b, initialColor.a);
                        finalPics.SetPixel(x, y, finalColor);
                    }
                }
                else
                {
                    if (x < activePic.width)
                    {
                        Color32 initialColor = webCamTexture.GetPixel(x, y - activePic.height);
                        Color32 finalColor = new Color32(255, initialColor.g, initialColor.b, initialColor.a);
                        finalPics.SetPixel(x, y, finalColor);
                    }
                    else
                    {
                        Color32 initialColor = activePic.GetPixel(x - activePic.width, y - activePic.height);
                        Color32 finalColor = new Color32(200, initialColor.g, 200, initialColor.a);
                        finalPics.SetPixel(x, y, finalColor);
                    }
                }
            }
        }    
        finalPics.Apply();
    }



    private void OnGUI()
    {
        if (GUI.Button(takePicRect, "TAKE PIC"))
        {
            webCamTexture.Play();
            StartCoroutine("TakePhoto");
        }

        if (GUI.Button(quitRect, "QUIT"))
        {
            Application.Quit();
        }

        if (activePic != null)
        {
            if (GUI.Button(applyFilterRect, "APPLY FILTER"))
            {
                JoinPictures();
            }

            if (finalPics != null)
            {
                if (GUI.Button(saveRect, "SAVE"))
                {
                    SavePictures();
                }
            }
        }
        nameToSave = GUI.TextField(inputRect, nameToSave, 20);

        if (activePic != null)
        {            
            GUI.DrawTexture(activePicRect, activePic);
            if (finalPics != null)
            {
                GUI.DrawTexture(finalPicsRect, finalPics);
            }
        }
    }

    private void SavePictures ()
    {
        Debug.Log(Application.persistentDataPath);
        File.WriteAllBytes(Application.persistentDataPath + "/" + nameToSave + ".png", finalPics.EncodeToPNG());
    }
}
