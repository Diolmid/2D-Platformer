using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animationBoolName) : base(player, playerStateMachine, playerData, animationBoolName)
    {
    }
    
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        player.SetVelocityY(-playerData.wallSlideVelocity);

        if (grabInput && yInput == 0)
        {
            playerStateMachine.ChangeState(player.WallGrabState);
        }
    }
}
