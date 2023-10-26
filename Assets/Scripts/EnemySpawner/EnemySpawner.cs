using System.Collections;
using Unity.Entities;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float TimeBetweenSpawns = 1f;
    public float SpawnIncreaseRate = 0.01f;

    public float SpawnRadius = 10f;

    public GameObject Enemy;

    public int SpawnAmount = 1;

    void Start()
    {
        //StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        yield return null;
        /*
        while(true)
        {
            //Increase how fast enemies spawn
            if(CurrentTimeBetweenSpawns - SpawnIncreaseRate > 0f)
            {
                CurrentTimeBetweenSpawns -= SpawnIncreaseRate;
            }
            float CalculatedTime = CurrentTimeBetweenSpawns + (Random.Range(-1f, 1f) * TimeDeviationBetweenSpawns) * (CurrentTimeBetweenSpawns / TimeBetweenSpawns);

            yield return new WaitForSeconds(CalculatedTime);
        }
        */
    }
}

public class EnemySpawnerBaker : Baker<EnemySpawner>
{
    public override void Bake(EnemySpawner authoring)
    {
        AddComponent(GetEntity(TransformUsageFlags.None), new EnemySpawnerData
        {
            TimeBetweenSpawns = authoring.TimeBetweenSpawns,
            SpawnIncreaseRate = authoring.SpawnIncreaseRate,
            TimeUntilNextSpawn = authoring.TimeBetweenSpawns,
            SpawnAmount = authoring.SpawnAmount,
            SpawnRadius = authoring.SpawnRadius,
            EnemyEntity = GetEntity(authoring.Enemy, TransformUsageFlags.Dynamic)
        });
    }
}

public struct EnemySpawnerData : IComponentData
{
    public float TimeBetweenSpawns;
    public float SpawnIncreaseRate;
    public float TimeUntilNextSpawn;

    public int SpawnAmount;

    public float SpawnRadius;

    public Entity EnemyEntity;
}