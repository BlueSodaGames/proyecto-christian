using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenOrientationManager : MonoBehaviour
{
    [SerializeField] private bool portrait;
    [SerializeField] private bool landscape;
    void Awake()
    {
        if (portrait)
        {
            Screen.orientation = ScreenOrientation.Portrait;
            //PlayerSettings.defaultInterfaceOrientation = UnityEditor.UIOrientation.Portrait;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortraitUpsideDown = false;
            

        }
        else if (landscape)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            //PlayerSettings.defaultInterfaceOrientation = UnityEditor.UIOrientation.LandscapeLeft;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortraitUpsideDown = false;

        }
    }

}
