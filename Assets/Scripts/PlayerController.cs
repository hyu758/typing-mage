using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public List<EnemyBase> enemies = new List<EnemyBase>();

    void Start()
    {
        Instance = this;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) HandleInput(KeyCode.UpArrow);
        if (Input.GetKeyDown(KeyCode.DownArrow)) HandleInput(KeyCode.DownArrow);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) HandleInput(KeyCode.LeftArrow);
        if (Input.GetKeyDown(KeyCode.RightArrow)) HandleInput(KeyCode.RightArrow);
    }

    void HandleInput(KeyCode keyCode)
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i].CheckInput(keyCode))
            {
                enemies.RemoveAt(i);
            }
        }
    }
    public void RegisterEnemy(EnemyBase enemy)
    {
        enemies.Add(enemy);
    }
}
