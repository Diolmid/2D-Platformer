using UnityEngine;

public class E2_DodgeState : DodgeState
{
    private Enemy2 _enemy;

    public E2_DodgeState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_DodgeState stateData, Enemy2 enemy) 
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

        if (isDodgeOver)
        {
            if (isPlayerInMaxAgroRange && performCloseRangeAction)
                stateMachine.ChangeState(_enemy.MeleeAttackState);
            else if (isPlayerInMaxAgroRange && !performCloseRangeAction)
                stateMachine.ChangeState(_enemy.RangeAttackState);
            else if (!isPlayerInMaxAgroRange)
                stateMachine.ChangeState(_enemy.LookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}