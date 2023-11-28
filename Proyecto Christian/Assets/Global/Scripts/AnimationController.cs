using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private string actualState;
    public Animator animator;
    public Type controllerType;

    public bool isIdle;
    public bool isWalking;
    public bool isJumping;
    public bool isShooting;
    public bool isDashing;
    public bool isDying;

    public enum Type
    {
        TopDown,
        Platformer,
        Adventure
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeAnimationState(string newState)
    {

        if (actualState == newState) return;


        animator.Play(newState);

        actualState = newState;
    }

}
