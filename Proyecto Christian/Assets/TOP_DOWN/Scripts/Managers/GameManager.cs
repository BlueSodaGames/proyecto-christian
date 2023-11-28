using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject volume;
    public GraphicsQualityController graphicsController;
    public bool menu = false;

    // Start is called before the first frame update
    private void Awake()
    {
        graphicsController.SetGraphicsQuality(0);

        if (!menu)
        {
            if (PlayerPrefs.GetInt("LimitarFPSActivado", 1) == 1)
            {
                Application.targetFrameRate = 60;
            }
            else
            {
                Application.targetFrameRate = -1;
            }
        }
        else
        {
            Application.targetFrameRate = 60;
        }
            




        if (volume != null)
        {
            if (PlayerPrefs.GetInt("PostprocesadoActivado", 0) == 0)
            {
                volume.SetActive(false);
            }
            else
            {
                volume.SetActive(true);
            }
        }
        
    }
}
