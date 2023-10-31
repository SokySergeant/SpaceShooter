using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial class EnemySetTargetSystemBase : SystemBase
{
    Transform PlayerTrans;

    protected override void OnUpdate()
    {
        if(PlayerTrans == null) PlayerTrans = GameObject.FindGameObjectWithTag("Player").transform;

        float3 Pos = PlayerTrans.position;
        Entities.ForEach((ref EnemyData Data) =>
        {
            Data.TargetPos = Pos;
        }).ScheduleParallel();
    }
}
