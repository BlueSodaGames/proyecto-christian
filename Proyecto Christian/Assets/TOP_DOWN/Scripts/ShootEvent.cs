using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEvent : MonoBehaviour
{
    public BreakableThing[] shooted;
    public GameObject[] objectsToUnlock;
    public bool eventPassed;
    void Update()
    {
        foreach (BreakableThing thing in shooted) {
            eventPassed = true;
            if (!(thing.hitPoints <= 0))
            {
                eventPassed = false;
            }
        }
        if (eventPassed)
        {
            foreach (GameObject gameObject in objectsToUnlock) {
                gameObject.SetActive(true);
            }
        }
    }
}
