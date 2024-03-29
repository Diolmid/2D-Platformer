using UnityEngine;

public class E1_ChargeState : ChargeState
{
    private Enemy1 _enemy;

    public E1_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_Charge stateData, Enemy1 enemy) 
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
        else if (!isDetectedLedge || isDetectedWall)
            stateMachine.ChangeState(_enemy.LookForPlayerState);
        else if (isChargeTimeOver)
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