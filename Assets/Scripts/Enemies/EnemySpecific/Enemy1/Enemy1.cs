using UnityEngine;

public class Enemy1 : Entity
{
    public E1_IdleState IdleState { get; private set; }
    public E1_MoveState MoveState { get; private set; }
    public E1_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E1_ChargeState ChargeState { get; private set; }
    public E1_LookForPlayerState LookForPlayerState { get; private set; }
    public E1_MeleeAttackState MeleeAttackState { get; private set; }
    public E1_StunState StunState { get; private set; }
    public E1_DeadState DeadState { get; private set; }

    [Space]
    [SerializeField] private D_Idle idleStateData;
    [SerializeField] private D_Move moveStateData;
    [SerializeField] private D_PlayerDetected playerDetectedStateData;
    [SerializeField] private D_Charge chargeStateData;
    [SerializeField] private D_LookForPlayer lookForPlayerStateData;
    [SerializeField] private D_MeleeAttack meleeAttackStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;

    [Space]
    [SerializeField] private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        MoveState = new E1_MoveState(this, StateMachine, "move", moveStateData, this);
        IdleState = new E1_IdleState(this, StateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new E1_PlayerDetectedState(this, StateMachine, "playerDetected", playerDetectedStateData, this);
        ChargeState = new E1_ChargeState(this, StateMachine, "charge", chargeStateData, this);
        LookForPlayerState = new E1_LookForPlayerState(this, StateMachine, "lookForPlayer", lookForPlayerStateData, this);
        MeleeAttackState = new E1_MeleeAttackState(this, StateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        StunState = new E1_StunState(this, StateMachine, "stun", stunStateData, this);
        DeadState = new E1_DeadState(this, StateMachine, "dead", deadStateData, this);

        StateMachine.Initialize(MoveState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if(isDead)
            StateMachine.ChangeState(DeadState);
        else if (isStunned && StateMachine.CurrentState != StunState)
            StateMachine.ChangeState(StunState);
    }
}