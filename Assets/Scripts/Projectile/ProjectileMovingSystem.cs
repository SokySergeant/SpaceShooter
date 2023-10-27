using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct ProjectileMovingSystem : ISystem
{
    [BurstCompile]
    public void Update(ref SystemState State)
    {
        new ProjectileMovingJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct ProjectileMovingJob : IJobEntity
{
    public float DeltaTime;

    [BurstCompile]
    private void Execute(ProjectileAspect Projectile)
    {
        Projectile.Move(DeltaTime);
    }
}