using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerDash : MonoBehaviour
{
    public PlatformerPlayerMovement player;
    public float dashSpeed = 20;
    public bool dashing = false;
    public bool hasDashed;
    public bool buttonDash;
    // Start is called before the first frame update

    private void Update()
    {
        if (player.canMove)
        {
            if (!player.mobile)
            {
                float xRaw = Input.GetAxisRaw("Horizontal");
                float yRaw = Input.GetAxisRaw("Vertical");
                if (this.enabled && Input.GetButtonDown("JumpDash") && !hasDashed)
                {
                    if (xRaw != 0 || yRaw != 0)
                    {
                        Dash(xRaw, yRaw);
                    }

                }
            }
            else
            {
                float x = player.joystickMovement.Horizontal;
                float y = player.joystickMovement.Vertical;
                if (this.enabled && buttonDash && !hasDashed)
                {
                    if (x != 0 || y != 0)
                    {
                        Dash(x, y);
                    }

                }
            }
            
        }
    }

    public void ButtonDashTrue()
    {
        buttonDash = true;
    }

    public void Dash(float x, float y)
    {
        hasDashed = true;

        player.anim.SetTrigger("dash");

        player.rb.velocity = Vector2.zero;
        float adjustedX = x * dashSpeed;
        float adjustedY = y * dashSpeed; // Multiplicar por un factor menor en la dirección Y

        Vector2 dir = new Vector2(adjustedX, adjustedY).normalized;

        player.rb.velocity += dir * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        StartCoroutine(GroundDash());

        player.rb.gravityScale = 0;
        player.betterJumping.enabled = false;
        dashing = true;

        yield return new WaitForSeconds(.3f);

        player.rb.gravityScale = 2.5f;
        player.betterJumping.enabled = true;
        dashing = false;
        buttonDash = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (player.coll.onGround)
            hasDashed = false;
    }
}
