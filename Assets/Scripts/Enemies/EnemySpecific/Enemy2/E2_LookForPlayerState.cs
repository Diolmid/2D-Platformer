using UnityEngine;

public class E2_LookForPlayerState : LookForPlayerState
{
    private Enemy2 _enemy;

    public E2_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_LookForPlayer stateData, Enemy2 enemy) 
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

        if (isPlayerInMinAgroRange)
            stateMachine.ChangeState(_enemy.PlayerDetectedState);
        else if (isAllTurnsDone)
            stateMachine.ChangeState(_enemy.MoveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}