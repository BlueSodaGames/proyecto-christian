using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [Header("Coin Splash")]
    public Transform objTrans;
    protected float delay;
    protected float pastTime = 0;
    protected float when = 0.5f;
    protected Vector3 off;

    public SignalSender powerupSignal;

    [Header("Magnet")]
    public Rigidbody2D rig;
    public TopDownPlayerMovement player;
    public bool magnetize = false;
    public bool magnetizeAbility = false;
    
     void Start()
    {
        //random x
        off = new Vector3(Random.Range(-2, 2), off.y, off.z);
        //random y
        off = new Vector3(off.x, Random.Range(-2, 2), off.z);
        rig = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = FindObjectOfType<TopDownPlayerMovement>();
        }
        StartCoroutine(Magnet());
        powerupSignal.Raise();
    }

    private void Update()
    {
        //when = cuando deberia dejar de moverse
        if (when >= delay)
        {
            pastTime = Time.deltaTime;

            //position of coin
            objTrans.position += off * Time.deltaTime;
            delay += pastTime;
        }

        if (magnetize && magnetizeAbility)
        {
            Vector3 playerPoint = Vector3.MoveTowards(transform.position, player.transform.position + new Vector3(0, -0.5f, 0), 30 * Time.deltaTime);
            rig.MovePosition(playerPoint);
        }


    }


    private IEnumerator Magnet()
    {
        yield return new WaitForSeconds(1f);
        magnetize = true;
    }
}
