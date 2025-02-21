using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance;

    public GameObject[] enemyPrefabs;
    public int poolSize = 10;

    private Dictionary<string, Queue<GameObject>> enemyPools;

    void Awake()
    {
        Instance = this;
        enemyPools = new Dictionary<string, Queue<GameObject>>();
        
        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(enemyPrefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            enemyPools.Add(enemyPrefab.name, pool);
        }
    }

    public GameObject SpawnEnemy(string enemyType, Vector3 position)
    {
        if (enemyPools.ContainsKey(enemyType) && enemyPools[enemyType].Count > 0)
        {
            GameObject enemy = enemyPools[enemyType].Dequeue();
            enemy.transform.position = position;
            enemy.SetActive(true);
            Debug.Log($"Spawned enemy {enemyType}. Pool size còn lại: {enemyPools[enemyType].Count}");
            return enemy;
        }
        Debug.LogWarning($"Không thể spawn enemy {enemyType}, pool đã hết!");
        return null;
    }

    public void ReturnToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        string enemyType = enemy.name.Replace("(Clone)", "").Trim();
        if (enemyPools.ContainsKey(enemyType))
        {
            enemyPools[enemyType].Enqueue(enemy);
            Debug.Log($"Enemy {enemyType} đã quay lại pool. Pool size: {enemyPools[enemyType].Count}");
        }
        else
        {
            Debug.LogWarning($"Enemy {enemyType} không thuộc pool nào!");
        }
    }
}