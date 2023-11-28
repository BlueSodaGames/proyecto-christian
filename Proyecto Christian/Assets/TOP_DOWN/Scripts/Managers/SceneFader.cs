using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneFader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;



    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        Resources.UnloadUnusedAssets();
    }

    public void LoadIndexLevel(int indexLevel)
    {
        StartCoroutine(LoadLevel(indexLevel));
        Resources.UnloadUnusedAssets();
    }

    public IEnumerator LoadLevel(int levelIndex)
    {
        AudioManager.Instance.PlaySFX("SceneChange");
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }
        

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }



}
