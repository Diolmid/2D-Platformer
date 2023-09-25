using UnityEngine;

[CreateAssetMenu(fileName = "newRangeAttackStateData", menuName = "Data/State Data/Range Attack State")]
public class D_RangeAttackState : ScriptableObject
{
    public float projectileDamage = 10;
    public float projectileSpeed = 12;
    public float projectileTravelDistance = 5;
    public GameObject projectile;
}