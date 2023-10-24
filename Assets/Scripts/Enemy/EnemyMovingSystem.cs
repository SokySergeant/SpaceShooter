using Unity.Entities;
using Unity.Burst;

[BurstCompile]
public partial struct EnemyMovingSystem : ISystem
{
    Entity Player;

    [BurstCompile]
    public void OnUpdate(ref SystemState State)
    {
        new EnemyMovingJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct EnemyMovingJob : IJobEntity
{
    public float DeltaTime;

    [BurstCompile]
    private void Execute(EnemyAspect Enemy)
    {
        Enemy.Move(DeltaTime);
    }
}