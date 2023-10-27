using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject Projectile;
}

public class ProjectileSpawnerBaker : Baker<ProjectileSpawner>
{
    public override void Bake(ProjectileSpawner authoring)
    {
        AddComponent(GetEntity(TransformUsageFlags.None), new ProjectileSpawnerData
        {
            ProjectileEntity = GetEntity(authoring.Projectile, TransformUsageFlags.Dynamic)
        });
    }
}

public struct ProjectileSpawnerData : IComponentData
{
    public Entity ProjectileEntity;
}