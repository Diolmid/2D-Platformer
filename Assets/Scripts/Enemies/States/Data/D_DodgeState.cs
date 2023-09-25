using UnityEngine;

[CreateAssetMenu(fileName = "newDodgeStateData", menuName = "Data/State Data/Dodge State")]
public class D_DodgeState : ScriptableObject
{
    public float dodgeSpeed = 10;
    public float dodgeTime = 0.2f;
    public float dodgeCooldown = 2;
    public Vector2 dodgeAngle;
}