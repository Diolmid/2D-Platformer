using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animationBoolName) 
        : base(player, playerStateMachine, playerData, animationBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isExitingState)
            return;
        
        if (xInput != 0)
        {
            playerStateMachine.ChangeState(player.MoveState);
        }
        else if (isAnimationFinished)
        {
            playerStateMachine.ChangeState(player.IdleState);
        }
    }
}
