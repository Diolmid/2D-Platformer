using UnityEngine;

public class AttackState : State
{
    protected bool isAnimationFinished;
    protected bool isPlayerInMinAgroRange;

    protected Transform attackPosition;

    public AttackState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, Transform attackPosition) 
        : base(entity, stateMachine, animationBoolName)
    {
        this.attackPosition = attackPosition;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        entity.AnimationToStateMachine.attackState = this;
        isAnimationFinished = false;

        entity.SetVelocity(0);
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

    public virtual void TriggerAttack()
    {

    }

    public virtual void FinishAttack()
    {
        isAnimationFinished = true;
    }
}