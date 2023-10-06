using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    
    public PlayerGroundedState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animationBoolName) 
        : base(player, playerStateMachine, playerData, animationBoolName)
    {
        
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

        xInput = player.InputHandler.NormInputX;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }
}
