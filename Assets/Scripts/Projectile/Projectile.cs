using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;

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
    }
}

public struct ProjectileData : IComponentData
{
    public float Speed;
    public float Range;
}

public readonly partial struct ProjectileAspect : IAspect
{
    private readonly RefRW<LocalTransform> Transform;
    private readonly RefRO<ProjectileData> Data;

    public void Move(float DeltaTime)
    {
        Transform.ValueRW.Position += Transform.ValueRO.Up() * Data.ValueRO.Speed * DeltaTime;
    }
}