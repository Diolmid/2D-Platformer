using UnityEngine;

public class E2_RangeAttackState : RangeAttackState
{
    private Enemy2 _enemy;

    public E2_RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, Transform attackPosition, D_RangeAttackState stateData, Enemy2 enemy) 
        : base(entity, stateMachine, animationBoolName, attackPosition, stateData)
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

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (isPlayerInMinAgroRange)
                stateMachine.ChangeState(_enemy.PlayerDetectedState);
            else
                stateMachine.ChangeState(_enemy.LookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}