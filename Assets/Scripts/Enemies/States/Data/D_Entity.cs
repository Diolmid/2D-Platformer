using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;

    [Space]
    public float minAgroDistance = 3;
    public float maxAgroDistance = 4;

    [Space]
    public float closeRangeActionDistance = 1;

    [Space]
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
}