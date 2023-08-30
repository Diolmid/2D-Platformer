using UnityEngine;

public class State
{
    protected float startTime;

    protected string animationBoolName;

    protected Entity entity;
    protected FiniteStateMachine stateMachine;

    public State( Entity entity, FiniteStateMachine stateMachine, string animationBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animationBoolName = animationBoolName;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        entity.Animator.SetBool(animationBoolName, true);
        DoChecks();
    }

    public virtual void Exit() 
    {
        entity.Animator.SetBool(animationBoolName, false);
    }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() 
    {
        DoChecks();
    }

    public virtual void DoChecks() { }
}