using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool targetPlayer;
    public GameObject target;
    public float speed;
    public GameObject hitEffect;
    public float timeToDestroy;
    public void Start()
    {
        if (targetPlayer)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        //bulletRB = GetComponent<Rigidbody2D>();
        //Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
        //bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);
        //Destroy(this.gameObject, timeToDestroy);
    }

    //TRIGGER : COLLISION WITH PLAYER
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject;
        if (player && player.tag == "Player" && collision.isTrigger)
        {
            FindObjectOfType<AudioManager>().PlaySFX("Impact");
            CinemachineShake.Instance.ShakeCamera(3.5f, .1f);
            player.GetComponent<TopDownPlayerMovement>().TakeHit(1);
            player.GetComponent<TopDownPlayerMovement>().Knockback(transform);
            Destroy(gameObject);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, .5f);
    }


}
