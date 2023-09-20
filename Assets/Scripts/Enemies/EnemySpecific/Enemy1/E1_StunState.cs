using UnityEngine;

public class E1_StunState : StunState
{
    private Enemy1 _enemy;

    public E1_StunState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_StunState stateData, Enemy1 enemy) 
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

        if (isStunTimeOver)
        {
            if (performCloseRangeAction)
                stateMachine.ChangeState(_enemy.MeleeAttackState);
            else if (isPlayerInMinAgroRange)
                stateMachine.ChangeState(_enemy.ChargeState);
            else
            {
                _enemy.LookForPlayerState.SetTurnImmediately(true);
                stateMachine.ChangeState(_enemy.LookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}