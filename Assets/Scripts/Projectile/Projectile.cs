using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;

public class Projectile : MonoBehaviour
{
    public float Speed = 5f;

    void Update()
    {
        //Move
        //transform.position += transform.up * Speed * Time.deltaTime;

        //Kill overlapping enemy
    }
}

public class ProjectileBaker : Baker<Projectile> 
{
    public override void Bake(Projectile authoring)
    {
        AddComponent(GetEntity(TransformUsageFlags.Dynamic), new ProjectileData 
        {
            Speed = authoring.Speed
        });
    }
}

public struct ProjectileData : IComponentData
{
    public float Speed;
}

public readonly partial struct ProjectileAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRW<LocalTransform> Transform;
    private readonly RefRO<ProjectileData> Data;

    public void Move(float DeltaTime)
    {
        Transform.ValueRW.Position += Transform.ValueRO.Up() * Data.ValueRO.Speed * DeltaTime;
    }
}