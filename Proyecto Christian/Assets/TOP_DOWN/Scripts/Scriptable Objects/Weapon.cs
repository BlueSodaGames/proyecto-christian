using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{

    public Sprite currentWeaponSprite;
    public RuntimeAnimatorController currentWeaponAnimator;
    public ShootingPattern shootingPattern;
    public GameObject bulletPrefab;
    public float fireRate = 1;
    public int damage = 1;
    public float bulletForce = 20f;
    public float amountSpreed;
    public float bulletDuration;
    [Header("Visual Effects")]
    public GameObject Seffect;
    public GameObject Seffect2;


    public AudioClip[] shootClips;

    //--------SHOOT-----------

    public enum ShootingPattern
    {
        SingleShot,
        TripleShot,
        ConsecutiveShots,
        // Add more patterns as needed
    }
    public void Shoot()
    {
        if (WeaponHandler.Instance != null)
        {
            WeaponHandler.Instance.FireSingleShot();
        }
    }

    public void ShootMobile()
    {
        if (WeaponHandler.Instance != null)
        {
            WeaponHandler.Instance.FireSingleShotMobile();
        }
    }

}