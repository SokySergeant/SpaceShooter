using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class ProjectileSpawnerSystemBase : SystemBase
{
    EntityCommandBuffer CommandBuffer;

    Player Player;

    protected override void OnCreate()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Player.SpawnProjectile += SpawnProjectile;
    }

    protected override void OnUpdate() {}

    public void SpawnProjectile(Vector3 SpawnPos, Quaternion SpawnRot)
    {
        CommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        foreach(var ProjectileSpawnerData in SystemAPI.Query<RefRW<ProjectileSpawnerData>>())
        {
            Entity SpawnedEntity = CommandBuffer.Instantiate(ProjectileSpawnerData.ValueRO.ProjectileEntity);
            CommandBuffer.SetComponent(SpawnedEntity, new LocalTransform { Position = SpawnPos, Rotation = SpawnRot, Scale = 2f });
        }

        //Playback buffer
        CommandBuffer.Playback(EntityManager);
    }
}
