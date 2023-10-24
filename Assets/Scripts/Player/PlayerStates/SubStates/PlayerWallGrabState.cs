using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 _holdPosition;
    
    public PlayerWallGrabState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animationBoolName) : base(player, playerStateMachine, playerData, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _holdPosition = player.transform.position;
        
        HoldPosition();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        HoldPosition();
        
        if (yInput > 0)
        {
            playerStateMachine.ChangeState(player.WallClimbState);
        }
        else if (yInput < 0 && !grabInput)
        {
            playerStateMachine.ChangeState(player.WallSlideState);
        }
    }

    private void HoldPosition()
    {
        player.transform.position = _holdPosition;
        
        player.SetVelocityX(0);
        player.SetVelocityY(0);
    }
}
