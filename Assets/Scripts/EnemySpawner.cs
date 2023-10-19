using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float TimeBetweenSpawns = 1f;
    [SerializeField] private float TimeDeviationBetweenSpawns = 0.5f;
    [SerializeField] private float SpawnIncreaseRate = 0.01f;
    private float CurrentTimeBetweenSpawns = 0f;

    [SerializeField] private float SpawnRadius = 5f;

    [SerializeField] private GameObject Enemy;
    [SerializeField] private Transform PlayerTrans;

    [SerializeField] private bool SpawnConstant = false;
    [SerializeField] private int SpawnAmount;

    void Start()
    {
        CurrentTimeBetweenSpawns = TimeBetweenSpawns;

        if(SpawnConstant)
        {
            for(int i = 0; i < SpawnAmount; i++)
            {
                if(PlayerTrans)
                {
                    Vector3 RandomDir = Random.insideUnitCircle.normalized;
                    GameObject TempEnemy = Instantiate(Enemy, transform.position + RandomDir * SpawnRadius, Quaternion.identity);
                    TempEnemy.GetComponent<Enemy>().PlayerTrans = PlayerTrans;
                }
            }
        } else
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while(true)
        {
            //Spawn enemies while the player is still alive
            if(PlayerTrans)
            {
                for(int i = 0; i < SpawnAmount; i++)
                {
                    Vector3 RandomDir = Random.insideUnitCircle.normalized;
                    GameObject TempEnemy = Instantiate(Enemy, transform.position + RandomDir * SpawnRadius, Quaternion.identity);
                    TempEnemy.GetComponent<Enemy>().PlayerTrans = PlayerTrans;
                }
            }

            //Increase how fast enemies spawn
            if(CurrentTimeBetweenSpawns - SpawnIncreaseRate > 0f)
            {
                CurrentTimeBetweenSpawns -= SpawnIncreaseRate;
            }
            float CalculatedTime = CurrentTimeBetweenSpawns + (Random.Range(-1f, 1f) * TimeDeviationBetweenSpawns) * (CurrentTimeBetweenSpawns / TimeBetweenSpawns);

            yield return new WaitForSeconds(CalculatedTime);
        }
    }
}
