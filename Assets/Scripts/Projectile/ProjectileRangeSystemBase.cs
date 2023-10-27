using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class ProjectileRangeSystemBase : SystemBase
{
    EntityCommandBuffer CommandBuffer;

    protected override void OnUpdate()
    {
        //Create entity command buffer
        CommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        //Check if projectile has moved outside of its range. WithoutBurst() required because of the entity command buffer
        Entities.WithoutBurst().ForEach((Entity Entity, in LocalTransform Transform, in ProjectileData ProjData, in ProjectileStartPosData StartPosData) =>
        {
            float Dist = math.distance(StartPosData.StartPos, Transform.Position);
            if(Dist > ProjData.Range) //Outside of range
            {
                CommandBuffer.DestroyEntity(Entity);
            }
        }).Run();

        //Destroy all projectiles outside of their range
        CommandBuffer.Playback(EntityManager);
    }
}
