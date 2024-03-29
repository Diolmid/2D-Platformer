using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/State Data/Idle State")]
public class D_Idle : ScriptableObject
{
    public float minIdleTime = 1;
    public float maxIdleTime = 2;
}