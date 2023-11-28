using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsQualityController : MonoBehaviour
{
    public void SetGraphicsQuality(int qualityIndex)
    {
        if (qualityIndex >= 0 && qualityIndex < QualitySettings.names.Length)
        {
            QualitySettings.SetQualityLevel(qualityIndex, true);
        }
    }
}
