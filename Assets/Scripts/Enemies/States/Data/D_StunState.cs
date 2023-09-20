using UnityEngine;

[CreateAssetMenu(fileName = "newStunStateData", menuName = "Data/State Data/Stun State")]
public class D_StunState : ScriptableObject
{
    public float stunTime = 3;
    public float stunKnockbackTime = 0.2f;
    public float stunKnockbakSpeed = 20;

    public Vector2 stunKnockbackAngle;
}