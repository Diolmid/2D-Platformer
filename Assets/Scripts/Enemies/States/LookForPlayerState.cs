using UnityEngine;

public class LookForPlayerState : State
{
    protected int amountOfTurnsDone;
    protected float lastTurnTime;
    protected bool isAllTurnsDone;
    protected bool isAllTurnsTimeDone;

    protected bool isPlayerInMinAgroRange;
    protected bool turnImmediately;

    protected D_LookForPlayer stateData;

    public LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animationBoolName, D_LookForPlayer stateData) 
        : base(entity, stateMachine, animationBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        isAllTurnsDone = false;
        isAllTurnsTimeDone = false;

        lastTurnTime = StartTime;
        amountOfTurnsDone = 0;

        entity.SetVelocity(0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (turnImmediately)
        {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
            turnImmediately = false;
        }
        else if(Time.time >= lastTurnTime + stateData.timeBetweenTurns && !isAllTurnsDone)
        {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
        }

        isAllTurnsDone = amountOfTurnsDone >= stateData.amountOfTurns;
        isAllTurnsTimeDone = Time.time >= lastTurnTime + stateData.timeBetweenTurns && isAllTurnsDone;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetTurnImmediately(bool flip)
    {
        turnImmediately = flip;
    }
}