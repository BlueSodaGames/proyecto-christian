using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformerPlayerMovement : MonoBehaviour
{

    [Header("Player Attributtes")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimer;

    [Space]
    [Header("Animation/States")]
    private string actualState;
    private bool idle = true, jumping = false, walking = false, falling = false;
    [HideInInspector] public bool canMove = true;

    [Space]
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator anim;
    private BetterJumping betterJumping;
    [HideInInspector] public Collision coll;

    
    // Start is called before the first frame update
    void Start()
    {
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

            CheckJump();
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
        if (jumping)
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

    public void PlayerDeath()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }
}
