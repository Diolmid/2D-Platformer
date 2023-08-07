using UnityEngine;

public class MoveState : State
{
    protected bool isDetectedWall;
    protected bool isDetectedLedge;

    protected D_MoveState stateData;

    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_MoveState stateData) 
        : base(entity, stateMachine, animationBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(stateData.movementSpeed);

        isDetectedWall = entity.CheckWall();
        isDetectedLedge = entity.CheckLedge();
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

        isDetectedWall = entity.CheckWall();
        isDetectedLedge = entity.CheckLedge();
    }
}