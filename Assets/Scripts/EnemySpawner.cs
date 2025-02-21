using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public string[] enemyTypes;
    public Transform spawnPoint;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            string randomEnemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
            Vector3 randomSpawnPosition;
            if (randomEnemyType == "FlyingEyes")
            {
                randomSpawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y + 10f + Random.Range(-1f, 2.5f), spawnPoint.position.z);
            }
            else
            {
                randomSpawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y + Random.Range(-4f, 2f), spawnPoint.position.z);
            }
            
            GameObject enemy = EnemyPoolManager.Instance.SpawnEnemy(randomEnemyType, randomSpawnPosition);
            if (enemy == null)
            {
                Debug.LogWarning($"Không thể spawn enemy {randomEnemyType}. Có thể pool đã hết!");
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            PlayerController.Instance.RegisterEnemy(enemy.GetComponent<EnemyBase>());
            yield return new WaitForSeconds(Random.Range(0.2f, 1.5f));
        }
    }
}