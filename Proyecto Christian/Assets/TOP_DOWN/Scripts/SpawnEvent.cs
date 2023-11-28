using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEvent : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject portalPrefab;
    public Transform player;

    public float spawnRadius = 10f;
    public int maxEnemies = 10; // Número máximo de enemigos.
    public float spawnInterval = 2f; // Intervalo de generación de enemigos.
    public float finishEventTime = 30f; // Tiempo máximo de generación.
    private float elapsedTime = 0f;
    private int currentEnemyCount = 0;
    private float elapsedTimeSinceStart;

    public int minSpawnPortalRange = 50;
    public int maxSpawnPortalRange = 100;
    public TimelineManager timelineManager;

    private void OnEnable()
    {
        // Calcula un valor de offset dentro de los límites definidos
        float randomOffsetMagnitude = Random.Range(minSpawnPortalRange, maxSpawnPortalRange);
        Vector2 randomOffset = Random.insideUnitCircle.normalized * randomOffsetMagnitude;

        // Reposiciona el portal cerca del jugador.
        Vector3 portalPosition = player.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
        portalPrefab.SetActive(true); // Activa el portal reposicionado.
        portalPrefab.transform.position = portalPosition;
    }

    void Update()
    {
        elapsedTimeSinceStart += Time.deltaTime;

        if (currentEnemyCount < maxEnemies && elapsedTimeSinceStart < finishEventTime)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= spawnInterval)
            {
                SpawnEnemy();
                elapsedTime = 0f;
            }
        }
    }

    void SpawnEnemy()
    {
        // Random.insideUnitCircle es un punto aleatorio en un circulo,
        // cuyo centro será la posición del jugador.
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = player.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;
    }
}
