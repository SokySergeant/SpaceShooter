using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;

public partial class EnemySpawnerSystem : SystemBase
{
    EntityCommandBuffer CommandBuffer;

    protected override void OnUpdate()
    {
        CommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        foreach(var EnemySpawnerData in SystemAPI.Query<RefRW<EnemySpawnerData>>())
        {
            //Check if should spawn yet
            if(EnemySpawnerData.ValueRO.TimeUntilNextSpawn > 0f)
            {
                EnemySpawnerData.ValueRW.TimeUntilNextSpawn -= SystemAPI.Time.DeltaTime;
                return;
            }

            //Spawning
            for(int i = 0; i < EnemySpawnerData.ValueRO.SpawnAmount; i++)
            {
                //Get position to spawn at
                Vector3 RandomDir = UnityEngine.Random.insideUnitCircle.normalized;
                RandomDir *= EnemySpawnerData.ValueRO.SpawnRadius;

                //Spawn and move entity to starting position
                Entity SpawnedEntity = CommandBuffer.Instantiate(EnemySpawnerData.ValueRO.EnemyEntity);
                CommandBuffer.SetComponent(SpawnedEntity, new LocalTransform { Position = new float3(RandomDir.x, RandomDir.y, 0), Scale = 0.5f });
            }

            //Increment TimeBetweenSpawns so enemies spawn faster over time
            if(EnemySpawnerData.ValueRO.TimeBetweenSpawns - EnemySpawnerData.ValueRO.SpawnIncreaseRate > 0f)
            {
                EnemySpawnerData.ValueRW.TimeBetweenSpawns -= EnemySpawnerData.ValueRO.SpawnIncreaseRate;
            }

            //Reset time
            EnemySpawnerData.ValueRW.TimeUntilNextSpawn = EnemySpawnerData.ValueRO.TimeBetweenSpawns;
        }

        //Playback buffer
       CommandBuffer.Playback(EntityManager);
    }
}
