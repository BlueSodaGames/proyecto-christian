using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GraphicsQualityController graphicsController;
    public bool menu = false;

    // Start is called before the first frame update
    private void Awake()
    {

        Application.targetFrameRate = 60;        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
