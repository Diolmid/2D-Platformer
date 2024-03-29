using UnityEngine;

public class IdleState : State
{
    protected float idleTime;

    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;
    protected bool isPlayerInMinAgroRange;

    protected D_Idle stateData;

    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_Idle stateData) 
        : base(entity, stateMachine, animationBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(0);
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (flipAfterIdle)
            entity.Flip();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= StartTime + idleTime)
            isIdleTimeOver = true;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetFlipAfterIdle(bool flip) 
        => flipAfterIdle = flip;

    private void SetRandomIdleTime() 
        => idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
}