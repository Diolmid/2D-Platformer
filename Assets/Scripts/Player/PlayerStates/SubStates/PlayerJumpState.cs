using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int _jumpAmountLeft;
    
    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animationBoolName) 
        : base(player, playerStateMachine, playerData, animationBoolName)
    {
        _jumpAmountLeft = playerData.jumpAmount;
    }

    public override void Enter()
    {
        base.Enter();
        
        player.SetVelocityY(playerData.jumpVelocity);
        isAbilityDone = true;
        DecreaseJumpAmountLeft();
        player.InAirState.SetIsJumping();
    }

    public void DecreaseJumpAmountLeft() => _jumpAmountLeft--;
    
    public void ResetJumpAmountLeft() => _jumpAmountLeft = playerData.jumpAmount;
    
    public bool CanJump()
    {
        return _jumpAmountLeft > 0;
    }
}
