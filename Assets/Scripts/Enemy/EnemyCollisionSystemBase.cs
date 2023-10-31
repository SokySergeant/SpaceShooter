using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class EnemyCollisionSystemBase : SystemBase
{
    Transform PlayerTrans;
    Health PlayerHealth;
    EntityCommandBuffer CommandBuffer;

    protected override void OnUpdate()
    {
        if(PlayerTrans == null) PlayerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        if(PlayerHealth == null) PlayerHealth = PlayerTrans.GetComponent<Health>();

        //Create entity command buffer
        CommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        //Check collision with player. WithoutBurst() required because of the entity command buffer
        Entities.WithoutBurst().ForEach((Entity Entity, in LocalTransform Transform, in EnemyData Data) =>
        {
            float Dist = math.distance(PlayerTrans.position, Transform.Position);
            if(Dist <= Data.HitboxSize) //Collision happened
            {
                CommandBuffer.DestroyEntity(Entity);
                PlayerHealth.UpdateHealth(-Data.Damage);
            }
        }).Run();

        //Destroy all enemies that collided with player
        CommandBuffer.Playback(EntityManager);
    }
}