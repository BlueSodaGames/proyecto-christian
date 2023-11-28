using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public Weapon weapon;
    public WeaponHandler weaponHandler;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            weaponHandler = FindObjectOfType<WeaponHandler>();
            weaponHandler.currentWeapon = weapon;
            collision.GetComponent<TopDownPlayerMovement>().currentWeapon = weapon;
            collision.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sprite = weapon.currentWeaponSprite;
            collision.transform.GetChild(2).GetChild(0).GetComponent<Animator>().runtimeAnimatorController = weapon.currentWeaponAnimator;
            Destroy(gameObject);
        }
    }
}
