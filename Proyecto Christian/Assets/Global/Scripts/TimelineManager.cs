using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private bool disableInputAtStart;
    [SerializeField] private TopDownPlayerMovement player;
    [SerializeField] private PlayableDirector timeline;
    public GameObject[] objectsToActive;

    public bool finished;
    void Start()
    {
        if (disableInputAtStart)
        {

            player.disableInput = true;
        }
        timeline.stopped += OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector aDirector)
    {
        if (aDirector == timeline)
        {
            player.disableInput = false;
            foreach (GameObject gameObject in objectsToActive)
            {
                gameObject.SetActive(true);
            }
        }
    }


}
