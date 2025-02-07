using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTowerStatus : MonoBehaviour
{
    [SerializeField] private int maxHP = 20;
    [SerializeField] private int currentHP;
    void Start()
    {
        currentHP = maxHP;
    }

    
    void Update()
    {
        if (currentHP <= 0)
        {
            
        }
    }
}
