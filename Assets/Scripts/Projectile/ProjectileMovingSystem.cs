using Unity.Entities;
using Unity.Burst;

[BurstCompile]
public partial struct ProjectileMovingSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState State)
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
    private void Execute(ProjectileAspect Proj)
    {
        Proj.Move(DeltaTime);
    }
}