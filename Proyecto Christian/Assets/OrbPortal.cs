using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class OrbPortal : MonoBehaviour
{
    public float waitTime = 2f;
    public float force = 0.02f;
    public BoxCollider2D col;
    public SceneFader sceneManager;
    public int sceneIndexToLoad;

    private bool catched;

    [SerializeField] private GameObject player;
    public PlayableDirector timelineOrb;

    private void OnEnable()
    {
        col = GetComponent<BoxCollider2D>();
        sceneManager = FindObjectOfType<SceneFader>();
        timelineOrb = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject;
        if (player && player.tag == "Player" && collision.isTrigger)
        {
            timelineOrb.enabled = true;
            player.GetComponent<TopDownPlayerMovement>().canMove = false;
            //PlayerPrefs.SetInt("NumberOfCoins", player.numberOfCoins);
            //PlayerPrefs.SetInt("NumberOfMemories", player.numberOfMemories);


            StartCoroutine(animationCoroutine(player.GetComponent<Animator>()));

        }
    }

    private void FixedUpdate()
    {
        if (catched)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, this.transform.position, force);
        }
    }

    IEnumerator animationCoroutine(Animator playerAnimator)
    {
        catched = true;
        player.GetComponent<BoxCollider2D>().enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(0.4f);

        playerAnimator.SetBool("win", true);

        yield return new WaitForSeconds(0.4f);

        yield return new WaitForSeconds(2f);
        playerAnimator.SetBool("win", false);
        catched = false;
        playerAnimator.gameObject.SetActive(false);
        sceneManager.LoadIndexLevel(sceneIndexToLoad);
        Destroy(this.gameObject);

    }
}
