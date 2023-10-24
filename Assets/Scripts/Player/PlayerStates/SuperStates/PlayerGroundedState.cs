using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;

    private bool _jumpInput;
    private bool _grabInput;
    private bool _isGrounded;
    private bool _isTouchingWall;
    
    public PlayerGroundedState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animationBoolName) 
        : base(player, playerStateMachine, playerData, animationBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        player.JumpState.ResetJumpAmountLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        _jumpInput = player.InputHandler.JumpInput;
        _grabInput = player.InputHandler.GrabInput;

        if (_jumpInput && player.JumpState.CanJump())
        {
            player.InputHandler.UseJumpInput();
            playerStateMachine.ChangeState(player.JumpState);
        }
        else if (!_isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            playerStateMachine.ChangeState(player.InAirState);
        }
        else if(_isTouchingWall && _grabInput)
        {
            playerStateMachine.ChangeState(player.WallGrabState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = player.CheckIfGrounded();
        _isTouchingWall = player.CheckIfTouchingWall();
    }
}
