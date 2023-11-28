using System.Collections;
using UnityEngine;

public class WalkingShootingEnemy : Entity
{
    [Header("Enemy Attributes")]
    public float lineOfSite;
    public float obstacleDetectionDistance = 2f;
    public LayerMask obstacleLayer;

    public EnemyShootingController shootingController;
    public EnemyShootingController.ShootingPattern shootingPattern;
    public float fireRate = 1f;
    private float nextFireTime;
    public float shootingRange;
    public Transform firePoint;
    public GameObject projectile;
    private bool isAwake = false;
    public float wakeDistance;
    private void Update()
    {
        if (target != null)
        {
            float fireAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
            firePoint.eulerAngles = new Vector3(0, 0, fireAngle);
            if (dying)
            {
                
            }
            else if (!dying && !spawning)
            {
                float distanceFromPlayer = Vector2.Distance(target.position, transform.position);
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

                        // Update animation states if needed.
                        idle = false;
                        walking = true;
                    }
                    else if (distanceFromPlayer <= shootingRange && nextFireTime < Time.time && canAttack)
                    {
                        attacking = true;
                        walking = false;
                        StartCoroutine(ShootCo());
                        nextFireTime = Time.time + fireRate;
                    }
                    else
                    {
                        // Handle other cases or states as needed.
                        idle = true;
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wakeDistance);
    }

    // Add other methods or coroutines as needed.
}
