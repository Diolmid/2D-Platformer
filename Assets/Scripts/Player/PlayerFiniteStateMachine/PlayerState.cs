using UnityEngine;

public class PlayerState
{
    private string _animationBoolName;

    protected float startTime;

    protected bool isAnimationFinished;
    protected bool isExitingState;
    
    protected Player player;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerData playerData;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animationBoolName)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.playerData = playerData;
        _animationBoolName = animationBoolName;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        player.Animator.SetBool(_animationBoolName, true);
        isAnimationFinished = false;
        isExitingState = false;
        
        DoChecks();
    }

    public virtual void Exit()
    {
        player.Animator.SetBool(_animationBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() { }
    
    public virtual void AnimationTrigger(){}

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}