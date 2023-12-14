using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PixelCrushers;

public class SceneFader : MonoBehaviour
{
    public float transitionTime = 1f;
    public GameObject blackFlash;
    public GameObject blackFadeStart;

    public void Start()
    {
        blackFadeStart.SetActive(true);
    }

    public void NewGame()
    {
        StartCoroutine(LoadLevel("TopDown_1", ""));
    }

    public void TitleScreen()
    {
        StartCoroutine(LoadLevel("TopDown_0", ""));
    }

    public void LoadIndexLevel(string levelName, string spawnpoint)
    {
        StartCoroutine(LoadLevel(levelName, spawnpoint));
    }

    public IEnumerator LoadLevel(string levelName, string spawnpoint)
    {
        Resources.UnloadUnusedAssets();

        //AudioManager.Instance.PlaySFX("SceneChange");
        blackFlash.SetActive(true);

        yield return new WaitForSeconds(transitionTime);
        if (spawnpoint == "" || spawnpoint == null)
        {
            SaveSystem.LoadScene(levelName);
        }
        else
        {
            SaveSystem.LoadScene(levelName + "@" + spawnpoint);
        }
        
    }

}
