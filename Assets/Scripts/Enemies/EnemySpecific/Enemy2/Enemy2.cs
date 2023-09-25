using UnityEngine;

public class Enemy2 : Entity
{
    public E2_MoveState MoveState { get; private set; }
    public E2_IdleState IdleState { get; private set; }
    public E2_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E2_MeleeAttackState MeleeAttackState { get; private set; }
    public E2_LookForPlayerState LookForPlayerState { get; private set; }
    public E2_StunState StunState { get; private set; }
    public E2_DeadState DeadState { get; private set; }
    public E2_DodgeState DodgeState { get; private set;}
    public E2_RangeAttackState RangeAttackState { get; private set; }

    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private Transform rangeAttackPosition;

    [Space]
    public D_DodgeState dodgeStateData;
    [SerializeField] private D_Move moveStateData;
    [SerializeField] private D_Idle idleStateData;
    [SerializeField] private D_PlayerDetected playerDetectedStateData;
    [SerializeField] private D_MeleeAttack meleeAttackStateData;
    [SerializeField] private D_LookForPlayer lookForPlayerStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;
    [SerializeField] private D_RangeAttackState rangeAttackStateData;
 
    public override void Start()
    {
        base.Start();

        MoveState = new E2_MoveState(this, StateMachine, "move", moveStateData, this);
        IdleState = new E2_IdleState(this, StateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new E2_PlayerDetectedState(this, StateMachine, "playerDetected", playerDetectedStateData, this);
        MeleeAttackState = new E2_MeleeAttackState(this, StateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        LookForPlayerState = new E2_LookForPlayerState(this, StateMachine, "lookForPlayer", lookForPlayerStateData, this);
        StunState = new E2_StunState(this, StateMachine, "stun", stunStateData, this);
        DeadState = new E2_DeadState(this, StateMachine, "dead", deadStateData, this);
        DodgeState = new E2_DodgeState(this, StateMachine, "dodge", dodgeStateData, this);
        RangeAttackState = new E2_RangeAttackState(this, StateMachine, "rangeAttack", rangeAttackPosition, rangeAttackStateData, this);

        StateMachine.Initialize(MoveState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (isDead)
            StateMachine.ChangeState(DeadState);
        else if (isStunned && StateMachine.CurrentState != StunState)
            StateMachine.ChangeState(StunState);
        else if (CheckPlayerInMinAgroRange())
            StateMachine.ChangeState(RangeAttackState);
        else if (!CheckPlayerInMinAgroRange())
        {
            LookForPlayerState.SetTurnImmediately(true);
            StateMachine.ChangeState(LookForPlayerState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}