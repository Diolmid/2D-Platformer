using UnityEngine;

public class E1_PlayerDetectedState : PlayerDetectedState
{
    private Enemy1 _enemy;

    public E1_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_PlayerDetected stateData, Enemy1 enemy) 
        : base(entity, stateMachine, animationBoolName, stateData)
    {
        _enemy = enemy;
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

        if (performLongRangeAction)
            stateMachine.ChangeState(_enemy.ChargeState);
        else if (performCloseRangeAction)
            stateMachine.ChangeState(_enemy.MeleeAttackState);
        else if (!isPlayerInMaxAgroRange)
            stateMachine.ChangeState(_enemy.LookForPlayerState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}