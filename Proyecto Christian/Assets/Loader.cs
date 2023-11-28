using PixelCrushers.Wrappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public SaveSystemMethods saveSystemMethods;
    public int slot;


    private void Start()
    {
        saveSystemMethods.LoadFromSlot(slot);
    }
}
