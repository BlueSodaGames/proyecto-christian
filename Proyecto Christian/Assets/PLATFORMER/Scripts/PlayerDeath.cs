using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlatformerPlayerMovement>();
        if (player && collision.isTrigger)
        {
            StartCoroutine(player.PlayerDeath());
        }
    }


}

