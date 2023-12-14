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
        timeline.stopped += OnTimelineFinished;
        DisablePlayerInput();
    }

    private void OnTimelineFinished(PlayableDirector aDirector)
    {
        if (aDirector == timeline)
        {
            EnablePlayerInput();
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

    void DisablePlayerInput()
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

    void EnablePlayerInput()
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
    }

    private void OnDisable()
    {
        EnablePlayerInput();
    }
}
