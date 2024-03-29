using UnityEngine;

public class MoveState : State
{
    protected bool isDetectedWall;
    protected bool isDetectedLedge;
    protected bool isPlayerInMinAngroRange;

    protected D_Move stateData;

    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_Move stateData) 
        : base(entity, stateMachine, animationBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isDetectedWall = entity.CheckWall();
        isDetectedLedge = entity.CheckLedge();
        isPlayerInMinAngroRange = entity.CheckPlayerInMaxAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(stateData.movementSpeed);   
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}