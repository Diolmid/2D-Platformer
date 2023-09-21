using UnityEngine;

public class E2_MoveState : MoveState
{
    private Enemy2 _enemy;

    public E2_MoveState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_Move stateData, Enemy2 enemy) 
        : base(entity, stateMachine, animationBoolName, stateData)
    {
        _enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAngroRange)
            stateMachine.ChangeState(_enemy.PlayerDetectedState);
        else if (isDetectedWall || !isDetectedLedge)
        {
            stateMachine.ChangeState(_enemy.IdleState);
            _enemy.IdleState.SetFlipAfterIdle(true);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}