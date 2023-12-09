using PixelCrushers;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.AudioSettings;

public class PlatformerPlayerMovement : MonoBehaviour
{

    [Header("Player Attributtes")]
    [SerializeField] public float speed = 10;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimer;

    [Space]
    [Header("Animation/States")]
    private string actualState;
    private bool idle = true, jumping = false, walking = false, falling = false;
    public bool canMove = true;
    

    [Space]
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;
    public Animator anim;
    public BetterJumping betterJumping;
    [HideInInspector] public Collision coll;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] public PlatformerDash dash;

    [SerializeField] private GameObject redBackground;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject deathEffect;

    [Space]
    [Header("Mobile")]
    [SerializeField] public bool mobile = false;
    [SerializeField] public Joystick joystickMovement;
    [SerializeField] private float tiempoPulsado = 0f;

    public Button botonSalto;
    public float jumpForceMultiplier = 3f; // Ajusta según sea necesario
    public float minJumpForce = 5f; // Ajusta según sea necesario
    public float maxJumpForce = 15f;
    private float startJumpTime;
    public float maxJumpHeight = 15f;
    public bool buttonDash;


    // Start is called before the first frame update
    void Start()
    {
        if (mobile)
        {
            joystickMovement.gameObject.SetActive(true);
            botonSalto.gameObject.SetActive(true);

        }
        else
        {
            joystickMovement.gameObject.SetActive(false);
            botonSalto.gameObject.SetActive(false);
        }

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
            if (!mobile)
            {
                CheckMovement();
                CheckJump();
            }
            else
            {
                CheckMovementMobile();
                CheckJumpMobile();
            }
            
            AnimationUpdate();
        }
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
            dash.hasDashed = false;
            dash.dashing = false;
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

    private void CheckJumpMobile()
    {
        if (rb.velocity.y < 0)
        {
            falling = true;
            jumping = false;
        }
        if (coll.onGround)
        {
            dash.hasDashed = false;
            dash.dashing = false;
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

        if (jumping)
        {
            float jumpTime = Time.time - startJumpTime;
            float jumpHeight = Mathf.Clamp(jumpTime * jumpForceMultiplier, minJumpForce, maxJumpForce);

            // Limitar la altura máxima del salto
            jumpHeight = Mathf.Min(jumpHeight, maxJumpHeight);

            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }

    public void StartJump()
    {
        if (coll.onGround || coyoteTimer > 0f)
        {
            startJumpTime = Time.time;
            jumping = true;
            JumpMobile(Vector2.up, false);
            ChangeAnimationState("Jump");

            
        }

        // Si el jugador está en el aire y no está actualmente realizando un dash, realizar el dash
        if (!coll.onGround && !dash.hasDashed)
        {
            dash.buttonDash = true;
        }
    }

    // Llamado desde el evento PointerUp del botón
    public void EndJump()
    {
        jumping = false;
    }

    private void JumpMobile(Vector2 dir, bool wall)
    {
        float jumpTime = Time.time - startJumpTime;
        float jumpHeight = Mathf.Clamp(jumpTime * jumpForceMultiplier, minJumpForce, maxJumpForce);

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpHeight;
    }

    private void Jump(Vector2 dir, bool wall)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
    }


    #endregion



    #region MOVIMIENTO

    private void CheckMovementMobile()
    {
        float x = joystickMovement.Horizontal;
        float y = joystickMovement.Vertical;
        Vector2 movement = new Vector2(x, y);
        if (x == 0)
        {
            walking = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (x > 0)
        {
            anim.SetFloat("anglex", 1);
            Walk(new Vector2(1, movement.y));
        }
        //izquierda
        if (x < 0)
        {
            anim.SetFloat("anglex", -1);
            Walk(new Vector2(-1, movement.y));
        }
    }

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
        if (dash.dashing)
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
        canMove = true;
    }

    public void StopDeathMusic()
    {
        AudioManager.Instance.StopMusic("DeathTheme");
    }



}
