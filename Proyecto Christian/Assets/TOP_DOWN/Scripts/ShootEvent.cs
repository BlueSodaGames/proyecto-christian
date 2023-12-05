using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEvent : MonoBehaviour
{
    public BreakableThing[] shooted;
    public GameObject[] activated;

    public GameObject[] objectsToUnlock;
    public bool eventPassed = true;

    public enum Condition
    {
        ShootThings,
        ActivateThings
    }

    public Condition condition;
    void Update()
    {
        switch (condition)
        {
            case Condition.ShootThings:
                foreach (BreakableThing thing in shooted)
                {
                    eventPassed = true;
                    if (!(thing.hitPoints <= 0))
                    {
                        eventPassed = false;
                    }
                }
                break;
            case Condition.ActivateThings:
                eventPassed = true;
                foreach (GameObject thing in activated)
                {
                    if (!thing.activeSelf)
                    {
                        eventPassed = false;
                        break;
                    }
                }

                break;
            default:
                break;
        }
        
        if (eventPassed)
        {
            foreach (GameObject gameObject in objectsToUnlock) {
                gameObject.SetActive(true);
            }
        }
    }
}
