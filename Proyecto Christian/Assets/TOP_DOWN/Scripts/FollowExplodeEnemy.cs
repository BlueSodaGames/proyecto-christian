using System.Collections;
using UnityEngine;

public class FollowExplodeEnemy :Entity
{
    [Header("Enemy Attributes")]
    public float lineOfSite;
    public float obstacleDetectionDistance = 2f;
    public LayerMask obstacleLayer;
    public float attackRange = 2f; // Ajusta el rango de ataque seg�n tus necesidades.
    private bool inAttackRange = false;
    public GameObject explosion;
    private bool isAwake = false;
    public float wakeDistance;
    public bool explodeIfFar = false;

    private void Update()
    {
        if (target != null)
        {
            if (dying)
            {
                // Handle dying state if needed.
            }
            else if (!dying && !spawning)
            {
                float distanceFromPlayer = Vector2.Distance(target.position, transform.position);
                // Comprueba si el enemigo est� en rango de ataque.
                if (distanceFromPlayer > 20 && explodeIfFar)
                {
                    StartCoroutine(ExplodeCoroutine());
                }
                if (!isAwake && distanceFromPlayer <= wakeDistance)
                {
                    isAwake = true;
                }
                if (isAwake)
                {
                    inAttackRange = (distanceFromPlayer <= attackRange);

                    if (inAttackRange)
                    {
                        // Llama a la corutina ExplodeCoroutine si est� en rango de ataque.
                        StartCoroutine(ExplodeCoroutine());
                    }
                    else if (distanceFromPlayer < lineOfSite && canMove)
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

                            transform.position += alternativeDirection * moveSpeed * Time.deltaTime;
                            Debug.DrawRay(transform.position, alternativeDirection.normalized * obstacleDetectionDistance, Color.yellow);


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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wakeDistance);

    }

    private IEnumerator ExplodeCoroutine()
    {
        canMove = false;
        attacking = true;
        idle = false;
        walking = false;
        //inicio animacion pam pam PUM
        yield return new WaitForSeconds(1f);
        //se destruye
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    // Add other methods or coroutines as needed.
}