using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableThing : MonoBehaviour
{
    public float hitPoints;
    [SerializeField] private float MaxHitPoints = 1;
    [SerializeField] private GameObject destroyEffect;
    public GameObject[] activate;
    public GameObject deactivate;
    void Awake()
    {
        hitPoints = MaxHitPoints;
    }
    public void TakeHit(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            CinemachineShake.Instance.ShakeCamera(2.5f, .1f);
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            if (activate != null)
            {
                foreach (GameObject gameObject in activate)
                {
                    gameObject.SetActive(true);
                }
            }
            if (deactivate != null)
            {
                deactivate.SetActive(false);
            }
        }
    }


}
