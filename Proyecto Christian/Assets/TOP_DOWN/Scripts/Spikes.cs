using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    
    [SerializeField] private bool isExplosion = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            FindObjectOfType<AudioManager>().PlaySFX("Impact");
            CinemachineShake.Instance.ShakeCamera(3.5f, .1f);
            player.GetComponent<TopDownPlayerMovement>().TakeHit(1);
            player.GetComponent<TopDownPlayerMovement>().Knockback(transform);
        }
    }

    private void Start()
    {
        if (isExplosion)
        {
            Destroy(this.gameObject, 0.8f);
        }
    }

}