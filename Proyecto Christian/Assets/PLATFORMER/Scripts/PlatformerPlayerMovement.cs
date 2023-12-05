using PixelCrushers;
using System.Collections;
using UnityEngine;

public class PlatformerPlayerMovement : MonoBehaviour
{

    [Header("Player Attributtes")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimer;
    public float dashSpeed = 20;

    [Space]
    [Header("Animation/States")]
    private string actualState;
    private bool idle = true, jumping = false, walking = false, falling = false, dashing = false;
    public bool canMove = true;
    private bool hasDashed;

    [Space]
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator anim;
    private BetterJumping betterJumping;
    [HideInInspector] public Collision coll;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private GameObject redBackground;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject deathEffect;



    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collision>();
        betterJumping = GetComponent<BetterJumping>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            CheckMovement();
            float xRaw = Input.GetAxisRaw("Horizontal");
            float yRaw = Input.GetAxisRaw("Vertical");
            if (Input.GetButtonDown("JumpDash") && !hasDashed)
            {
                if (xRaw != 0 || yRaw != 0)
                {
                    Dash(xRaw, yRaw);
                }
                    
            }
            CheckJump();
            AnimationUpdate();
        }
    }

    private void Dash(float x, float y)
    {
        hasDashed = true;

        anim.SetTrigger("dash");

        rb.velocity = Vector2.zero;
        float adjustedX = x * dashSpeed;
        float adjustedY = y * dashSpeed; // Multiplicar por un factor menor en la direcci�n Y

        Vector2 dir = new Vector2(adjustedX, adjustedY).normalized;

        rb.velocity += dir * dashSpeed;
        StartCoroutine(DashWait());
    }
    IEnumerator DashWait()
    {
        StartCoroutine(GroundDash());

        rb.gravityScale = 0;
        betterJumping.enabled = false;
        dashing = true;

        yield return new WaitForSeconds(.3f);

        rb.gravityScale = 2.5f;
        betterJumping.enabled = true;
        dashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (coll.onGround)
            hasDashed = false;
    }

    #region SALTOS

    private void CheckJump()
    {
        if (rb.velocity.y < 0)
        {
            falling = true;
            jumping = false;
        }
        if (coll.onGround)
        {
            hasDashed = false;
            dashing = false;
            coyoteTimer = coyoteTime;
            betterJumping.enabled = true;
            if (falling)
            {
                falling = false;
            }
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }


        if (Input.GetButtonDown("Jump") && (coll.onGround || coyoteTimer > 0f))
        {
            Jump(Vector2.up, false);
            jumping = true;
            ChangeAnimationState("Jump");
        }

        
    }

    private void Jump(Vector2 dir, bool wall)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
    }

    #endregion


    #region MOVIMIENTO
    private void CheckMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(x, y);
        if (x == 0)
        {
            walking = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (x > 0)
        {
            anim.SetFloat("anglex", 1);
            Walk(movement);
        }
        //izquierda
        if (x < 0)
        {  
            anim.SetFloat("anglex", -1);
            Walk(movement);
        }

    }

    private void Walk(Vector2 movement)
    {
        walking = true;
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }
    #endregion


    void ChangeAnimationState(string newState)
    {

        if (actualState == newState) return;


        anim.Play(newState);

        actualState = newState;
    }

    void AnimationUpdate()
    {
        if (dashing)
        {
            ChangeAnimationState("Dash");
        }
        else if (jumping)
        {
            ChangeAnimationState("Jump");
        }
        else if (falling)
        {
            ChangeAnimationState("Fall");
        }
        else if (walking)
        {
            ChangeAnimationState("Walk");
        }
        else
        {
            ChangeAnimationState("Idle");
        }
    }

    public IEnumerator PlayerDeath()
    {
        canMove = false;
        boxCollider.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        AudioManager.Instance.PlaySFX("DeathSound");
        AudioManager.Instance.StopMusic("Theme");


        yield return new WaitForSecondsRealtime(1f);
        redBackground.SetActive(true);

        spriteRenderer.color = Color.black;
        spriteRenderer.sortingLayerName = "Effects";

        yield return new WaitForSecondsRealtime(0.5f);
        this.spriteRenderer.enabled = false;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(0.5f);
        redBackground.SetActive(false);

        UIManager.Instance.gameOverUIPlatformer.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        AudioManager.Instance.PlayMusic("DeathTheme");
    }

    public void StopDeathMusic()
    {
        AudioManager.Instance.StopMusic("DeathTheme");
    }



}
