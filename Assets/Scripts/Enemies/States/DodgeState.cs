using UnityEngine;

public class DodgeState : State
{
    protected bool performCloseRangeAction;
    protected bool isPlayerInMaxAgroRange;
    protected bool isGrounded;
    protected bool isDodgeOver;

    protected D_DodgeState stateData;

    public DodgeState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_DodgeState stateData) 
        : base(entity, stateMachine, animationBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isGrounded = entity.CheckGround();
    }

    public override void Enter()
    {
        base.Enter();

        isDodgeOver = false;

        entity.SetVelocity(stateData.dodgeSpeed, stateData.dodgeAngle, -entity.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= StartTime + stateData.dodgeTime && isGrounded)
            isDodgeOver = true;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}