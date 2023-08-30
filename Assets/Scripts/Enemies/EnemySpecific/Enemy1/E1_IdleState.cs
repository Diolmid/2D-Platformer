using UnityEngine;

public class E1_IdleState : IdleState
{
    private Enemy1 _enemy;

    public E1_IdleState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_Idle stateData, Enemy1 enemy)
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

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(_enemy.PlayerDetectedState);
        }
        else if (isIdleTimeOver)
            stateMachine.ChangeState(_enemy.MoveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}