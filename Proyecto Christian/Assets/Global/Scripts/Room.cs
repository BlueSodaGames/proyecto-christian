using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject virtualCamera;


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (objects != null)
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    if (objects[i] != null)
                    {
                        ChangeActivation(objects[i], true);
                    }
                }
            }

            virtualCamera.SetActive(true);
            //StartCoroutine(TransitionDelay());
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (objects != null)
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    if (objects[i] != null)
                    {
                        ChangeActivation(objects[i], false);
                    }
                }
            }

            virtualCamera.SetActive(false);

        }
    }

    public void ChangeActivation(GameObject component, bool activation)
    {
        component.SetActive(activation);
    }

    //private IEnumerator TransitionDelay()
    //{

    //    Time.timeScale = 0f; // Congelar el tiempo

    //    yield return new WaitForSecondsRealtime(0.5f); // Ajusta el tiempo de espera según tus necesidades

    //    Time.timeScale = 1f; // Reanudar el tiempo
    //}
}
