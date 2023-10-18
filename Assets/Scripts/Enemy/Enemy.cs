using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed = 2f;
    public int Damage = 1;
    public float HitboxSize = 0.2f;
}

public class EnemyBaker : Baker<Enemy>
{
    public override void Bake(Enemy authoring)
    {
        AddComponent(GetEntity(TransformUsageFlags.Dynamic), new EnemyData
        {
            Speed = authoring.Speed,
            Damage = authoring.Damage,
            HitboxSize = authoring.HitboxSize
        });
    }
}

public struct EnemyData : IComponentData
{
    public float Speed;
    public int Damage;
    public float3 TargetPos;
    public float HitboxSize;
}

public readonly partial struct EnemyAspect : IAspect
{
    private readonly RefRW<LocalTransform> Transform;
    private readonly RefRO<EnemyData> Data;

    public void Move(float DeltaTime)
    {
        float3 Dir = math.normalize(Data.ValueRO.TargetPos - Transform.ValueRO.Position);
        Transform.ValueRW.Position += Dir * DeltaTime * Data.ValueRO.Speed;
    }
}