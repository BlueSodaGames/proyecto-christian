using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPortal : MonoBehaviour
{

    public float waitTime = 2f;
    public float force = 0.02f;
    public Animator winDoorAnimator;
    public BoxCollider2D col;
    public SceneFader sceneManager;
    public int sceneIndexToLoad;

    private bool catched;

    [SerializeField] private TopDownPlayerMovement player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player)
        {

            player.canMove = true;
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
        player.coll.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(0.4f);
        
        playerAnimator.SetBool("win", true);

        yield return new WaitForSeconds(0.4f);
        winDoorAnimator.SetBool("win", true);

        yield return new WaitForSeconds(2f);
        playerAnimator.SetBool("win", false);
        winDoorAnimator.SetBool("win", false);
        catched = false;
        playerAnimator.gameObject.SetActive(false);
        sceneManager.LoadIndexLevel(sceneIndexToLoad);
        Destroy(this.gameObject);

    }
}
