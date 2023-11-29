using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [Header("Attributes")]
    protected float hitPoints;
    public float MaxHitPoints = 1;
    public float permaMoveSpeed = 1f;
    public float moveSpeed = 1f;
    protected Animator anim;
    public Transform target;
    protected string actualState;
    [SerializeField] public Collider2D col;
    protected float angle;
    protected Vector3 direction;
    public bool destroyOnCollision;
    public Vector2 homePosition;

    [Header("Knockback")]
    [SerializeField] private float knockbackVel = 4f;
    [SerializeField] private float knockbackTime = 0.25f;
    [SerializeField] private bool knockbacked;
    [SerializeField] private Vector3 knockbackDirection;
    private float knockbackTimer;


    [Header("Animator")]
    protected bool idle = false;
    protected bool walking = false;
    protected bool attacking = false;
    protected bool dying = false;
    protected bool spawning = true;
    protected bool damaged = false;

    protected bool canMove = false;
    protected bool canAttack = false;


    [Header("Loot")]
    public LootTable thisLoot;

    private void OnEnable()
    {
        
        knockbackDirection = new Vector2(0, 0);
        knockbacked = false;
        walking = false;
        attacking = false;
        dying = false;
        moveSpeed = permaMoveSpeed;
        col.enabled = true;
        anim = GetComponent<Animator>();
        hitPoints = MaxHitPoints;

        target = FindObjectOfType<TopDownPlayerMovement>().transform;
    }

    private void Start()
    {
        homePosition = transform.position;
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        canMove = false;
        canAttack = false;
        col.enabled = false;
        yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
        

        spawning = true;
        ChangeAnimationState("Spawn");
        
        yield return new WaitForSeconds(1.2f);
        col.enabled = true;
        spawning = false;
        canMove = true;
        canAttack = true;
    }

    private void FixedUpdate()
    {
        //ApplyMovement();
        if (target != null)
        {
            direction = target.position - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
        

        //DIRECTION
        //RIGHT
        if (angle <= 90 && angle >= -90)
        {
            anim.SetFloat("anglex", 1);
            anim.SetFloat("angley", 0);
        }
        //LEFT
        else
        {
            anim.SetFloat("anglex", -1);
            anim.SetFloat("angley", 0);
        }



        //ANIMATION
        if (damaged)
        {
            ChangeAnimationState("Damaged");
        }
        else if (!walking && !dying && !attacking && idle && !spawning && !damaged)
        {
            ChangeAnimationState("Idle");
        }
        else if (walking && !dying && !attacking && !spawning && !damaged)
        {
            ChangeAnimationState("Walk");
        }
        else if (attacking && !dying && !walking && !spawning && !damaged)
        {
            ChangeAnimationState("Attack");
        }
        else if (dying && !spawning && !damaged)
        {
            ChangeAnimationState("Death");
        }

        if (knockbacked)
        {
            // Aplicar knockback manualmente
            transform.position += knockbackDirection * knockbackVel * Time.deltaTime;

            // Reducir el tiempo de knockback
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                knockbacked = false;
                knockbackDirection = Vector3.zero;
            }
        }
    }


    public void TakeHit(float damage)
    {

        hitPoints -= damage;
        damaged = true;
        knockbackTimer = knockbackTime;

        StartCoroutine(DamagedCo());
        //HitStop.Instance.Stop(0.05f);
        //Instantiate(hitEffect, transform.position, Quaternion.identity);
        if (hitPoints <= 0)
        {

            StartCoroutine(DeathCo());
        }
    }

    private void MakeLoot()
    {
        if (thisLoot != null)
        {
            for (int i = 0; i < thisLoot.numberOfThis; i++)
            {
                if (thisLoot != null)
                {
                    Powerup current = thisLoot.LootPowerup();
                    if (current != null)
                    {
                        Instantiate(current.gameObject, transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0), Quaternion.identity);
                    }
                }
            }
        }
    }

    IEnumerator DeathCo()
    {
        while (Time.timeScale != 1.0f) 
            yield return null;
        col.enabled = false;
        dying = true;
        knockbacked = false;
        knockbackDirection = new Vector2(0, 0);
        moveSpeed = 0;
        yield return new WaitForSeconds(0.8f);
        FindObjectOfType<AudioManager>().PlaySFX("EnemyPlayerDeath");
        MakeLoot();
        //this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    IEnumerator DamagedCo()
    {
        yield return new WaitForSeconds(0.3f);
        damaged = false;
    }


    protected void ChangeAnimationState(string newState)
    {

        if (actualState == newState) return;
        anim.Play(newState);
        actualState = newState;
    }




    public void Knockback(Transform t)
    {
        Vector2 dir = (this.transform.position - t.position).normalized;

        knockbackDirection = dir;
        knockbacked = true;

        //StartCoroutine(Unkockback());

    }

    //private IEnumerator Unkockback()
    //{
    //    yield return new WaitForSeconds(knockbackTime);
    //    moveSpeed = permaMoveSpeed;
    //    knockbacked = false;
    //    knockbackDirection = new Vector2(0, 0);
    //}



    private void OnTriggerEnter2D(Collider2D collision)
    {
        var bounciness = 0f;
        var player = collision.GetComponent<TopDownPlayerMovement>();

        if (player)
        {
            FindObjectOfType<AudioManager>().PlaySFX("Impact");
            CinemachineShake.Instance.ShakeCamera(3.5f, .1f);
            player.TakeHit(1);
            player.Knockback(transform);
            if (destroyOnCollision)
            {
                CinemachineShake.Instance.ShakeCamera(3.5f, .1f);
                FindObjectOfType<AudioManager>().PlaySFX("Impact");
                this.TakeHit(1);
            }

        }
        if (collision.CompareTag("World"))
        {
            bounciness = 1.5f;
        }
        //rb.velocity += collision.relativeVelocity * bounciness;
    }


        
}
