using UnityEngine;

public class StunState : State
{
    protected bool isStunTimeOver;
    protected bool isGrounded;
    protected bool isMovementStoped;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;

    protected D_StunState stateData;

    public StunState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_StunState stateData) 
        : base(entity, stateMachine, animationBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = entity.CheckGround();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        isStunTimeOver = false;
        isMovementStoped = false;
        entity.SetVelocity(stateData.stunKnockbakSpeed, stateData.stunKnockbackAngle, entity.LastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();

        entity.ResetStunResistance();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        isStunTimeOver = Time.time >= startTime + stateData.stunTime;

        if(isGrounded && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStoped)
        {
            isMovementStoped = true;
            entity.SetVelocity(0);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}