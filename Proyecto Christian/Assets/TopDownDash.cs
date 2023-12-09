using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class TopDownDash : MonoBehaviour
{
    [SerializeField] private TopDownPlayerMovement player;
    [SerializeField] private float dashSpeed;
    [SerializeField] public float dashLength = .5f, dashCooldown = 1f;
    public float dashCounter;
    public float dashCoolCounter;

    private void Start()
    {
        player = GetComponent<TopDownPlayerMovement>();   
    }
    private void Update()
    {
        Dashing();
    }

    void Dashing()
    {

        if (Input.GetButton("Dash") && !player.knockbacked)
        {
            Dash();
        }
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                player.dashing = false;
                player.moveSpeed = player.savedMoveSpeed;
                dashCoolCounter = dashCooldown;
                player.applyCollisions();
            }
        }

        if (dashCoolCounter > 0)
        {
            player.canDash = false;
            player.topDownDash.dashCoolCounter -= Time.deltaTime;
        }
        if (dashCoolCounter <= 0)
        {
            player.canDash = true;
        }
    }

    public void Dash()
    {
        if (dashCoolCounter <= 0 && dashCounter <= 0 && !player.shooting && player.walking)
        {
            CinemachineShake.Instance.ShakeCamera(3f, .05f);
            // GameObject dashEffect = Instantiate(dashParticle, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().PlaySFX("Dash");
            player.dashing = true;
            player.moveSpeed = dashSpeed;
            dashCounter = dashLength;
            player.ignoreCollisions();
        }
    }
}
