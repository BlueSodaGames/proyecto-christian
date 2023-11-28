using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurorZone : MonoBehaviour
{
    float originalSpeed = 0f, originalShootBonus = 0f, originalShootSpeed = 0f, originalPermaMoveSpeed = 0f;
    [SerializeField] private float speedBonus = 5.5f, shootSpeedBonus = 3.5f, shootFireBonus = 0.75f, permaMoveSpeedBonus = 5.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<TopDownPlayerMovement>();
        if (player)
        {
            player.furor = true;

            //Velocidad de movimiento
            originalSpeed = player.moveSpeed;
            player.moveSpeed = speedBonus;

            //Velocidad de movimiento(HOLD)
            originalPermaMoveSpeed = player.savedMoveSpeed;
            player.savedMoveSpeed = permaMoveSpeedBonus;

            //Velocidad de movimiento al disparar
            originalShootSpeed = player.shootMovementSpeed;
            player.shootMovementSpeed = shootSpeedBonus;

            //Cadencia de fuego
            originalShootBonus = player.shootBonus;
            player.shootBonus = shootFireBonus;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<TopDownPlayerMovement>();
        if (player)
        {
            player.furor = false;
            player.moveSpeed = originalSpeed;
            player.savedMoveSpeed = originalPermaMoveSpeed;
            player.shootMovementSpeed = originalShootSpeed;
            player.shootBonus = originalShootBonus;
            originalSpeed = 0;
            originalPermaMoveSpeed = 0;
            originalShootSpeed = 0;
            originalShootBonus = 0;
        }
    }
}
