using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float maxHealth = 30;

    [Space]
    public float damageHopSpeed = 3;

    [Space]
    public float stunResistance = 3;
    public float stunRecoveryTime = 2;

    [Space]
    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;
    public float groundCheckRadius = 0.3f;

    [Space]
    public float minAgroDistance = 3;
    public float maxAgroDistance = 4;

    [Space]
    public float closeRangeActionDistance = 1;

    [Space]
    public GameObject hitParticle;

    [Space]
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
}