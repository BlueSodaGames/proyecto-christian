using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private SaveSystemMethods saveSystemMethods;
    [SerializeField] private Button botonContinuar;
    void Start()
    {
        if (saveSystemMethods.HasSaved(1))
        {
            botonContinuar.interactable = true;
        }
        else
        {
            botonContinuar.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
