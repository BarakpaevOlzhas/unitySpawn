using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private GameObject cloneEnemy;

    [SerializeField]
    private float timeRespawn;

    private bool enemyIsDied = false;

    private float timeRespawnStart;

    private float realTime;

    public void EnemyIsDied()
    {
        Debug.Log("fif");
        enemyIsDied = true;
        timeRespawnStart = Time.time;
    }

    void Update()   
    {
        if (enemyIsDied)
        {
            realTime = Time.time;
            Debug.Log(transform.position);
            if (timeRespawn <= realTime - timeRespawnStart)
            {
                Instantiate(cloneEnemy, spawnPoint.position, transform.rotation);
                enemyIsDied = false;
            }
            
        }
    }
}
