using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPortal : MonoBehaviour
{

    public float force = 0.02f;
    public BoxCollider2D col;
    public SceneFader sceneManager;
    public string sceneName;

    private bool catched;

    [SerializeField] private GameObject player;
    private float currentSize = 1;
    public string spawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject;
        if (player && player.tag == "Player" && collision.isTrigger)
        {

            player.GetComponent<PlatformerPlayerMovement>().canMove = false;

            StartCoroutine(animationCoroutine(player.GetComponent<Animator>()));
            
        }
    }


    private void FixedUpdate()
    {
        if (catched && currentSize > 0)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, this.transform.position, force);
            currentSize -= 0.005f;
            player.transform.localScale = new Vector3(currentSize, currentSize, 1f);
        }
    }

    IEnumerator animationCoroutine(Animator playerAnimator)
    {
        catched = true;
        player.GetComponent<BoxCollider2D>().enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(2f);
        catched = false;
        playerAnimator.gameObject.SetActive(false);
        sceneManager.LoadIndexLevel(sceneName, spawnPoint);
        Destroy(this.gameObject);

    }
}
