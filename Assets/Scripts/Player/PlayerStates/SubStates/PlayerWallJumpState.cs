using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int _wallJumpDirection;
    
    public PlayerWallJumpState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animationBoolName) : base(player, playerStateMachine, playerData, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.InputHandler.UseJumpInput();
        player.JumpState.ResetJumpAmountLeft();
        player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, _wallJumpDirection);
        player.CheckIfShouldFlip(_wallJumpDirection);
        player.JumpState.DecreaseJumpAmountLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Animator.SetFloat("yVelocity", player.CurrentVelocity.y);
        player.Animator.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));

        isAbilityDone = Time.time >= startTime + playerData.wallJumpTime;
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        _wallJumpDirection = isTouchingWall ? -player.FacingDirection : player.FacingDirection;
    }
}
