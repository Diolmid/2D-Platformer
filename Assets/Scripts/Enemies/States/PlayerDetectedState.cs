using UnityEngine;

public class PlayerDetectedState : State
{
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;
    protected bool isDetectingLedge;

    protected D_PlayerDetected stateData;

    public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_PlayerDetected stateData) 
        : base(entity, stateMachine, animationBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMaxAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isDetectingLedge = entity.CheckLedge();
    }

    public override void Enter()
    {
        base.Enter();

        performLongRangeAction = false;
        entity.SetVelocity(0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        performLongRangeAction = Time.time >= startTime + stateData.longRangeActionTime;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}