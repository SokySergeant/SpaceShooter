using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class EnemySetTargetSystemBase : SystemBase
{
    Transform PlayerTrans;

    protected override void OnStartRunning()
    {
        PlayerTrans = GameObject.Find("Player").transform;
    }

    protected override void OnUpdate()
    {
        float3 Pos = PlayerTrans.position;
        Entities.ForEach((ref EnemyData Data) =>
        {
            Data.TargetPos = Pos;
        }).ScheduleParallel();
    }
}
