using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public partial class ProjectileCollisionSystemBase : SystemBase
{
    EntityCommandBuffer CommandBuffer;
    List<float3> ProjectilePositions = new List<float3>();

    protected override void OnUpdate()
    {
        //Create buffer
        CommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        //Get positions of all projectiles
        ProjectilePositions.Clear();
        foreach((LocalTransform, ProjectileData) ProjItem in SystemAPI.Query<LocalTransform, ProjectileData>())
        {
            ProjectilePositions.Add(ProjItem.Item1.Position);
        }

        //Check if any projectiles are close enough to any enemy to hit it
        Entities.WithoutBurst().ForEach((in Entity EnemyEntity, in LocalTransform EnemyTransform, in EnemyData EnemyData) =>
        {
            for(int i = 0; i < ProjectilePositions.Count; i++)
            {
                float Dist = math.distance(ProjectilePositions[i], EnemyTransform.Position);
                if(Dist <= EnemyData.HitboxSize) //Close enough to enemy to hit it
                {
                    CommandBuffer.DestroyEntity(EnemyEntity);
                }
            }
        }).Run();

        //Destroy hit enemies
        CommandBuffer.Playback(EntityManager);
    }
}
