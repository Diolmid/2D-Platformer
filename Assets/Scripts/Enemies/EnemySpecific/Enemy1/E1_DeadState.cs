using UnityEngine;

public class E1_DeadState : DeadState
{
    private Enemy1 _enemy;

    public E1_DeadState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_DeadState stateData, Enemy1 enemy) : base(entity, stateMachine, animationBoolName, stateData)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}