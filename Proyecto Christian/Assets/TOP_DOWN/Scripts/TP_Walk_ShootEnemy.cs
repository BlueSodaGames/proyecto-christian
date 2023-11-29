using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TP_Walk_ShootEnemy : Entity
{

    [Header("Enemy Attributes")]
    public float fireRate = 1f;
    private float nextFireTime;
    public float lineOfSite;
    public float shootingRange;
    public EnemyShootingController shootingController;
    public EnemyShootingController.ShootingPattern shootingPattern;
    private bool teleporting = false;
    public float tpCooldown = 4f;
    public Transform firePoint;
    public GameObject projectile;

    public float limiteIzquierdo, limiteDerecho, limiteAbajo, limiteArriba;

    private bool isAwake = false;
    public float wakeDistance;
    public float obstacleDetectionDistance = 2f;
    public LayerMask obstacleLayer;

    private void Update()
    {
        float fireAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        firePoint.eulerAngles = new Vector3(0, 0, fireAngle);
        if (target !=null)
        {
            if (dying)
            {
                //rb.freezeRotation = true;
                dying = true;
            }
            else if (!dying && !spawning)
            {
                //rb.freezeRotation = false;
                float distanceFromPlayer = Vector2.Distance(target.position, this.transform.position);
                if (!isAwake && distanceFromPlayer <= wakeDistance)
                {
                    isAwake = true;
                }
                if (isAwake)
                {
                    if (distanceFromPlayer < lineOfSite && canMove && distanceFromPlayer > shootingRange)
                    {
                        // Calculate direction to the player.
                        Vector3 directionToPlayer = target.position - transform.position;

                        // Check for obstacles in the current direction.
                        RaycastHit2D obstacleHitLeft = Physics2D.Raycast(new Vector2(transform.position.x - 1, transform.position.y), directionToPlayer.normalized, obstacleDetectionDistance, obstacleLayer);
                        RaycastHit2D obstacleHitCenter = Physics2D.Raycast(transform.position, directionToPlayer.normalized, obstacleDetectionDistance, obstacleLayer);
                        RaycastHit2D obstacleHitRight = Physics2D.Raycast(new Vector2(transform.position.x + 1, transform.position.y), directionToPlayer.normalized, obstacleDetectionDistance, obstacleLayer);
                        // Visualize the raycast
                        Debug.DrawRay(new Vector2(transform.position.x - 0.5f, transform.position.y), directionToPlayer.normalized * obstacleDetectionDistance, Color.red);
                        Debug.DrawRay(new Vector2(transform.position.x + 0.5f, transform.position.y), directionToPlayer.normalized * obstacleDetectionDistance, Color.blue);
                        Debug.DrawRay(transform.position, directionToPlayer.normalized * obstacleDetectionDistance, Color.green);
                        if (obstacleHitLeft.collider != null || obstacleHitRight.collider != null || obstacleHitCenter.collider != null)
                        {
                            // There's an obstacle, try to find an alternative direction.
                            Vector2 avoidanceDirection = Vector2.Perpendicular(directionToPlayer.normalized);
                            Vector3 alternativeDirection = new Vector3(avoidanceDirection.x, avoidanceDirection.y, 0);

                            // Move towards the alternative direction.
                            RaycastHit2D obstacleAvoidDirection = Physics2D.Raycast(transform.position, alternativeDirection.normalized, obstacleDetectionDistance, obstacleLayer);
                            //if (obstacleAvoidDirection.collider == null)
                            //{
                            transform.position += alternativeDirection * moveSpeed * Time.deltaTime;
                            Debug.DrawRay(transform.position, alternativeDirection.normalized * obstacleDetectionDistance, Color.yellow);
                            //}
                            //else
                            //{
                            //    transform.position += -(alternativeDirection) * moveSpeed * Time.deltaTime;
                            //    Debug.DrawRay(transform.position, -alternativeDirection.normalized * obstacleDetectionDistance, Color.yellow);
                            //}

                        }
                        else
                        {
                            // No obstacles, move towards the player.
                            transform.position += directionToPlayer.normalized * moveSpeed * Time.deltaTime;
                        }

                        moveSpeed = permaMoveSpeed;
                        idle = false;
                        attacking = false;
                        walking = true;
                    }
                    if (distanceFromPlayer < shootingRange && canMove && !teleporting)
                    {
                        Teleport();
                        teleporting = true;
                        StartCoroutine(TeleportCooldown());
                        moveSpeed = 0;
                        attacking = false;
                        walking = true;
                    }
                    else if (distanceFromPlayer <= shootingRange && nextFireTime < Time.time)
                    {
                        attacking = true;
                        walking = false;
                        StartCoroutine(ShootCo());
                        nextFireTime = Time.time + fireRate;
                    }
                    else if (distanceFromPlayer > lineOfSite)
                    {
                        moveSpeed = 0;
                        idle = true;
                        attacking = false;
                        walking = false;
                    }
                }
                
            }
        }
    }

    private IEnumerator ShootCo()
    {
        canMove = false;
        moveSpeed = 0;
        shootingRange *= 2;
        yield return new WaitForSeconds(1f);
        shootingController.Shoot(shootingPattern);
        yield return new WaitForSeconds(0.3f);
        attacking = false;
        walking = true;
        shootingRange /= 2;
        canMove = true;
        moveSpeed = permaMoveSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }

    private void Teleport()
    {
        Vector3 playerPos = target.position;
        float randomAngle = Random.Range(0, 360);
        float randomRadius = Random.Range(2f, 4f);
        Vector3 teleportPos = playerPos + Quaternion.Euler(0, 0, randomAngle) * Vector3.right * randomRadius;
        float clampedX = Mathf.Clamp(teleportPos.x, limiteIzquierdo, limiteDerecho);
        float clampedY = Mathf.Clamp(teleportPos.y, limiteAbajo, limiteArriba);
        teleportPos = new Vector3(clampedX, clampedY, 0f);

        transform.position = teleportPos;
    }

    private IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(tpCooldown);
        teleporting = false;
    }

}
