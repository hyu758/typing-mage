using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseZone : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Trigger Entered: " + other.tag);
            GameManager.Instance.GameOver();
        }
    }
}
