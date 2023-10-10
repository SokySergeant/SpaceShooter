using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float TimeBetweenSpawns = 1f;
    [SerializeField] private float TimeDeviationBetweenSpawns = 0.5f;

    [SerializeField] private float SpawnRadius = 5f;
    [SerializeField] private float SpawnRadiusDeviation = 1f;

    [SerializeField] private GameObject Enemy;
    [SerializeField] private Transform PlayerTrans;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while(true)
        {
            //Spawn enemies while the player is still alive
            if(PlayerTrans)
            {
                Vector3 RandomDir = Random.insideUnitCircle.normalized;
                GameObject TempEnemy = Instantiate(Enemy, transform.position + RandomDir * (SpawnRadius + Random.Range(-1f, 1f) * SpawnRadiusDeviation), Quaternion.identity);
                TempEnemy.GetComponent<Enemy>().PlayerTrans = PlayerTrans;
            }

            yield return new WaitForSeconds(TimeBetweenSpawns + Random.Range(-1f, 1f) * TimeDeviationBetweenSpawns);
        }
    }
}
