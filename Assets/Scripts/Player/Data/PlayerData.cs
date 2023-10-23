using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")] 
    public float movementVelocity = 10;

    [Header("Jump State")] 
    public int jumpAmount = 1;
    public float jumpVelocity = 15;

    [Header("In Air State")] 
    public float coyoteTime = .2f;
    public float variableJumpHeightMultiplier = .5f;
    
    [Header("Check Variables")] 
    public float groundCheckRadius =.3f;
    public LayerMask whatIsGround;
}
