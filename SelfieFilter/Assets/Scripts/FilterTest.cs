//This script changes the filter mode of your Texture you attach when you press the space key in Play Mode. It switches between Point, Bilinear and Trilinear filter modes.
//Attach this script to a GameObject
//Click on the GameObject and attach a Texture to the My Texture field in the Inspector.
//Apply the Texture to GameObjects (click and drag the Texture onto a GameObject in Editor mode) in your Scene to see it change modes in-game.

using UnityEngine;

public class FilterTest : MonoBehaviour
{
    //Remember to assign a Texture in the Inspector window to ensure this works
    public Texture m_MyTexture;

    void Update()
    {
        //Press the space key to switch between Filter Modes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Switch the Texture's Filter Mode
            m_MyTexture.filterMode = SwitchFilterModes();
            //Output the current Filter Mode to the console
            Debug.Log("Filter mode : " + m_MyTexture.filterMode);
        }
    }

    //Switch between Filter Modes when the user clicks the Button
    FilterMode SwitchFilterModes()
    {
        //Switch the Filter Mode of the Texture when user clicks the Button
        switch (m_MyTexture.filterMode)
        {
            //If the FilterMode is currently Bilinear, switch it to Point on the Button click
            case FilterMode.Bilinear:
                m_MyTexture.filterMode = FilterMode.Point;
                break;

            //If the FilterMode is currently Point, switch it to Trilinear on the Button click
            case FilterMode.Point:
                m_MyTexture.filterMode = FilterMode.Trilinear;
                break;

            //If the FilterMode is currently Trilinear, switch it to Bilinear on the Button click
            case FilterMode.Trilinear:
                m_MyTexture.filterMode = FilterMode.Bilinear;
                break;
        }
        //Return the new Texture FilterMode
        return m_MyTexture.filterMode;
    }
}