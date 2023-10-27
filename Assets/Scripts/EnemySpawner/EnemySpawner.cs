using Unity.Entities;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float TimeBetweenSpawns = 1f;
    public float SpawnIncreaseRate = 0.01f;

    public float SpawnRadius = 10f;

    public GameObject Enemy;

    public int SpawnAmount = 1;
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