using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using PixelCrushers;

public enum PlayerState
{
    walk,
    shoot,
    stagger,
    idle,
}


public class TopDownPlayerMovement : MonoBehaviour
{

    [Space]
    [Header("Player Attributes")]
    [SerializeField] private FloatValue currentHealth;
    [SerializeField] private SignalSender playerHealthSignal;
    public float moveSpeed = 5f;
    private Vector2 movement;
    [SerializeField] private float timeBtwShots = 1;
    [HideInInspector] public BoxCollider2D coll;
    private Animator anim;
    private Rigidbody2D rb;

    [Space]
    [Header("Knockback Attributtes")]
    [SerializeField] private Transform center;
    [SerializeField] private float knockbackVel = 90f;
    [SerializeField] private float knockbackTime = 0.5f;
    [SerializeField] public bool knockbacked;
    [SerializeField] private Vector3 knockbackDirection;

    [Space]
    [Header("Shooting Atributtes")]
    [SerializeField] private Camera mousePositionCam;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Animator firePointAnim;
    public Weapon currentWeapon;
    public float shootMovementSpeed = 4f;
    public float shootBonus = 0f;
    [SerializeField] private Rigidbody2D fireRb;
    private Vector3 lookDir;
    private float angle;
    private float fireAngle;
    private Transform aimTransform;
    private Vector2 mousePos;
    private bool recentShoot;
    private float timeSinceLastShot = 0f;
    public float timeToResetRecentShoot = 2f;

    [Space]
    [Header("Dash Atributtes")]
    public TopDownDash topDownDash;
    public float savedMoveSpeed;


    [Space]
    [Header("Visual Effects")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject blood;
    [SerializeField] private GameObject dashParticle;
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private GameObject redBackground;
    [SerializeField] private GameObject gameOverUI;

    [Space]
    [Header("Invulnerability Frames")]
    [SerializeField] private SpriteRenderer mySprite;
    [SerializeField] private Color flashColor;
    [SerializeField] private Color regularColor;
    [SerializeField] private float flashDuration;
    [SerializeField] private int numerOfFlashes;

    [HideInInspector]
    private string actualState;
    public bool dashing = false;
    public bool shooting = true;
    public bool walking = true;
    public bool canDash = true;
    private bool dying = false;
    public bool furor = false;
    public bool disableInput = false;

    [Space]
    [Header("Mobile")]
    [SerializeField] public Joystick joystickMovement, joystickShoot;
    public GameObject cinematicBar;

    [SerializeField] private AudioSource audioSource;

    private Vector3 lastAimPosition;


    private void Awake()
    {
        aimTransform = transform.GetChild(0).transform;
    }

    private void Start()
    {

        
        joystickMovement.gameObject.SetActive(false);
        joystickShoot.gameObject.SetActive(false);
        

        savedMoveSpeed = moveSpeed;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        aimTransform.GetChild(0).GetComponent<SpriteRenderer>().sprite = currentWeapon.currentWeaponSprite;
        audioSource = GetComponent<AudioSource>();
        knockbackDirection = new Vector2(0, 0);
        knockbacked = false;

    }
    void Update()
    {


        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = mousePositionCam.ScreenToWorldPoint(Input.mousePosition);
            
        

        if (!disableInput)
        {

            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            lookDir = (mousePos - rb.position).normalized;

            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90f;
            fireAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            aimTransform.eulerAngles = new Vector3(0, 0, angle);
            


            fireRb.rotation = fireAngle;
            CheckAngle();

            //------MOVEMENT------
            Walking();

            //------ANIMATIONS UPDATE------

            AnimationUpdate();


                //------SHOOT------

            Shoot();

        }
        else
        {
            ChangeAnimationState("Idle");
            movement = Vector2.zero;
            shooting = false;
            walking = false;
            dying = false;
            anim.SetFloat("anglex", 0);
            anim.SetFloat("angley", -1);
        }


    }

    #region ANIMATION/INPUT


    private void CheckAngle()
    {
        if (recentShoot)
        {
            //arriba
            if (angle >= 135 && angle <= 225)
            {
                anim.SetFloat("angley", 1);
                anim.SetFloat("anglex", 0);
            }
            //derecha
            else if (angle >= 45 && angle < 135)
            {
                anim.SetFloat("angley", 0);
                anim.SetFloat("anglex", 1);
            }
            //abajo
            else if (angle < 45 && angle >= -45)
            {
                anim.SetFloat("angley", -1);
                anim.SetFloat("anglex", 0);
            }
            //izquierda
            else
            {
                anim.SetFloat("angley", 0);
                anim.SetFloat("anglex", -1);
            }
        }
        else
        {
            
            if (!(movement == Vector2.zero))
            {
                anim.SetFloat("anglex", movement.x);
                anim.SetFloat("angley", movement.y);
            }

        }
        
    }


    void AnimationUpdate()
    {
        if (!shooting && !walking && !dying)
        {
            firePointAnim.SetBool("shootFP", false);
            ChangeAnimationState("Idle");
        }

        if (walking && !shooting && !dashing && !dying)
        {
            ChangeAnimationState("Walk");
            firePointAnim.SetBool("shootFP", false);
        }

        if (shooting && walking && !dashing)
        {
            firePointAnim.SetBool("shootFP", true);
            ChangeAnimationState("WalkShoot");
        }
        if (shooting && !walking && !dashing && !dying)
        {
            firePointAnim.SetBool("shootFP", true);
            ChangeAnimationState("Shoot");
        }

        if (dashing && !dying)
        {
            ChangeAnimationState("Dash");
        }

        if (dying)
        {
            ChangeAnimationState("Death");
            firePointAnim.SetBool("shootFP", false);
        }

        if (knockbacked && !dying)
        {
            rb.AddForce(knockbackDirection * Time.deltaTime * knockbackVel);

        }
    }

    void ChangeAnimationState(string newState)
    {

        if (actualState == newState) return;


        anim.Play(newState);

        actualState = newState;
    }


    #endregion

    #region MOVIMIENTO
    void Walking()
    {

        if (movement.x != 0 || movement.y != 0 && !disableInput)
        {
            //CreateDust();
            if (!audioSource.isPlaying)
            {
                audioSource.Play();

            }
            walking = true;
        }
        else
        {
            audioSource.Stop();
            walking = false;

        }
    }
    #endregion

    #region DISPAROS
    void Shoot()
    {
        // Restablecer recentShoot si ha pasado suficiente tiempo desde el �ltimo disparo.
        if (Time.time - timeSinceLastShot > timeToResetRecentShoot)
        {
            recentShoot = false;
        }
        timeBtwShots -= Time.deltaTime;
        if (Input.GetButton("Fire1"))
        {
            recentShoot = true;
            timeSinceLastShot = Time.time;
            moveSpeed = shootMovementSpeed;
            shooting = true;
            if (Time.time >= timeBtwShots)
            {
                currentWeapon.Shoot();
                timeBtwShots = Time.time + 1 / currentWeapon.fireRate;
            }
        }
        else if (!Input.GetButton("Fire1") && !dashing && !dying)
        {
            moveSpeed = savedMoveSpeed;
            shooting = false;
        }
        else
        {
            //moveSpeed = activeMoveSpeed;
            shooting = false;
        }
    }

    void MobileShoot()
    {
        float shootHorizontal = joystickShoot.Horizontal;
        float shootVertical = joystickShoot.Vertical;
        // Restablecer recentShoot si ha pasado suficiente tiempo desde el �ltimo disparo.
        if (Time.time - timeSinceLastShot > timeToResetRecentShoot)
        {
            recentShoot = false;
        }
        timeBtwShots -= Time.deltaTime;
        if (Mathf.Abs(shootHorizontal) >= 0.4f || Mathf.Abs(shootVertical) >= 0.4f)
        {
            angle = Mathf.Atan2(shootVertical, shootHorizontal) * Mathf.Rad2Deg + 90f;
            fireAngle = Mathf.Atan2(shootVertical, shootHorizontal) * Mathf.Rad2Deg - 90f;
            aimTransform.eulerAngles = new Vector3(0, 0, angle);
            // Actualizar la �ltima posici�n del aim
            lastAimPosition = aimTransform.eulerAngles;

            recentShoot = true;
            timeSinceLastShot = Time.time;
            moveSpeed = shootMovementSpeed;
            shooting = true;

            if (Time.time >= timeBtwShots)
            {
                currentWeapon.Shoot();
                timeBtwShots = Time.time + 1 / currentWeapon.fireRate;
            }
        }
        else
        {
            moveSpeed = savedMoveSpeed;
            shooting = false;

            // Restaurar la posici�n del aim a la �ltima posici�n cuando no se est� disparando
            aimTransform.eulerAngles = lastAimPosition;
        }
    }

    #endregion

    

    #region COMBAT
    //--------HIT-----------
    public void TakeHit(float damage)
    {
        
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            StartCoroutine(FlashCo());
        }
        
        if (currentHealth.RuntimeValue <= 0)
        {
            StartCoroutine(DeathCo());
        }
    }

    //--------Death-----------

    IEnumerator DeathCo()
    {
        while (Time.timeScale != 1.0f)
            yield return null;
        knockbacked = false;
        knockbackDirection = new Vector2(0, 0);
        rb.freezeRotation = true;
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        disableInput = true;
        dying = true;
        timeBtwShots = 99f;
        canDash = false;
        coll.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        AudioManager.Instance.PlaySFX("DeathSound");
        AudioManager.Instance.StopMusic("Theme");


        yield return new WaitForSecondsRealtime(1f);
        redBackground.SetActive(true);

        mySprite.color = Color.black;
        mySprite.sortingLayerName = "Effects";

        yield return new WaitForSecondsRealtime(1f);
        this.mySprite.enabled = false;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(1f);
        currentHealth.RuntimeValue = currentHealth.initialValue;
        redBackground.SetActive(false);
        
        UIManager.Instance.gameOverUITopDown.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        AudioManager.Instance.PlayMusic("DeathTheme");
    }

    public void StopDeathMusic()
    {
        AudioManager.Instance.StopMusic("DeathTheme");
    }
    #endregion

    #region KNOCKBACK

    public void Knockback(Transform t)
    {
        Vector2 dir = (center.position - t.position).normalized;

        knockbackDirection = dir;
        knockbacked = true;


        StartCoroutine(Unkockback());
    }

    private IEnumerator Unkockback()
    {
        rb.drag = 0f;
        moveSpeed = 0;
        yield return new WaitForSeconds(knockbackTime - 0.2f);
        moveSpeed = savedMoveSpeed;
        knockbackDirection = new Vector2(0, 0);
        knockbacked = false;
    }
    #endregion


    //--------FLASH-----------

    private IEnumerator FlashCo()
    {
        int temp = 0;
        ignoreCollisions();
        while (temp < numerOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }

        applyCollisions();
    }


    public void ignoreCollisions()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Atravesable"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy Bullet"), true);
    }

    public void applyCollisions()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Atravesable"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy Bullet"), false);
    }

}
