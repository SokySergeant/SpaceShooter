using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class ProjectileSpawnerSystemBase : SystemBase
{
    Player Player;

    protected override void OnUpdate() 
    {
        if(Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            Player.SpawnProjectile += SpawnProjectile;
        }
    }

    public void SpawnProjectile(Vector3 SpawnPos, Quaternion SpawnRot)
    {
        foreach(var ProjectileSpawnerData in SystemAPI.Query<RefRW<ProjectileSpawnerData>>())
        {
            Entity SpawnedEntity = EntityManager.Instantiate(ProjectileSpawnerData.ValueRO.ProjectileEntity);
            EntityManager.SetComponentData(SpawnedEntity, new LocalTransform { Position = SpawnPos, Rotation = SpawnRot, Scale = 2f });
            EntityManager.SetComponentData(SpawnedEntity, new ProjectileStartPosData { StartPos = SpawnPos });
        }
    }
}
