using UnityEngine;


public class Bullet : MonoBehaviour
{
    public static Bullet Instance { get; private set; }
    private Animator anim;
    [Header("Bullet Attributes")]
    public float speed = 4;

    [Header("Visual Effects")]
    public GameObject hitEffect;
    public float force = 100;
    public float timeToDestroy;


    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("Bullet");
        Destroy(this.gameObject, timeToDestroy);
    }
    //TRIGGER : COLLISION WITH ENEMY

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 bulletDir = this.gameObject.transform.forward;
        var enemy = collision.gameObject;
        //var breakable = collision.gameObject;
        if (enemy && enemy.tag == "Breakable" && collision.isTrigger)
        {
            CinemachineShake.Instance.ShakeCamera(3.5f, .1f);
            FindObjectOfType<AudioManager>().PlaySFX("Impact");
            enemy.GetComponent<BreakableThing>().TakeHit(1);
        }
        if (enemy && enemy.tag == "Enemy" && collision.isTrigger)
        {
            CinemachineShake.Instance.ShakeCamera(3.5f, .1f);
            FindObjectOfType<AudioManager>().PlaySFX("Impact");


            //enemy.GetComponent<Entity>().Flash();
            enemy.GetComponent<Entity>().Knockback(transform);
            enemy.GetComponent<Entity>().TakeHit(GameObject.FindGameObjectWithTag("Player").GetComponent<TopDownPlayerMovement>().currentWeapon.damage);
        }

        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, .5f);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, .5f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, .5f);
    }


}
