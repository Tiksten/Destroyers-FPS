using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] enemySpawnPoints;
    public float timeSleepAfterTrigger;
    public float max_timeToSpawnEnemy;
    public float min_timeToSpawnEnemy;
    public bool onlyPlayerCanActivate;
    public bool canBeTriggeredOnlyOnce;

    private void OnTriggerEnter(Collider collider)
    {
        if(onlyPlayerCanActivate && collider.gameObject.tag == "Player")
        {
            Activate();
        }
        else if(!onlyPlayerCanActivate)
        {
            Activate();
        }
    }

    public void Activate()
    {
        foreach(Transform _spawnPoint in enemySpawnPoints)
        {
            StartCoroutine(SpawnEnemy(_spawnPoint));
            if (canBeTriggeredOnlyOnce)
            {
                Destroy(gameObject.GetComponent<Collider>());
                Destroy(gameObject, max_timeToSpawnEnemy + 1);
            }
        }
    }

    public IEnumerator SpawnEnemy(Transform pos)
    {
        yield return new WaitForSeconds(Random.Range(min_timeToSpawnEnemy, max_timeToSpawnEnemy));
        Instantiate(enemyPrefab, pos.position, pos.rotation);
    }
}
