using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private int _xInput;

    private float _startWallJumpCoyoteTime;
    
    private bool _coyoteTime;
    private bool _wallJumpCoyoteTime;
    private bool _jumpInput;
    private bool _jumpInputStop;
    private bool _grabInput;
    private bool _isJumping;
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isTouchingWallBack;
    private bool _oldIsTouchingWall;
    private bool _oldIsTouchingWallBack;
    
    public PlayerInAirState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animationBoolName) 
        : base(player, playerStateMachine, playerData, animationBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();
        
        _xInput = player.InputHandler.NormInputX;
        _jumpInput = player.InputHandler.JumpInput;
        _jumpInputStop = player.InputHandler.JumpInputStop;
        _grabInput = player.InputHandler.GrabInput;

        CheckJumpMultiplier();
        
        if (_isGrounded && player.CurrentVelocity.y < .01f)
        {
            playerStateMachine.ChangeState(player.LandState);
        }
        else if (_jumpInput && (_isTouchingWall || _isTouchingWallBack || _wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            _isTouchingWall = player.CheckIfTouchingWall();
            player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
            playerStateMachine.ChangeState(player.WallJumpState);
        }
        else if (_jumpInput && player.JumpState.CanJump())
        { 
            playerStateMachine.ChangeState(player.JumpState);
        }
        else if (_isTouchingWall && _grabInput)
        {
            playerStateMachine.ChangeState(player.WallGrabState);
        }
        else if (_isTouchingWall && _xInput == player.FacingDirection && player.CurrentVelocity.y <= 0)
        {
            playerStateMachine.ChangeState(player.WallSlideState);
        }
        else
        {
            player.CheckIfShouldFlip(_xInput);
            player.SetVelocityX(playerData.movementVelocity * _xInput);

            player.Animator.SetFloat("yVelocity", player.CurrentVelocity.y);
            player.Animator.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _oldIsTouchingWall = _isTouchingWall;
        _oldIsTouchingWallBack = _isTouchingWallBack;
        
        _isGrounded = player.CheckIfGrounded();
        _isTouchingWall = player.CheckIfTouchingWall();
        _isTouchingWallBack = player.CheckIfTouchingWallBack();

        if (!_wallJumpCoyoteTime && !_isTouchingWall && !_isTouchingWallBack &&
            (_oldIsTouchingWall || _oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    private void CheckJumpMultiplier()
    {
        if (_isJumping)
        {
            if (_jumpInputStop)
            {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                _isJumping = false;
            }
            else if(player.CurrentVelocity.y <= 0)
            {
                _isJumping = false;
            }
        }
    }
    
    private void CheckCoyoteTime()
    {
        if (_coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            _coyoteTime = false;
            player.JumpState.DecreaseJumpAmountLeft();
        }
    }
    
    private void CheckWallJumpCoyoteTime()
    {
        if (_wallJumpCoyoteTime && Time.time > _startWallJumpCoyoteTime + playerData.coyoteTime)
        {
            _wallJumpCoyoteTime = false;
            player.JumpState.DecreaseJumpAmountLeft();
        }
    }

    public void StartCoyoteTime() => _coyoteTime = true;
    
    public void StartWallJumpCoyoteTime()
    {
        _wallJumpCoyoteTime = true;
        _startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => _wallJumpCoyoteTime = false;

    public void SetIsJumping() => _isJumping = true;
}
