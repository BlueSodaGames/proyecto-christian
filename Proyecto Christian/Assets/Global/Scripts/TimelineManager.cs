using PixelCrushers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private bool disableInputAtStart;
    [SerializeField] private TopDownPlayerMovement player;
    [SerializeField] private PlatformerPlayerMovement playerPlatformer;
    [SerializeField] private AdventurePlayerMovement playerAdventure;
    [SerializeField] private PlayableDirector timeline;
    public GameObject[] objectsToActive;
    public GameObject[] objectsToDeactive;
    public ActiveSaver timelineReproducedSaver;


    public bool finished;
    void Start()
    {
        if (disableInputAtStart)
        {
            if (player)
            {
                player.disableInput = true;
            }
            else if (playerPlatformer)
            {
                playerPlatformer.canMove = false;
            }
            else if (playerAdventure)
            {
                
            }
            
        }
        timeline.stopped += OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector aDirector)
    {
        if (aDirector == timeline)
        {
            if (player)
            {
                player.disableInput = false;
            }
            else if (playerPlatformer)
            {
                playerPlatformer.canMove = true;
            }
            else if (playerAdventure)
            {

            }
            if (timelineReproducedSaver != null)
            {
                timelineReproducedSaver.enabled = true;
            }

            if (objectsToActive.Length > 0)
            {
                foreach (GameObject gameObject in objectsToActive)
                {
                    gameObject.SetActive(true);
                }
            }

            if (objectsToDeactive.Length > 0)
            {
                foreach (GameObject gameObject in objectsToDeactive)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }


}
