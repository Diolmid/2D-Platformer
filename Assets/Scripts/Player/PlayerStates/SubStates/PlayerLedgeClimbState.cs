using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private int _xInput;
    private int _yInput;

    private bool _isHanging;
    private bool _isClimbing;

    private Vector2 _detectedPosition;
    private Vector2 _cornerPosition;
    private Vector2 _startPosition;
    private Vector2 _stopPosition;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animationBoolName) : base(player, playerStateMachine, playerData, animationBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.Animator.SetBool("climbLedge", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        _isHanging = true;
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityZero();
        player.transform.position = _detectedPosition;
        _cornerPosition = player.DetermineCornerPosition();

        _startPosition.Set(_cornerPosition.x - (player.FacingDirection * playerData.startOffset.x), _cornerPosition.y - playerData.startOffset.y);
        _stopPosition.Set(_cornerPosition.x + (player.FacingDirection * playerData.startOffset.x), _cornerPosition.y + playerData.stopOffset.y);

        player.transform.position = _startPosition;
    }

    public override void Exit()
    {
        base.Exit();

        _isHanging = false;

        if (_isClimbing)
        {
            player.transform.position = _stopPosition;
            _isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            playerStateMachine.ChangeState(player.IdleState);
        }
        else
        {
            _xInput = player.InputHandler.NormInputX;
            _yInput = player.InputHandler.NormInputY;

            player.SetVelocityZero();
            player.transform.position = _startPosition;

            if(_xInput == player.FacingDirection && _isHanging && !_isClimbing)
            {
                _isClimbing = true;
                player.Animator.SetBool("climbLedge", true);
            }
            else if(_yInput == -1 && _isHanging && !_isClimbing)
            {
                playerStateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public void SetDetectedPosition(Vector2 position) => _detectedPosition = position;
}
