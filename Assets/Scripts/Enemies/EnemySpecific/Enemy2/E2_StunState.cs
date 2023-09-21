using UnityEngine;

public class E2_StunState : StunState
{
    private Enemy2 _enemy;

    public E2_StunState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_StunState stateData, Enemy2 enemy) 
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

        if(isStunTimeOver)
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
}