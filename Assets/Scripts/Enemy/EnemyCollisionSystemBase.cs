using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class EnemyCollisionSystemBase : SystemBase
{
    Transform PlayerTrans;

    protected override void OnStartRunning()
    {
        PlayerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void OnUpdate()
    {
        float3 Pos = PlayerTrans.position;
        Entities.ForEach((in LocalTransform Transform, in EnemyData Data) =>
        {
            float Dist = math.distance(Pos, Transform.Position);
            if(Dist <= Data.HitboxSize)
            {
                //EntityCommandBuffer.DestroyEntity();
            }
        }).ScheduleParallel();
    }
}
