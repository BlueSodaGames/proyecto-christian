using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MusicEvent : MonoBehaviour
{
    public ShootEvent shootEvent;
    public GameObject timelineMusic;
    void Update()
    {
        if (shootEvent.eventPassed) {
            timelineMusic.gameObject.SetActive(true);
        }
    }
}
