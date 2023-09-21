using UnityEngine;

public class E2_PlayerDetectedState : PlayerDetectedState
{
    private Enemy2 _enemy;

    public E2_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_PlayerDetected stateData, Enemy2 enemy) 
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

        if (performCloseRangeAction)
            stateMachine.ChangeState(_enemy.MeleeAttackState);
        else if (!isPlayerInMaxAgroRange)
            stateMachine.ChangeState(_enemy.LookForPlayerState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}