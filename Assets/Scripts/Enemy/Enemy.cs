using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed = 2f;
    public int Damage = 1;
    public float3 TargetPos;

    [SerializeField] private LayerMask PlayerLayer;
    private BoxCollider2D Collider;
    private ContactFilter2D CFilter;
    List<Collider2D> Cols;

    void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        CFilter = new ContactFilter2D();
        CFilter.SetLayerMask(PlayerLayer);
        Cols = new List<Collider2D>();
    }

    void Update()
    {
        //Rotate and move towards player
        /*
        if(PlayerTrans)
        {
            Vector2 Dir = PlayerTrans.position - transform.position;
            transform.rotation = Quaternion.LookRotation(transform.forward, Dir);
            transform.position += transform.up * Speed * Time.deltaTime;
        }*/

        //Damage player
        Collider.OverlapCollider(CFilter, Cols);
        if(Cols.Count > 0)
        {
            Cols[0].GetComponent<Health>().UpdateHealth(-Damage);
            Destroy(gameObject);
        }
    }
}

public class EnemyBaker : Baker<Enemy>
{
    public override void Bake(Enemy authoring)
    {
        AddComponent(GetEntity(TransformUsageFlags.Dynamic), new EnemyData
        {
            Speed = authoring.Speed,
            Damage = authoring.Damage,
            TargetPos = authoring.TargetPos
        }) ;
    }
}

public struct EnemyData : IComponentData
{
    public float Speed;
    public int Damage;
    public float3 TargetPos;
}

public readonly partial struct EnemyAspect : IAspect
{
    private readonly Entity Entity;

    private readonly RefRW<LocalTransform> Transform;
    private readonly RefRO<EnemyData> Data;

    public void Move(float DeltaTime)
    {
        float3 Dir = math.normalize(Data.ValueRO.TargetPos - Transform.ValueRO.Position);
        Transform.ValueRW.Position += Dir * DeltaTime * Data.ValueRO.Speed;
    }
}
