using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;

public class Projectile : MonoBehaviour
{
    public float Speed = 5f;
    public float Range = 10f;
}

public class ProjectileBaker : Baker<Projectile> 
{
    public override void Bake(Projectile authoring)
    {
        AddComponent(GetEntity(TransformUsageFlags.Dynamic), new ProjectileData 
        {
            Speed = authoring.Speed,
            Range = authoring.Range
        });

        AddComponent(GetEntity(TransformUsageFlags.Dynamic), new ProjectileStartPosData
        {
            StartPos = new float3(0, 0, 0)
        });
    }
}

public struct ProjectileData : IComponentData
{
    public float Speed;
    public float Range;
}

public struct ProjectileStartPosData : IComponentData
{
    public float3 StartPos;
}

public readonly partial struct ProjectileAspect : IAspect
{
    public readonly Entity ProjectileEntity;

    private readonly RefRW<LocalTransform> Transform;
    private readonly RefRO<ProjectileData> Data;

    public void Move(float DeltaTime)
    {
        Transform.ValueRW.Position += Transform.ValueRO.Up() * Data.ValueRO.Speed * DeltaTime;
    }
}